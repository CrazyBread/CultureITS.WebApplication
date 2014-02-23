using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Models
{
    public class AccessRight
    {
        [Key]
        public int Id { set; get; }

        [Required]
        public string Controller { set; get; }

        [Required]
        public string Action { set; get; }

        [Required]
        public AccountStatus Role { set; get; }

        [Required]
        public Boolean IsAllowed { set; get; }
    }
}