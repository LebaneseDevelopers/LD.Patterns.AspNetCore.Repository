using System.Linq;
using Basic.Models;
using LD.Patterns.AspNetCore.Repository;

namespace Basic.Services
{
    public class InMemoryRepository : InMemoryGenericRepository, IRepository
    {
        public IQueryable<Blog> Blogs => For<Blog>();

        public IQueryable<Post> Posts => For<Post>();
    }
}
