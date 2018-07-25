using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyApp.Persistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.WebApi
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyAppDbContext>
    {
        public MyAppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MyAppDbContext>();

            var connectionString = configuration.GetConnectionString("MyAppDatabase");

            builder.UseSqlServer(connectionString);

            return new MyAppDbContext(builder.Options);
        }
    }
}
