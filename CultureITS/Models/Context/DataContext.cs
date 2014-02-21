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
                context.SaveChanges();
            }
        }
    }
}