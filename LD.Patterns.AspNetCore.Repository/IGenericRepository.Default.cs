﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace LD.Patterns.AspNetCore.Repository
{
    public abstract class GenericRepository<TContext> : IGenericRepository
        where TContext : DbContext
    {
        public GenericRepository(TContext context)
        {
            Context = context;
        }

        public TContext Context { get; }

        public DatabaseFacade Database => Context.Database;

        public DbConnection Connection => Context.Database.GetDbConnection();

        public virtual void Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove<TEntity>(TEntity entity)
            where TEntity : class
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual Task SaveChangesAsync() => Context.SaveChangesAsync();

        public void RunInTransaction(
            Action<DbConnection, DbTransaction> action, IsolationLevel? isolationLevel = null)
        {
            RunInTransaction((connection, transaction) =>
            {
                action(connection, transaction);
                return true;
            }, isolationLevel);
        }

        public T RunInTransaction<T>(
            Func<DbConnection, DbTransaction, T> func, IsolationLevel? isolationLevel = null)
        {
            T result;

            if (Database.CurrentTransaction != null)
            {
                result = func(Connection, Database.CurrentTransaction.GetDbTransaction());
            }
            else
            {
                using (var contextTransaction = CreateTransaction(isolationLevel))
                {
                    try
                    {
                        result = func(Connection, contextTransaction.GetDbTransaction());
                        contextTransaction.Commit();
                    }
                    catch
                    {
                        contextTransaction.Rollback();
                        throw;
                    }
                }
            }

            return result;
        }

        public Task RunInTransactionAsync(
            Func<DbConnection, DbTransaction, Task> func, IsolationLevel? isolationLevel = null)
        {
            return RunInTransactionAsync(async (connection, transaction) =>
            {
                await func(connection, transaction);
                return true;
            }, isolationLevel);
        }

        public async Task<T> RunInTransactionAsync<T>(
            Func<DbConnection, DbTransaction, Task<T>> func, IsolationLevel? isolationLevel = null)
        {
            T result;

            if (Database.CurrentTransaction != null)
            {
                result = await func(Connection, Database.CurrentTransaction.GetDbTransaction());
            }
            else
            {
                using (var contextTransaction = CreateTransaction(isolationLevel))
                {
                    try
                    {
                        result = await func(Connection, contextTransaction.GetDbTransaction());
                        contextTransaction.Commit();
                    }
                    catch
                    {
                        contextTransaction.Rollback();
                        throw;
                    }
                }
            }

            return result;
        }

        public void Reload<T>(T entity)
            where T : class
        {
            Context.Entry(entity).Reload();
        }

        public Task ReloadAsync<T>(T entity)
            where T : class
        {
            return Context.Entry(entity).ReloadAsync();
        }

        public void SetState<T>(T entity, EntityState state)
            where T : class
        {
            Context.Entry(entity).State = state;
        }

        public void SetPropertyModified<T>(T entity, string propertyName)
            where T : class
        {
            Context.Entry(entity).Property(propertyName).IsModified = true;
        }

        public void SetPropertyModified<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
            where T : class
        {
            Context.Entry(entity).Property(property).IsModified = true;
        }

        public void Load<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
            where T : class
            where TProperty : class
        {
            Context.Entry(entity).Reference(property).Load();
        }

        public Task LoadAsync<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
            where T : class
            where TProperty : class
        {
            return Context.Entry(entity).Reference(property).LoadAsync();
        }

        public void Load<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> property)
            where T : class
            where TElement : class
        {
            Context.Entry(entity).Collection(property.Name).Load();
        }

        public Task LoadAsync<T, TElement>(T entity, Expression<Func<T, IEnumerable<TElement>>> property)
            where T : class
            where TElement : class
        {
            return Context.Entry(entity).Collection(property).LoadAsync();
        }

        public IQueryable<TElement> Query<T, TElement>(T entity, Expression<Func<T, IEnumerable<TElement>>> property)
            where T : class
            where TElement : class
        {
            return Context.Entry(entity).Collection(property).Query();
        }

        public virtual void Dispose()
        {
            Context.Dispose();
        }

        private IDbContextTransaction CreateTransaction(IsolationLevel? isolationLevel)
        {
            return
                isolationLevel == null ?
                Database.BeginTransaction() :
                Database.BeginTransaction(isolationLevel.Value);
        }
    }
}
