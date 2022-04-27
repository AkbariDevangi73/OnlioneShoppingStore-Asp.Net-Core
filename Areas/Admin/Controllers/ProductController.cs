using ECOMMERCENEW.Data;
using ECOMMERCENEW.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMMERCENEW.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductController(ApplicationDbContext db,IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c=>c.ProductTypes).ToList());
        }

        //POST Index Method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var prod = _db.Products.Include(c => c.ProductTypes).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                prod = _db.Products.Include(c => c.ProductTypes).ToList();
            }
            return View(prod);
        }

        //Get Create Method
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            return View();
        }

        //Post Create Method
        [HttpPost]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> Create(Products prod,IFormFile image)
        {
            if(ModelState.IsValid)
            {
                var serchProduct = _db.Products.FirstOrDefault(c => c.Name == prod.Name);
                if (serchProduct != null)
                {
                    ViewBag.message = "This Product is already exit";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    return View(prod);
                }
                if (image!=null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    prod.Image = "Images/" + image.FileName;
                }
                if(image==null)
                {
                    prod.Image = "Images/noimg.png";
                }
                _db.Products.Add(prod);
               await _db.SaveChangesAsync();
                TempData["save"] = "Product type has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(prod);
        }
        
        //Edit Get Action Method
        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            if(id==null)
            {
                return NotFound();
            }
            var prod = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if(prod==null)
            {
                return NotFound();
            }
            return View(prod);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Products prod, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    prod.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    prod.Image = "Images/noimg.png";
                }
                _db.Products.Update(prod);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product type has been Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(prod);
        }

        //GET Details Action Method
        public ActionResult Details(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var prod = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if(prod==null)
            {
                return NotFound();
            }
            return View(prod);
        }

        //GET Delete Action Method
        public ActionResult Delete(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var prod = _db.Products.Include(c => c.ProductTypes).Where(c => c.Id == id).FirstOrDefault();
            if(prod==null)
            {
                return NotFound();
            }
            return View(prod);
        }

        //POST Delete Action Method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var prod = _db.Products.FirstOrDefault(c => c.Id == id);
            if(prod==null)
            {
                return NotFound();
            }
            _db.Products.Remove(prod);
            await _db.SaveChangesAsync();
            TempData["delete"] = "Product type has been Deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
