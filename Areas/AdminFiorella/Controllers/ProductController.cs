using Fiorella_second.DAL;
using Fiorella_second.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Fiorella_second.Areas.AdminFiorella.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fiorella_second.ViewModel.Products;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Fiorella_second.Utilities;
using Microsoft.AspNetCore.Hosting;
using Fiorella_second.Services;
using Fiorella_second.ViewModel;

namespace Fiorella_second.Areas.AdminFiorella.Controllers
{
    [Area("AdminFiorella")]
    public class ProductController : Controller
    {
        private AppDbContext _context;
        private string _errorMessage;
        private LayoutServices _service { get; }
        private IWebHostEnvironment _env { get; }

        public ProductController(AppDbContext context, IWebHostEnvironment env, LayoutServices service)
        {
            _context = context;
            _env = env;
            _service = service;
        }
        // GET: ProductController
        public async Task<IActionResult> Index( int page=1,int take=10)
        {
            TempData["Take"] = take;
            var products = await _context.Products
                                    .Where(p=>p.IsDeleted==false)
                                    .Skip((page - 1) * take)
                                    .Take(take)
                                    .OrderByDescending(p => p.Id)
                                   .Include(c => c.CategoryName)
                                   .Include(c => c.Images)
                                   .ToListAsync();
            var productsVM = getProductList(products);
            int pageCount = getPageCount(take);
            Paginate<ProductListVM> model = new Paginate<ProductListVM>(productsVM,page, pageCount);
           // return Json(model);
            return View(model);
        }
        private int getPageCount(int take)
        {
            var productCount =  _context.Products
                                   .Where(p => p.IsDeleted == false).Count();
            return (int)Math.Ceiling(((decimal)productCount / take));
        }

        private List<ProductListVM> getProductList(List<Product> products)
        {
            List<ProductListVM> model = new List<ProductListVM>();
            foreach (var item in products)
            {
                var product = new ProductListVM
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price,
                    StockCount = item.StockCount,
                    CategoryName = item.CategoryName.Name,
                    Image = item.Images.Where(i => i.IsMain).FirstOrDefault().ImageUrl
                };
                model.Add(product);
            }
            return model;
        }

        // GET: ProductController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync(),"Id","Name");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            if (!ModelState.IsValid) return View();
            bool IsExist = _context.Products
                                 .Any(c => c.Title.ToLower().Trim() == productCreateVM.Name.ToLower().Trim());
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This Product already exist");
                return RedirectToAction(nameof(Index));
            }
            Product product = new Product
            {
                
                StockCount = productCreateVM.Count,
                Price = productCreateVM.Price,
                CategoryId = productCreateVM.CategoryId,
                Title = productCreateVM.Name

            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            if (ModelState["Photos"].ValidationState == ModelValidationState.Invalid) return View();


            if (!CheckImageValid(productCreateVM.Photos))
            {
                ModelState.AddModelError("Photos", _errorMessage);
                return View();
            }
            foreach (var photo in productCreateVM.Photos)
            {

                string fileName = await photo.SaveFileAysnc(_env.WebRootPath, "img");
                ProductImage productImage = new ProductImage()
                {                   
                  ProductId=product.Id,
                   ImageUrl =fileName

                };
               
                await _context.Images.AddAsync(productImage);

            }
           
           
          
            await _context.SaveChangesAsync();
          
            return RedirectToAction(nameof(Create));

        }

        private bool CheckImageValid(List<IFormFile> photos)
        {
            Dictionary<string, string> settings = _service.GetSetting();
            int size = Convert.ToInt32(settings["MaxImageSize"]);
            int maxCount = Convert.ToInt32(settings["Slider_Max_Count"]);
            var setting = _context.Settings.AsEnumerable().ToDictionary(s => s.Key, s => s.Value);
            int count = 1;
            int sldCount = _context.Sliders.Count();
            foreach (var photo in photos)
            {
                if (photos.Count > 5 - sldCount)
                {
                    _errorMessage = $"You can choose a maximum of {maxCount} photo ";
                    return false;
                }

                if (!photo.CheckFileType("image/"))
                {
                    _errorMessage = $"{photo.FileName} must be image type";
                    return false;
                }
                if (!photo.CheckFileSize(size))
                {
                    _errorMessage = $"{photo.FileName} size must be lest then " + $"{size}kb";
                    return false;
                }
                count++;
            }
            if (count > 5)
            {
                _errorMessage = $"must be 5 image ";
                return false;
            }

            return true;
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
