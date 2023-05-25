﻿using ASP_111.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP_111.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("asp111");
        }
    }
}
