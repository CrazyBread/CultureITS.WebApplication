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

        public string Text { set; get; }

        public string ApplicationType { set; get; }
        public byte[] ApplicationData { set; get; }

        public bool Right { set; get; }

        public virtual Question Question { set; get; }
    }
}