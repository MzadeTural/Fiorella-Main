using Fiorella_second.DAL;
using Fiorella_second.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiorella_second.Areas.AdminFiorella.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Areas.AdminFiorella.Controllers
{
    [Area("AdminFiorella")]
    public class CategoryController : Controller
    {
        private AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            TablesVM TablesVM = new TablesVM
            {
                Categories=await _context.Categories.Where(c => c.IsDeleted == false)
                                                       .Include(i=>i.Products)
                                                      .ToListAsync()


            };
            return View(TablesVM);
        }

        // GET: CategoryController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();
            bool IsExist = _context.Categories
                                 .Any(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View();
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Create));
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
       
        public IActionResult Update(int id)
        {
                       Category category = _context.Categories.Find(id);
                if (category == null) return NotFound();            
                return View(category);           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Update(int id ,Category category)
        {
            if (!ModelState.IsValid) return View();
            if (id != category.Id) return BadRequest();
            Category dbCategory = await _context.Categories.Where(c => c.IsDeleted == false && c.Id == id).FirstOrDefaultAsync();
            if (dbCategory == null) return NotFound();
            if (dbCategory.Name.ToLower().Trim() == category.Name.ToLower().Trim())
            {
                return RedirectToAction(nameof(Index));
            }
            bool IsExist = _context.Categories
                                .Any(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View(dbCategory);
            }
            dbCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")] 
        public async Task<IActionResult> DeletePost( int id )
        {
          
            Category dbCategory = await _context.Categories
                                 .Where(c=>c.IsDeleted==false&&c.Id == id).FirstOrDefaultAsync();

            if (dbCategory == null) return (NotFound());
            dbCategory.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
                
        }
    }
}
