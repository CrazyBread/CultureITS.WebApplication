using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models
{
    public enum AccountStatus
    {
        None, Student, Teacher, Admin
    }

    public class UserLogin
    {
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        public string Password { set; get; }
    }

    public class User
    {
        [Key]
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        public string Password { set; get; }

        [Required]
        [Display(Name = "Роль")]
        public AccountStatus UserRole { set; get; }

        [Required(ErrorMessage = "Введите ваше настоящее имя")]
        [Display(Name = "ФИО")]
        public string Name { set; get; }
    }
}