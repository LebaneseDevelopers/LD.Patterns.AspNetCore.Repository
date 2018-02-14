using System.Linq;
using Basic.Models;
using LD.Patterns.AspNetCore.Repository;

namespace Basic.Services
{
    public class Repository : GenericRepository<AppDbContext>, IRepository
    {
        public Repository(AppDbContext context)
            : base(context)
        {
        }

        public IQueryable<Blog> Blogs => Context.Blogs;

        public IQueryable<Post> Posts => Context.Posts;
    }
}
