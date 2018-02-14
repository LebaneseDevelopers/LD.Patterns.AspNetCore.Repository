using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LD.Patterns.AspNetCore.Repository
{
    public static class GenericRepositoryExtensions
    {
        public static Task AddAsync<TEntity>(this IGenericRepository @this, TEntity entity)
            where TEntity : class
        {
            @this.Add(entity);
            return @this.SaveChangesAsync();
        }

        public static void AddRange<TEntity>(this IGenericRepository @this, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                @this.Add(entity);
            }
        }

        public static Task AddRangeAsync<TEntity>(this IGenericRepository @this, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            @this.AddRange(entities);
            return @this.SaveChangesAsync();
        }

        public static Task UpdateAsync<TEntity>(this IGenericRepository @this, TEntity entity)
            where TEntity : class
        {
            @this.Update(entity);
            return @this.SaveChangesAsync();
        }

        public static void UpdateRange<TEntity>(this IGenericRepository @this, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                @this.Update(entity);
            }
        }

        public static Task UpdateRangeAsync<TEntity>(this IGenericRepository @this, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            @this.UpdateRange(entities);
            return @this.SaveChangesAsync();
        }

        public static Task RemoveAsync<TEntity>(this IGenericRepository @this, TEntity entity)
            where TEntity : class
        {
            @this.Remove(entity);
            return @this.SaveChangesAsync();
        }

        public static void RemoveRange<TEntity>(this IGenericRepository @this, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                @this.Remove(entity);
            }
        }

        public static Task RemoveRangeAsync<TEntity>(this IGenericRepository @this, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            @this.RemoveRange(entities);
            return @this.SaveChangesAsync();
        }

        public static void SetState<TEntity>(this IGenericRepository @this, IEnumerable<TEntity> entities, EntityState state)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                @this.SetState(entity, state);
            }
        }

        public static void Detach<TEntity>(this IGenericRepository @this, TEntity entity)
            where TEntity : class
        {
            @this.SetState(entity, EntityState.Detached);
        }

        public static void Detach<TEntity>(this IGenericRepository @this, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                @this.Detach(entity);
            }
        }
    }
}
