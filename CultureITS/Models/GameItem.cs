using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models
{
    public class GameItem
    {
        [Key]
        public int Id { set; get; }

        [Required]
        public string Name { set; get; }

        public virtual ICollection<Student> Students { set; get; }
    }
}