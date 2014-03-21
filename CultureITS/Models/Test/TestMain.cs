using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Test
{
    public class TestMain
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name="Заголовок")]
        public string Title { set; get; }

        [Required]
        [Display(Name = "Тема")]
        public string Topic { set; get; }

        [Required]
        [Display(Name = "Автор")]
        public string Author { set; get; }

        public virtual ICollection<Question> Questions { set; get; }
        public virtual ICollection<Session> Sessions { set; get; }
    }
}