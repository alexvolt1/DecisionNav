using System;
using System.Collections.Generic;
using System.Text;
using DecisionNav.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DecisionNav.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Query<NavigationList>().ToView("vNavigationList");
        }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<NavigationItem_View> NavigationItem_View { get; set; }
        public virtual DbQuery<NavigationList> NavigationList { get; set; }

    }
}
