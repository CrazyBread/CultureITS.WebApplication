using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Test
{
    public class Question
    {
        [Key]
        public int Id { set; get; }

        public string Text { set; get; }

        public string ApplicationType { set; get; }
        public byte[] ApplicationData { set; get; }

        public virtual TestMain Test { set; get; }
        public virtual ICollection<Answer> Answers { set; get; }
    }
}