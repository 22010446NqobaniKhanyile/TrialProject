using Bakery_Shop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bakery_Shop.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }



        public ActionResult Ordercancelationrequests()
        {
            var c = db.Ordercancelations.ToList();

            return View(c);
        }



        [HttpGet]
        public ActionResult Approveacancelation(int cid, Ordercancelation model)
        {


            Ordercancelation c = db.Ordercancelations.FirstOrDefault(x => x.Id == cid);
            model.Id = c.Id;
            model.OrderId = c.OrderId;
            model.Date = c.Date;
            model.Reason = c.Reason;

            ViewBag.c = c.Order.Total - c.Order.Total * 0.1;

            return View(model);
        }


        [HttpPost]
        public ActionResult Approveacancelation(Ordercancelation model, HttpPostedFileBase file)
        {
            Ordercancelation c = db.Ordercancelations.Find(model.Id);
            c.Cancelationfile = file.FileName;
            c.Status = "Cancellation approved";
            c.Statusnum = 2;
            db.SaveChanges();



            int id = c.OrderId;





            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "OrderRefundslip");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "OrderRefundslip\\" + id.ToString());


            if (!Directory.Exists(pathString1))
            {
                Directory.CreateDirectory(pathString1);
            }

            if (!Directory.Exists(pathString2))
            {
                Directory.CreateDirectory(pathString2);
            }


            //check if a file was  uploaded
            if (file != null && file.ContentLength > 0)
            {
                //get file extension
                string ext = file.ContentType.ToLower();
                //verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {

                        ModelState.AddModelError("", "Wrong Image Extension !");
                        return View(model);


                    }
                }
                //initilize image name
                string imageName = file.FileName;

                var path = string.Format("{0}\\{1}", pathString2, imageName);

                //save original image
                file.SaveAs(path);



                Order dbk = db.Orders.Find(model.OrderId);
                dbk.Status = "Cancelation approved & account refunded";
                dbk.isCancelled = true;
                dbk.Cancelationfile = file.FileName;
                db.SaveChanges();
            }

            return RedirectToAction("Ordercancelationrequests");

        }
    }
}