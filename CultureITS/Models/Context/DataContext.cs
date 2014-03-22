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

        public DbSet<TestMain> TestMain { set; get; }
        public DbSet<Question> TestQuestion { set; get; }
        public DbSet<Answer> TestAnswer { set; get; }
        public DbSet<Session> TestSessions { set; get; }

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
                context.Users.Add(new Student() { Login = "student", Password = "student", Name = "Студент студентыч", UserRole = AccountStatus.Student, Age = 19, Course = 3, Group = "ИСТбд-32" });
                context.SaveChanges();

                context.MenuItems.Add(new MenuItem() { Order = 0, Title = "В музей!", Controller = "Unity", Action = "Index", AccessMask = 1 << (int)AccountStatus.Student });
                context.MenuItems.Add(new MenuItem() { Order = 1, Title = "Профиль", Controller = "Account", Action = "Profile", AccessMask = -2 });
                context.MenuItems.Add(new MenuItem() { Order = 2, Title = "Экспонаты", Controller = "Exhibit", Action = "Index", AccessMask = -2 });
                context.MenuItems.Add(new MenuItem() { Order = 0, Title = "Администрирование", Area = "Admin", Controller = "Home", Action = "Index", AccessMask = 1 << (int)AccountStatus.Admin });
                context.MenuItems.Add(new MenuItem() { Order = 3, Title = "API", Controller = "Api", Action = "Index", AccessMask = 1 << (int)AccountStatus.Admin });
                context.MenuItems.Add(new MenuItem() { Order = 0, Title = "О системе", Controller = "Home", Action = "About", AccessMask = -1 });
                context.SaveChanges();

                context.Exhibits.Add(new Exhibit() { Code = "S1", Name = "Свиток Судьбы", Location = "Зал католицизма", Description = "<p>Какая-та херь</p>", CanNotified = false });
                context.Exhibits.Add(new Exhibit() { Code = "S2", Name = "Свиток Верности", Location = "Зал православия", Description = "<p>Какая-та неведомая херь</p>", CanNotified = true, FullDescription = "<p>123</p><p>1231231</p>" });
                context.SaveChanges();

                var test = new TestMain() { Code = "T1", Author = "Бобик Ёбик", Title = "Проверочный тест", Topic = "Публичная жизни Вручтель Серафимы" };
                context.TestMain.Add(test);
                var question1 = new Question() { Test = test, Text = "Как зовут кошку Симы?" };
                var question2 = new Question() { Test = test, Text = "У Симы есть кошка?" };
                context.TestQuestion.Add(question1);
                context.TestQuestion.Add(question2);
                context.TestAnswer.Add(new Answer() { Question = question1, Right = true, Text = "Люська." });
                context.TestAnswer.Add(new Answer() { Question = question1, Right = false, Text = "Муська." });
                context.TestAnswer.Add(new Answer() { Question = question1, Right = false, Text = "Пуська." });
                context.TestAnswer.Add(new Answer() { Question = question1, Right = false, Text = "Барабуська." });
                context.TestAnswer.Add(new Answer() { Question = question2, Right = true, Text = "Конечно!" });
                context.TestAnswer.Add(new Answer() { Question = question2, Right = true, Text = "Нет." });
                context.TestAnswer.Add(new Answer() { Question = question2, Right = false, Text = "А что это?" });
                context.SaveChanges();
            }
        }
    }
}
