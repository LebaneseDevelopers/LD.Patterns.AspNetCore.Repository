# LD.Patterns.AspNetCore.Repository

Provides a base when implementing the repository pattern with EF Core.

## Objectives

- Quickly implement the pattern with little to no plumbing or boilerplate on your end.
- Ease out unit testing by providing some kind of an in memory repository store.

To that end:

- Have a base repository class that handles most of the plumbing (like implementing Add, Remove, Update, SaveChanges, Transactions, ...).
- Have another base repository class that in memory implementations should inherit from. It should basically be an in memory store for entities that also provides things like ensuring (int, long) PKs get a proper incrementing value when entities are added.

## The basics

`IGenericRepository`, `GenericRepository`, and `InMemoryGenericRepository` are provided for your (real) Repository classes to inherit from. They do most of the work.

## A simple example

```cs
// This is your service.
public interface IRepository : IGenericRepository
{
    IQueryable<Blog> Blogs { get; }
}
```

```cs
// This is the real repository that stores entities in a database.
public class Repository : GenericRepository<ApplicationDbContext>, IRepository
{
    public Repository(ApplicationDbContext context) : base(context) { }

    IQueryable<Blog> Blogs => Context.Blogs;

    // Add, Remove, Update, SaveChangesAsync are all implemented in RepositoryCore.
}
```

```cs
// This is the in memory repository that will depend on InMemoryRepositoryCore to store entities in memory.
public class InMemoryRepository : InMemoryGenericRepository, IRepository
{
    // For() is a method on InMemoryRepositoryCore
    IQueryable<Blog> Blogs => For<Blog>();

    // Add, Remove, Update, SaveChangesAsync are all implemented in InMemoryRepositoryCore.
}
```

Note: The `InMemoryRepository` requires the PK property to be called "Id" for auto incrementing to work.

## Nice improvements to have

- Relationship fixups for the in memory store.
