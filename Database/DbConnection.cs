using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Database
{
    public class DbConnection : DbContext
    {
        public DbConnection(DbContextOptions<DbConnection> options)
            : base(options) { }

        public DbSet<Address> Addresses => Set<Address>();
    }
}