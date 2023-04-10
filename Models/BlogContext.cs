using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A10BlogsPosts.Models
{
    public class BlogContext : DbContext // The DbContext inheritance is automatically there because of EntityFrameworkCore
    {
        public DbSet<Blog> Blogs { get; set; } //Instead of List<>, using DbSet<> instead
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Will create tables listed above if they don't exist
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("BloggingContext"));

        }
    }
}
