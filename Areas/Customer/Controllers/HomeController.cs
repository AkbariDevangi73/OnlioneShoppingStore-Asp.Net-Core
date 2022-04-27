using ECOMMERCENEW.Data;
using ECOMMERCENEW.Models;
using ECOMMERCENEW.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ECOMMERCENEW.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index(int? page)
        {
            return View(_db.Products.Include(c=>c.ProductTypes).ToList().ToPagedList(pageNumber:page??1,pageSize:9));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //GET product Details Action Method
        public IActionResult Details(int? id)
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
            return View(prod  );
        }

        //POST product Details Action Method
        [HttpPost]
        [ActionName("Details")]
        public IActionResult ProductDetails(int? id)
        {
            List<Products>products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var prod = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if (prod == null)
            {
                return NotFound();
            }
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products==null)
            {
                products = new List<Products>();
            }
            products.Add(prod);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }

        //GET Remove Action Method
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //GET product Cart action method
        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products==null)
            {
                products = new List<Products>();
            }
            return View(products);
        }

    }
}
