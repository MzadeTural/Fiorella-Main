using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
