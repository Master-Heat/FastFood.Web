
using FastFood.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;       
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Reposiory
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
            base(options)
        {
        }
            public DbSet<ApplicationUser> ApplicationUsers { get; set; }
            public DbSet<Cart> Carts{ get; set; }
             public DbSet<Category> Categories { get; set; }
            
            public DbSet<Coupon>Coupons{ get; set; }
            public DbSet<Item> Items{ get; set; }
            public DbSet<OrderDetails> OrderDetails{ get; set; }
            public DbSet<OrderHeader> OrderHeaders{ get; set; }
            public DbSet<SubCategory> SubCategories{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Item -> SubCategory: Restrict delete
            modelBuilder.Entity<Item>()
                .HasOne(i => i.SubCategory)
                .WithMany()
                .HasForeignKey(i => i.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item -> Category: Restrict delete
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // SubCategory -> Category: Restrict delete
            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }

}

