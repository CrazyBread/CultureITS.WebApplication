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
        [Display(Name = "Идентификатор")]
        public string Code { set; get; }

        [Required]
        [AllowHtml]
        [Display(Name = "Описание")]
        public string Description { set; get; }

        [Required]
        [Display(Name = "Местоположение")]
        public string Location { set; get; }

        public string ApplicationType { set; get; }
        public byte[] ApplicationData { set; get; }

        public virtual ICollection<Student> Students { set; get; }
        public virtual ICollection<Article> Article { set; get; }
    }
}