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

        public DbSet<GameObject> GameObjects { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Administrator>().ToTable("Administrators");
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<Student>().ToTable("Students");
        }
    }

    public class DataContextInitializer : IDatabaseInitializer<DataContext>
    {
        public void InitializeDatabase(DataContext context)
        {
            if (!context.Database.Exists() || !context.Database.CompatibleWithModel(false))
            {
                context.Database.Delete();
                context.Database.Create();

                // Adding data
                context.Users.Add(new Administrator() { Login = "admin", Password = "admin", Name = "Админ админыч", UserRole = AccountStatus.Admin, Telephone = "322-22-23", Email = "admin@localhost" });
                context.Users.Add(new Teacher() { Login = "teacher", Password = "teacher", Name = "Учитель учителич", UserRole = AccountStatus.Teacher, University = "УлГТУ", Department = "IVK" });
                context.Users.Add(new Student() { Login = "student", Password = "student", Name = "Студент студентыч", UserRole = AccountStatus.Student });
                context.SaveChanges();

                context.MenuItems.Add(new MenuItem() { Order = 1, Title = "Профиль", Controller = "Account", Action = "Index", AccessMask = 1 << (int)AccountStatus.Student });
                context.MenuItems.Add(new MenuItem() { Order = 0, Title = "Администрирование", Area = "Admin", Controller = "Home", Action = "Index", AccessMask = 1 << (int)AccountStatus.Admin });
                context.MenuItems.Add(new MenuItem() { Order = 2, Title = "API", Controller = "Api", Action = "Index", AccessMask = 1 << (int)AccountStatus.Admin });
                context.MenuItems.Add(new MenuItem() { Order = 0, Title = "О системе", Controller = "Home", Action = "About", AccessMask = -1 });
                context.SaveChanges();

                context.GameObjects.Add(new GameObject() { Name = "Свиток Судьбы", Location = "Зал католицизма", Description = "<p>Какая-та херь</p>", CanNotified = false });
                context.GameObjects.Add(new GameObject() { Name = "Свиток Верности", Location = "Зал православия", Description = "<p>Какая-та неведомая херь</p>", CanNotified = true, FullDescription = "<p>123</p><p>1231231</p>" });
                context.SaveChanges();
            }
        }
    }
}