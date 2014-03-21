using CultureITS.Models.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models
{
    public enum AccountStatus
    {
        None,
        [Description("Студент")]
        Student,
        [Description("Преподаватель")]
        Teacher,
        [Description("Администратор")]
        Admin
    }

    public class UserLogin
    {
        [Required(ErrorMessage = "Введите e-mail в качестве логина")]
        [Display(Name = "E-mail")]
        public string Login { set; get; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        public string Password { set; get; }
    }

    public class User
    {
        [Key]
        public int Id { set; get; }

        [Required(ErrorMessage = "Введите e-mail в качестве логина")]
        [Display(Name = "E-mail")]
        public string Login { set; get; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        public string Password { set; get; }

        [Required]
        [Display(Name = "Роль")]
        public AccountStatus UserRole { set; get; }

        [Required(ErrorMessage = "Введите ваше настоящее имя")]
        [Display(Name = "ФИО")]
        public string Name { set; get; }

        public byte[] Photo { set; get; }
        public string PhotoMime { set; get; }
    }

    public class Student : User
    {
        [Display(Name = "Курс")]
        public int Course { set; get; }

        [Display(Name = "Группа")]
        public string Group { set; get; }

        [Display(Name = "Возраст")]
        public int Age { set; get; }

        public virtual ICollection<Exhibit> Exhibits { set; get; }
        public virtual ICollection<Session> TestSessions { set; get; }
    }

    public class Teacher : User
    {
        [Required(ErrorMessage = "Введите название учебного заведения")]
        [Display(Name = "Учебное завеление")]
        public string University { set; get; }

        [Required(ErrorMessage = "Введите название кафедры или иного структурного подразделения")]
        [Display(Name = "Кафедра/подраздедение")]
        public string Department { set; get; }
    }

    public class Administrator : User
    {
        [Required(ErrorMessage = "Введите контактный телефон")]
        [Display(Name = "Контактный телефон")]
        public string Telephone { set; get; }

        [Required(ErrorMessage = "Введите контактный email")]
        [Display(Name = "Контактный email")]
        public string Email { set; get; }
    }
}