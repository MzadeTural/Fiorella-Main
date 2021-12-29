using Fiorella_second.DAL;
using Fiorella_second.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context { get; set; }
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVm = new HomeVM
            {
                Sliders = await _context.Sliders.ToListAsync(),
                Products = await _context.Products
                                     .Where(c => c.IsDeleted == false)
                                     .Include(c => c.CategoryName)
                                     .Include(c => c.Images)
                                     .OrderByDescending(p => p.Id)
                                     .Take(8)
                                     .ToListAsync(),
                Categories = await _context.Categories
                                    .Where(t => t.IsDeleted == false)
                                    .ToListAsync(),
                SliderIntro = await _context.IntroSliders
                                           .FirstOrDefaultAsync(),
                About = await _context.About
                                       .FirstOrDefaultAsync(),
                AboutList = await _context.AboutList
                .Where(c => c.IsDeleted == false)
                .ToListAsync(),
                Experts = await _context.Experts
                                      .Where(e => e.IsDeleted == false)
                                       .Include(e => e.ExpertImage)
                                       .Include(e=>e.Position)
                                      .ToListAsync(),
                Blogs = await _context.Blogs
                                      .Where(e => e.IsDeleted == false)
                                     .ToListAsync(),
                Coments = await _context.Coments
                                      .Where(c => c.Isdeleted == false)
                                       .Include(e => e.ExpertImages)
                                      .ToListAsync(),


            };
            return View(homeVm);
        }
    }
}
