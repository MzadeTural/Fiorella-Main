using Fiorella_second.Models;
using System.Collections.Generic;

namespace Fiorella_second.ViewModel
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public IntroSlider SliderIntro { get; set; }
        public About About { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<AboutList> AboutList { get; set; }
        public List<Expert> Experts { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<ComentSlider> Coments { get; set; }
    }
}
