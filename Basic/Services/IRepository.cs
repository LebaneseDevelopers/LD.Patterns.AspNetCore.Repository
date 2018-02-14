using System.Linq;
using Basic.Models;
using LD.Patterns.AspNetCore.Repository;

namespace Basic.Services
{
    public interface IRepository : IGenericRepository
    {
        IQueryable<Blog> Blogs { get; }

        IQueryable<Post> Posts { get; }
    }
}
