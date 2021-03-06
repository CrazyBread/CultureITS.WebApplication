﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Test
{
    public class Session
    {
        [Key]
        public int Id { set; get; }

        public string Data { set; get; }
        public DateTime Date { set; get; }
        public double Percent { set; get; }

        public bool Complete { set; get; }
        public int QuestionsLeft { set; get; }

        [Required]
        public virtual Student Student { set; get; }
        [Required]
        public virtual TestMain Test { set; get; }
    }
}