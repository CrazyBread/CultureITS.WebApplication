using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Models
{
    public class Article
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { set; get; }

        [Required]
        [AllowHtml]
        [Display(Name = "Текст")]
        public string Text { set; get; }

        public string ApplicationType { set; get; }
        public byte[] ApplicationData { set; get; }

        [Required]
        public virtual Exhibit Exhibit { set; get; }
    }
}