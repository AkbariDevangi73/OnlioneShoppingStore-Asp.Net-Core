using ECOMMERCENEW.Data;
using ECOMMERCENEW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMMERCENEW.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
            UserManager<IdentityUser> _userManager;
            ApplicationDbContext _db;

            public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
            {
                _userManager = userManager;
                _db = db;
            }

       public IActionResult Index()
        {
            return View(_db.ApplicationUSers.ToList());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user,role:"User");
                    TempData["Save"] = "User Has Been Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if(user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult>Edit(ApplicationUser user)
        {
            var userinfo = _db.ApplicationUSers.FirstOrDefault(c => c.Id == user.Id);
            if(userinfo==null)
            {
                return NotFound();
            }
            userinfo.FirstName=user.FirstName;
            userinfo.LastName = user.LastName;
            var result =await _userManager.UpdateAsync(userinfo);
            if(result.Succeeded)
            {
                TempData["edit"] = "User Has been Updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userinfo);
        }
        public async Task<IActionResult> Details(string id)
        {
            var user = _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
      
        public async Task<IActionResult> Locout(string id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if(user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Locout(ApplicationUser user)
        {
            var userinfo = _db.ApplicationUSers.FirstOrDefault(c => c.Id == user.Id);
            if(userinfo==null)
            {
                return NotFound();
            }
            userinfo.LockoutEnd = DateTime.Now.AddYears(100);
            int rowAffected = _db.SaveChanges();
            if(rowAffected>0)
            {
                TempData["locout"] = "User Has Been Locout Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userinfo);
        }

        public async Task<IActionResult>Active(string id)
        {
            var user = _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if(user==null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userinfo = _db.ApplicationUSers.FirstOrDefault(c => c.Id == user.Id);
            if (userinfo == null)
            {
                return NotFound();
            }
            userinfo.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["locout"] = "User Has Been Active Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userinfo);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userinfo = _db.ApplicationUSers.FirstOrDefault(c => c.Id == user.Id);
            if (userinfo == null)
            {
                return NotFound();
            }
            _db.ApplicationUSers.Remove(userinfo);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["locout"] = "User Has Been Delete Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userinfo);
        }


    }
}
