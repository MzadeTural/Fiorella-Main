using Fiorella_second.DAL;
using Fiorella_second.Models;
using Fiorella_second.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Controllers
{
    public class ProductController : Controller
    {
        public AppDbContext _context { get; }
        private string _skpCnt { get; set; }
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            ViewBag.productsCount = _context.Products.Where(c => c.IsDeleted == false).Count();
            return View(_context.Products
                                .Where(c => c.IsDeleted == false)
                                .Include(p => p.Images)
                                .OrderByDescending(p => p.Id)
                                .Take(2));
        }
        public IActionResult LoadProduct(int skip)
        {

            _skpCnt = _context.Settings.Where(s => s.Key == "TakeCount").FirstOrDefault().Value;
            int count = Convert.ToInt32(_skpCnt);
            var model = _context.Products
                                .OrderByDescending(p => p.Id)
                                .Skip(skip)
                                .Take(count)
                                .Include(p => p.Images)
                                .ToList();
            return PartialView("_ProductPartial", model);

        }
        public List<BasketVM> getBasket()
        {
            List<BasketVM> basket;
            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }
            return basket;
        }
        public async Task<IActionResult> AddBasket( int? id)
        {
            
            var model = _context.Products                                                   
                                 .Include(p => p.Images)
                                .ToList();

            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) BadRequest();
            List<BasketVM> basket = getBasket();
            UpdateBasket((int)id, basket);
            return RedirectToAction("Basket", "Product");
           // return PartialView("_ProductPartial", model);
        }
        public async Task<IActionResult> Basket()
        {
            List<BasketVM> basket = getBasket();
            List<BasketItemVM> model = await GetBasketList(basket);
            return View(model);
        }
        private void UpdateBasket( int id, List<BasketVM> basket)
        {
            BasketVM IsExist = basket.Find(p => p.Id ==id);
            if (IsExist == null)
            {
                basket.Add(new BasketVM
                {
                    Id =id,
                    Count = 1
                });
            }
            else
            {
                IsExist.Count += 1;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
        }
      
       private BasketItemVM getBasketItem(BasketVM item, Product dbProduct)
        {
            return new BasketItemVM
                {
                    Id = item.Id,
                    Name = dbProduct.Title,
                    Count = item.Count,
                   StockCount=dbProduct.StockCount,
                    Image = dbProduct.Images.Where(i => i.IsMain).FirstOrDefault().ImageUrl,
                    Price = dbProduct.Price,
                    IsActive = dbProduct.IsDeleted
                };
            
          

        }
        private async Task<List<BasketItemVM>> GetBasketList(List<BasketVM> basket)
        {
            List<BasketItemVM> model = new List<BasketItemVM>();
            foreach (BasketVM item in basket)
            {
                Product dbProduct = await _context.Products
                                                  .Include(p => p.Images)
                                                  .FirstOrDefaultAsync(p => p.Id == item.Id);
                BasketItemVM ItemVM = getBasketItem(item, dbProduct);
                model.Add(ItemVM);

            }
            return model;
        }

    }
    
}
