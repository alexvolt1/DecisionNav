using System;
using System.Collections.Generic;
using System.Text;
using DecisionNav.Models;
using DecisionNav.Models.BankVars;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DecisionNav.Data
{
    public class BankVarsDbContext : IdentityDbContext
    {
        public BankVarsDbContext(DbContextOptions<BankVarsDbContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Query<NavigationList>().ToView("vNavigationList");
        //}
        public virtual DbSet<Bank> Bank { get; set; }


    }
}
