using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Test
{
    public class Answer
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Текст ответа")]
        public string Text { set; get; }

        [Required]
        [Display(Name = "Ответ правильный")]
        public bool Right { set; get; }

        public string ApplicationType { set; get; }
        public byte[] ApplicationData { set; get; }

        [Required]
        public virtual Question Question { set; get; }
    }
}