using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Models
{
    public class Expert
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public int ExpertImageId { get; set; }
        public ExpertImage ExpertImage { get; set; }
        //   public ICollection<ComentSlide> Coments { get; set; }

        public bool IsDeleted { get; set; }

    }
}
