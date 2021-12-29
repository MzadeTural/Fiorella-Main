using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Models
{
    public class ComentSlider
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ImageId { get; set; }
        public ExpertImage ExpertImages { get; set; }
        public int ExpertId { get; set; }
        public Expert Expert { get; set; }
        public bool Isdeleted { get; set; }
    }
}
