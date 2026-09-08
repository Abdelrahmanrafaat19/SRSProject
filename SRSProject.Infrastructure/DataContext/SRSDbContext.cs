using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SRSProject.Domain.Entities;
using SRSProject.Infrastructure.DataContext.IDentityEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.DataContext
{
    public class SRSDbContext : IdentityDbContext<UserEntity>
    {
        public SRSDbContext(DbContextOptions<SRSDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SRSDbContext).Assembly);
        }
        public DbSet<EmployeeEntity> Employees { get; set; }
    }
}
