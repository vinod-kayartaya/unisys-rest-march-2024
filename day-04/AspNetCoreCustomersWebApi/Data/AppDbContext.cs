using AspNetCoreCustomersWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreCustomersWebApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions):  base(dbContextOptions)
        {
        }

        // one or more object-relation mappings
        public DbSet<Customer> Customers { get; set; } // provides CRUD+Query operations on records of a table in DB
    }
}
