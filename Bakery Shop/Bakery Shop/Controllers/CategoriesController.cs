using Bakery_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bakery_Shop.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Categories()
        {
            return View(db.categories.ToList());
        }

        [HttpPost]
        public ActionResult FindCategoryProduct(string Category)
        {
            var Categories = db.categories.ToList();
            var AllProducts = db.products.ToList();

            if (Categories != null)
            {
                ViewBag.Category = Categories;
            }
            var RequestedCategory = db.products.Where(x => x.Category == Category).ToList();
            if (RequestedCategory.Count > 0)
            {
                return View(RequestedCategory);
            }
            if (Category == "All")
            {
                return View(AllProducts);
            }
            else
                return View(RequestedCategory);
        }

        public ActionResult NewCategory()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                db.categories.Add(category);
                db.SaveChanges();
            }
            return Json(category.CategoryName, JsonRequestBehavior.AllowGet);
        }
    }
}