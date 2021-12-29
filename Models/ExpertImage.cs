using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Models
{
    public class ExpertImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public Expert Expert { get; set; }
        public bool IsDeleted { get; set; }
    }
}
