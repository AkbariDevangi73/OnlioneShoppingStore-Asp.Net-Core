using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECOMMERCENEW.Data;
using ECOMMERCENEW.Models;
using Microsoft.AspNetCore.Authorization;

namespace ECOMMERCENEW.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db;
        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            //var data = _db.ProductTypes.ToList();
            return View(_db.ProductTypes.ToList());
        }

        //Create Get Action Method
        public IActionResult Create()
        {
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes prod)
          {
            if(ModelState.IsValid)
             {
                _db.ProductTypes.Add(prod);
                await _db.SaveChangesAsync();
                TempData["Save"] = "Product type has been saved";
                return RedirectToAction(nameof(Index));
             }
            return View(prod);
         }

        //Edit Get Action Method
        public IActionResult Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if(productType==null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Edit Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes prod)
        {
            if (ModelState.IsValid)
            {
                _db.Update(prod);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product type has been Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(prod);
        }

        //Details Get Action Method
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Details Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes prod)
        {
                return RedirectToAction(nameof(Index));
           
        }


        //Delete Get Action Method
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Delete Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,ProductTypes prod)
        {
            if(id==null)
            {
                return NotFound();
            }

            if(id!=prod.Id)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);
            if(productType==null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Product type has been Deleted";
                return RedirectToAction(nameof(Index));
            }
            return View(prod);
        }

    }
}
