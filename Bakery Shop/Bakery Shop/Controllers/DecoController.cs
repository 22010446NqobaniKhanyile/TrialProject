using Bakery_Shop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bakery_Shop.Controllers
{
    public class DecoController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Decko
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Decolist()
        {

            var d = db.Decos.ToList();
            return View(d);
        }


        [HttpGet]
        public ActionResult Adddeco()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Adddeco(Deco model, HttpPostedFileBase file)
        {

            Deco d = new Deco()
            {
                Theme_color = model.Theme_color,
                Packagedescription = model.Packagedescription,
                Package_name = model.Package_name,
                Package_price = model.Package_price,
                Picname = file.FileName,
                Statusnum = 1,
            };
            db.Decos.Add(d);
            db.SaveChanges();

            int id = d.Id;





            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Decos");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Decos\\" + id.ToString());


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
                //save mage name to DTO
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Deco dc = db.Decos.Find(id);
                    dc.Picname = imageName;
                    db.SaveChanges();
                }
                //set orignial and thumb image path
                var path = string.Format("{0}\\{1}", pathString2, imageName);

                //save original image
                file.SaveAs(path);


            }

            return Redirect("/Deco/Adddecopiece?did=" + id);

        }


        public ActionResult Archive(int did)
        {
            Deco d = db.Decos.Find(did);

            return View(d);
        }

        [HttpGet]
        public ActionResult Adddecopiece(int did, Decopiece model)
        {

            model.Decoid = did;

            return View(model);
        }

        [HttpPost]
        public ActionResult Adddecopiece(Decopiece model, HttpPostedFileBase file)
        {

            Decopiece dp = new Decopiece()
            {
                Decoid = model.Decoid,
                Pieceimage = file.FileName,
                Pieceprice = model.Pieceprice,
                Piecename = model.Piecename,

            };

            db.Decopieces.Add(dp);
            db.SaveChanges();

            int id = dp.Id;





            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Decopiece");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Decopiece\\" + id.ToString());


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
                //save mage name to DTO
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Decopiece dc = db.Decopieces.Find(id);
                    dc.Pieceimage = imageName;
                    db.SaveChanges();
                }
                //set orignial and thumb image path
                var path = string.Format("{0}\\{1}", pathString2, imageName);

                //save original image
                file.SaveAs(path);


            }

            return Redirect("/Deco/Adddecopiece?did=" + dp.Decoid);
        }



        public ActionResult Sidedecopieces(int did)
        {
            var dp = db.Decopieces.Where(x => x.Decoid == did).ToList();

            return View(dp);
        }


        public ActionResult Decobookings()
        {
            var dbs = db.Decobookings.Where(x => x.Statusnum > 1).ToList();
            return View(dbs);
        }



        [HttpGet]
        public ActionResult Processorder(int bid)
        {

            Decobooking bk = db.Decobookings.Find(bid);

            if (!db.Orderpieces.Any(x => x.Bookingid == bid))
            {
                var op = db.Decopieces.Where(x => x.Decoid == bk.Packageid).ToList();

                foreach (var item in op)
                {
                    Orderpiece odp = new Orderpiece()
                    {
                        Booked = bk.Numberofguest,
                        Bookingid = bk.Id,
                        Pieceid = item.Id,

                    };
                    db.Orderpieces.Add(odp);
                    db.SaveChanges();
                }
            }

            Deco d = db.Decos.Find(bk.Packageid);

            ViewBag.Pacimg = d.Picname;

            return View(bk);
        }


        public ActionResult Orderpieces(int bid)
        {

            Decobooking dbk = db.Decobookings.Find(bid);

            var od = db.Orderpieces.Where(x => x.Bookingid == bid).ToList();

            return View(od);
        }


        public ActionResult Orderprocessed(int oid)
        {
            Decobooking dbk = db.Decobookings.Find(oid);
            dbk.Status = "Order processed";
            dbk.Statusnum = 3;
            db.SaveChanges();

            return RedirectToAction("Decobookings");
        }

        [HttpGet]
        public ActionResult Approveacancelation(int bid, Cancelation model)
        {

            Decobooking dbk = db.Decobookings.Find(bid);

            Cancelation c = db.Cancelations.FirstOrDefault(x => x.Bookingid == bid);
            model.Id = c.Id;
            model.Bookingid = c.Bookingid;
            model.Date = c.Date;

            ViewBag.c = c.Decobooking.Bookingfee - c.Decobooking.Bookingfee * 0.1;

            return View(model);
        }


        [HttpPost]
        public ActionResult Approveacancelation(Cancelation model, HttpPostedFileBase file)
        {
            Cancelation c = db.Cancelations.Find(model.Id);
            c.Cancelationfile = file.FileName;
            c.Status = "Cancellation approved";
            c.Statusnum = 2;
            db.SaveChanges();



            int id = c.Bookingid;





            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Refundslip");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Refundslip\\" + id.ToString());


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



                Decobooking dbk = db.Decobookings.Find(model.Bookingid);
                dbk.Status = "Cancelation approved & account refunded";
                dbk.Refundslip = file.FileName;
                dbk.Statusnum = 8;
                db.SaveChanges();
            }

            return RedirectToAction("Decobookings");

        }
    }
}