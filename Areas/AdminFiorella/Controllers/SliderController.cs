using Fiorella_second.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fiorella_second.Areas.AdminFiorella.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Fiorella_second.Models;
using Fiorella_second.Utilities;
using Microsoft.AspNetCore.Hosting;
using Fiorella_second.Utilities.File;
using System.Collections.Generic;
using Fiorella_second.Services;
using System;
using Fiorella_second.ViewModel.Sliders;

namespace Fiorella_second.Areas.AdminFiorella.Controllers
{   [Area("AdminFiorella")]
    public class SliderController : Controller
    {
        private AppDbContext _context { get;}
        private IWebHostEnvironment _env {get;}
        private string _errorMessage;
        private int _maxCount=5;
        private LayoutServices _service { get; }

        public SliderController(AppDbContext context,IWebHostEnvironment env, LayoutServices service)
        {
            _context = context;
            _env = env;
            _service = service;
          
        }
        // GET: SliderController
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> settings = _service.GetSetting();
            int maxCount = Convert.ToInt32(settings["Slider_Max_Count"]);
            var setting = _context.Settings.AsEnumerable().ToDictionary(s => s.Key, s => s.Value);
            int sldCount = _context.Sliders.Count();
            TempData["SldCount"] = sldCount;
            TempData["MaxCount"] = maxCount;
            TablesVM TablesVM = new TablesVM
            {
        
            Sliders = await _context.Sliders.ToListAsync()
             };
            return View(TablesVM);
        }

        // GET: SliderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SliderController/Create
        public ActionResult Create()
        {
            Dictionary<string, string> settings = _service.GetSetting();
            int maxCount = Convert.ToInt32(settings["Slider_Max_Count"]);
            var setting = _context.Settings.AsEnumerable().ToDictionary(s => s.Key, s => s.Value);
            int sldCount = _context.Sliders.Count();
            TempData["SldCount"] = sldCount;
            TempData["MaxCount"] = maxCount;

            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MultipleSliderVM sliderVM)
        {

            #region MyRegion
            //Dictionary<string, string> settings = _service.GetSetting();
            //int size = Convert.ToInt32(settings["MaxImageSize"]);
            //var setting = _context.Settings.AsEnumerable().ToDictionary(s => s.Key, s => s.Value);
            //if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            //foreach (var item in collection)
            //{

            //}
            //if (!slider.Photo.CheckFileType("image/"))
            //{
            //    //  ModelState.AddModelError("Photo", "File must be image type");
            //    throw new FieldAccessException("File must be image type");
            //    // return View();
            //}
            //if (!slider.Photo.CheckFileSize(size))
            //{
            //    ModelState.AddModelError($"Photo", "File size must be lest then " + $"{size}kb");
            //    return View();
            //}
            //string fileName = await slider.Photo.SaveFileAysnc(_env.WebRootPath, "img");
            //slider.ImgUrl = fileName;
            #endregion
           
            if (ModelState["Photos"].ValidationState == ModelValidationState.Invalid) return View();

           
            if (!CheckImageValid(sliderVM.Photos))
            {
                ModelState.AddModelError("Photos", _errorMessage);
                return View();
            }
            foreach (var photo in sliderVM.Photos)
            {
               
                string fileName = await photo.SaveFileAysnc(_env.WebRootPath, "img");
                Slider slider = new Slider()
                {
                    ImgUrl = fileName
                };
                await _context.Sliders.AddAsync(slider);

            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
                if (photos.Count>_maxCount-sldCount)
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

        // GET: SliderController/Edit/5

        public IActionResult Update(int id)
        {
           
            Slider slider = _context.Sliders.Find(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Slider slider)
        {
            try
            {
                Dictionary<string, string> settings = _service.GetSetting();
                int size = Convert.ToInt32(settings["MaxImageSize"]);
                int maxCount = Convert.ToInt32(settings["Slider_Max_Count"]);
                var setting = _context.Settings.AsEnumerable().ToDictionary(s => s.Key, s => s.Value);

                if (id != slider.Id) return BadRequest();
                if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
                Slider dbslider = await _context.Sliders.FindAsync(id);
                if (dbslider == null) return NotFound();
                if (!slider.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError($"Photo", "File must be image type");
                    // throw new FieldAccessException("File must be image type");
                    return View(dbslider);
                }
                if (!slider.Photo.CheckFileSize(size))
                {
                    ModelState.AddModelError($"Photo", "File size must be lest then " + $"{size}kb");
                    return View(dbslider);
                }
                Helper.RemoveFile(_env.WebRootPath, "img", dbslider.ImgUrl);
                string newFileName = await slider.Photo.SaveFileAysnc(_env.WebRootPath, "img");
                dbslider.ImgUrl = newFileName;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SliderController/Delete/5
      

        // POST: SliderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Slider slider =  await _context.Sliders.FindAsync(id);
                if (slider == null) return NotFound();
                Helper.RemoveFile(_env.WebRootPath, "img", slider.ImgUrl);
                _context.Sliders.Remove(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
