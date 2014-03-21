using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Models
{
    public class Exhibit
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Название")]
        public string Name { set; get; }

        [Required]
        [AllowHtml]
        [Display(Name = "Описание")]
        public string Description { set; get; }

        [Required]
        [Display(Name = "Местоположение")]
        public string Location { set; get; }

        [Required]
        [Display(Name = "Может быть отмечено пользователем")]
        public bool CanNotified { set; get; }

        [Display(Name = "Полное описание")]
        [AllowHtml]
        public string FullDescription { set; get; }

        public virtual ICollection<Student> Students { set; get; }
    }
}