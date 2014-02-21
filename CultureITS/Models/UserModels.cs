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

    public class Login
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string UserName { set; get; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        public string Password { set; get; }
    }

    public class User
    {
        [Key]
        public string UserName { set; get; }

        [Required]
        public string Password { set; get; }

        [Required]
        public AccountStatus UserRole { set; get; }

        [Required]
        public string Name { set; get; }
    }
}