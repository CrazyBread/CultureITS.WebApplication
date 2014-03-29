using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { set; get; }

        [Display(Name = "Заголовок")]
        [Required]
        public string Title { set; get; }

        [Display(Name = "Область")]
        public string Area { set; get; }

        [Display(Name = "Контроллер")]
        [Required]
        public string Controller { set; get; }

        [Display(Name = "Действие")]
        [Required]
        public string Action { set; get; }

        [Display(Name = "Дополнительные параметры ссылки (якорь, параметры)")]
        public string AdditionalUrl { set; get; }

        [Display(Name = "Порядок")]
        [Required]
        public int Order { set; get; }

        [Display(Name = "Маска доступа")]
        [Required]
        public short AccessMask { set; get; }
    }
}