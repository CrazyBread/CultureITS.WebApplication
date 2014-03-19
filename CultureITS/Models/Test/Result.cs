using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Test
{
    public class Result
    {
        [Key]
        public int Id { set; get; }

        public string Data { set; get; }
        public DateTime Date { set; get; }
        public double Percent { set; get; }

        public bool Complete { set; get; }
        public int QuestionsLeft { set; get; }

        public Student Student { set; get; }
        public TestMain Test { set; get; }
    }
}