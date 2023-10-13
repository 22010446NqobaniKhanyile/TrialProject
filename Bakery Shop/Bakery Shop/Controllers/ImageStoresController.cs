using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bakery_Shop.Models;

namespace Bakery_Shop.Controllers
{
    public class ImageStoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ImageStores
        public ActionResult Index()
        {
            return View(db.ImageStores.ToList());
        }

        // GET: ImageStores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageStore imageStore = db.ImageStores.Find(id);
            if (imageStore == null)
            {
                return HttpNotFound();
            }
            return View(imageStore);
        }
        [HttpGet]
        // GET: ImageStores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageStores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public ActionResult Create( ImageStore imageModel)
        {

            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.ImageStores.Add(imageModel);
                db.SaveChanges();
                return RedirectToAction("Edit", "ImageStores");
            }
            ModelState.Clear();

            return View();
        }

        // GET: ImageStores/Edit/5
        public ActionResult Edit(int? id = 1)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageStore fQA = db.ImageStores.Find(id);
            if (fQA == null)
            {
                return HttpNotFound();
            }
            return View(fQA);
        }
        [HttpPost]

        public ActionResult Edit(ImageStore imageModel)
        {


            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(imageModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "ImageStores");
            }
            ModelState.Clear();

            return View();

        }

        // GET: ImageStores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageStore imageStore = db.ImageStores.Find(id);
            if (imageStore == null)
            {
                return HttpNotFound();
            }
            return View(imageStore);
        }

        // POST: ImageStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImageStore imageStore = db.ImageStores.Find(id);
            db.ImageStores.Remove(imageStore);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
