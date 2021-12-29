using Fiorella_second.Areas.AdminFiorella.ViewModel;
using Fiorella_second.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Areas.AdminFiorella.Controllers
{
    [Area("AdminFiorella")]
    public class TablesController : Controller
    {
        private AppDbContext _context { get; }
        public TablesController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           TablesVM TablesVM=new TablesVM
            {
                Categories = await _context.Categories                                  
                                    .ToListAsync(),
                Experts = await _context.Experts
                                      .Where(e => e.IsDeleted == false)
                                       .Include(e => e.ExpertImage)
                                      .ToListAsync(),
                Products = await _context.Products                                   
                                     .Include(c => c.CategoryName)
                                     .Include(c => c.Images)
                                     .OrderByDescending(p => p.Id)
                                     .ToListAsync(),
                ProductImages = await _context.Images
                                      .ToListAsync(),

            };

            return View(TablesVM);
        }
    }
}
