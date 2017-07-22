using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nicoblogproject.Models;

namespace nicoblogproject.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<EmailList> EmailList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            modelBuilder.Entity<Article>().ToTable("Article");
            modelBuilder.Entity<Images>().ToTable("Images");
            modelBuilder.Entity<EmailList>().ToTable("EmailList");
        }
    }
}
