using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models
{
    public class GameObject
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Название")]
        public string Name { set; get; }
        
        [Required]
        [Display(Name = "Описание")]
        public string Description { set; get; }

        [Required]
        [Display(Name = "Может быть отмечено пользователем")]
        public bool CanNotified { set; get; }

        [Display(Name = "Полное описание")]
        public string FullDescription { set; get; }
    }
}