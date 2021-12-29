using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        public DateTime? CreatedTime { get; set; }
        [NotMapped,Required]
        public IFormFile Photo { get; set; }
    }
}
