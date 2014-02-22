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
        public DbSet<MenuItem> MenuItems { set; get; }
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
                context.Users.Add(new User() { UserName = "admin", Password = "admin", Name = "Админ админыч", UserRole = AccountStatus.Admin });
                context.Users.Add(new User() { UserName = "teacher", Password = "teacher", Name = "Учитель учителич", UserRole = AccountStatus.Teacher });
                context.Users.Add(new User() { UserName = "student", Password = "student", Name = "Студент студентыч", UserRole = AccountStatus.Student });
                context.SaveChanges();

                context.MenuItems.Add(new MenuItem() { Order = 1, Title = "Профиль", Controller = "Account", Action = "Index", AccessMask = 1 << (int)AccountStatus.Student });
                context.MenuItems.Add(new MenuItem() { Order = 1, Title = "Администрирование", Controller = "Admin", Action = "Index", AccessMask = 1 << (int)AccountStatus.Admin });
                context.SaveChanges();
            }
        }
    }
}