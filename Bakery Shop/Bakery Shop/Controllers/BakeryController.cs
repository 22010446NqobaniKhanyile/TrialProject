using Bakery_Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bakery_Shop.Controllers
{
    public class BakeryController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Bakery
        public ActionResult Shop()
        {
            var Categories = _db.categories.ToList();
            if (Categories != null)
            {
                ViewBag.Category = Categories;
            }
            var Products = _db.products.ToList();
            return View(Products);
        }
        [HttpPost]
        public ActionResult Shop(string SearchWord)
        {
            var product = _db.products.Where(m => m.ProductName.Contains(SearchWord) || SearchWord == null).ToList();
            if (product.Count == 0)
            {
                ViewBag.NoProduct = "Result not found!";
            }
            var Categories = _db.categories.ToList();
            if (Categories != null)
            {
                ViewBag.Category = Categories;
            }
            return View(product);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Products()
        {
            var Products = _db.products.ToList();
            return View(Products);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult NewProduct()
        {
            var Categories = _db.categories.ToList();
            if (Categories != null)
            {
                ViewBag.Category = Categories;
            }
            return View();
        }

        [HttpPost]
        public ActionResult NewProduct(Product product)
        {
            string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
            string extension = Path.GetExtension(product.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            product.Image = "../Content/Products/" + fileName;
            fileName = Path.Combine(Server.MapPath("../Content/Products/"), fileName);
            product.ImageFile.SaveAs(fileName);
            _db.products.Add(product);
            _db.SaveChanges();
            return View("Success");
        }
        //Delete Product
        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProduct(int id)
        {
            Product product = _db.products.Find(id);
            _db.products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Products");
        }
        //Edit Product
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var Categories = _db.categories.ToList();
            if (Categories != null)
            {
                ViewBag.Category = Categories;
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(Product product)
        {
            if(ModelState.IsValid)
            {
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}