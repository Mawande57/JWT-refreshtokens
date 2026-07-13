using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using JWTtesting.Entities;

namespace JWTtesting.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
