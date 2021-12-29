using Fiorella_second.DAL;
using Fiorella_second.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            if (Request.Cookies["basket"]!=null)
            {
                var basket = JsonConvert
                                  .DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
                ViewBag.BasketItemCount = basket.Sum(p => p.Count);
            }
            else
            {
                ViewBag.BasketItemCount = 0;
            }
            var setting = _context.Settings.AsEnumerable().ToDictionary(s => s.Key, s => s.Value);
            return View( await Task.FromResult(setting));
        }
    }
}
