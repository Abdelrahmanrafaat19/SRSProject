using Microsoft.EntityFrameworkCore;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.DataContext
{
    public class SRSDbContext : DbContext
    {
        public SRSDbContext(DbContextOptions<SRSDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SRSDbContext).Assembly);
        }
        public DbSet<EmployeeEntity> Employees { get; set; }
    }
}
