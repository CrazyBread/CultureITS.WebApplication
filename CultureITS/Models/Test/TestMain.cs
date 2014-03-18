﻿using System;
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

        public string Title { set; get; }
        public string Topic { set; get; }
        public string Author { set; get; }

        public virtual ICollection<Question> Questions { set; get; }
        public virtual ICollection<Result> Results { set; get; }
    }
}