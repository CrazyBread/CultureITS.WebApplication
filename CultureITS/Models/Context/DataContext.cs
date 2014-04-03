using CultureITS.Models.Test;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Context
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { set; get; }
        public DbSet<Administrator> Administrators { set; get; }
        public DbSet<Teacher> Teachers { set; get; }
        public DbSet<Student> Students { set; get; }

        public DbSet<MenuItem> MenuItems { set; get; }
        public DbSet<AccessRight> AccessRights { set; get; }

        public DbSet<Exhibit> Exhibits { set; get; }
        public DbSet<Article> Articles { set; get; }

        public DbSet<TestMain> TestMain { set; get; }
        public DbSet<Question> TestQuestion { set; get; }
        public DbSet<Answer> TestAnswer { set; get; }
        public DbSet<Session> TestSessions { set; get; }

        public DbSet<GameItem> GameItems { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Administrator>().ToTable("Administrators");
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<Student>().ToTable("Students");
        }
    }
}
