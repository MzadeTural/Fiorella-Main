using Fiorella_second.DAL;
using Fiorella_second.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Fiorella_second.Areas.AdminFiorella.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fiorella_second.Areas.AdminFiorella.Controllers
{
    [Area("AdminFiorella")]
    public class ProductController : Controller
    {
        private AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            TablesVM TablesVM = new TablesVM
            {
                
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

        // GET: ProductController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid) return View();
            bool IsExist = _context.Products
                                 .Any(c => c.Title.ToLower().Trim() == product.Title.ToLower().Trim());
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This Product already exist");
                return RedirectToAction(nameof(Index));
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            //foreach (var item in product)
            //{
            //    MovieGenre movieGenre = new MovieGenre()
            //    {
            //        GenreId = item,
            //        MovieId = movie.MovieId,
            //        Genre = _context.Genre.Where(x => x.GenreId == item).FirstOrDefault(),
            //        Movie = movie
            //    };
            //    _context.MovieGenre.Add(movieGenre);
            //}
            //_context.SaveChanges();
            //return View();

            return RedirectToAction(nameof(Create));

        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {

            Product dbProducts = await _context.Products
                                 .Where(c => c.IsDeleted == false && c.Id == id).FirstOrDefaultAsync();

            if (dbProducts == null) return (NotFound());
            dbProducts.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
