using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Infrastructure.Persistence.SqlServer
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(
              "Server=(localdb)\\MSSQLLocalDB;Database=STAFF_TRAINING_PLATFORM;Integrated Security=True;TrustServerCertificate=true;MultipleActiveResultSets=true",
               b => b.MigrationsAssembly("YachayPeru.Infrastructure"));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
