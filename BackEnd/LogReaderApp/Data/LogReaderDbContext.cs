using LogReaderApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogReaderApp.Data
{
    public class LogReaderDbContext : DbContext
    {
        public LogReaderDbContext(DbContextOptions<LogReaderDbContext> options) : base(options) { }
        public LogReaderDbContext() { }

        public DbSet<Log> Log { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => 
            options.UseNpgsql(@"User ID =postgres;Password=123qwe;Server=localhost;Port=5432;Database=DBLogReader;Integrated Security=true;Pooling=true; ",
                opt => opt.MaxBatchSize(6));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Nome = "User 1", IP = "200.139.123.784" },
                    new User { Id = 2, Nome = "User 2", IP = "200.139.123.888" });

        }
    }
}
