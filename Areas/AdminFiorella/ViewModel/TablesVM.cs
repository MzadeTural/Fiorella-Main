using Fiorella_second.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Areas.AdminFiorella.ViewModel
{
    public class TablesVM
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Settings> Settings { get; set; }
        public List<Expert> Experts { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<Slider> Sliders { get; set; }
    }
}
