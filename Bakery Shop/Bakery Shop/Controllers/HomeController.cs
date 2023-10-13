using Bakery_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bakery_Shop.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Decos()
        {

            var d = db.Decos.ToList();

            return View(d);
        }

        [Authorize]
        public ActionResult Decodetails(int did)
        {

            Deco d = db.Decos.Find(did);

            return View(d);
        }

        public ActionResult Decopieces(int did)
        {
            var dp = db.Decopieces.Where(x => x.Decoid == did).ToList();
            return View(dp);
        }


        [Authorize]
        public ActionResult Bookdeco(int did, DateTime date, int numofdays)
        {

            Decobooking dbk = new Decobooking()
            {
                Date = DateTime.UtcNow,
                Returndate = date.AddDays(numofdays),
                Bookingdate = date,
                Packageid = did,
                Useremail = User.Identity.Name
            };

            db.Decobookings.Add(dbk);
            db.SaveChanges();

            return Redirect("/home/Addbookingdetails?dbkid=" + dbk.Id);
        }

        [HttpGet, Authorize]
        public ActionResult Addbookingdetails(int dbkid)
        {

            Decobooking dbk = db.Decobookings.Find(dbkid);

            return View(dbk);
        }

        [HttpPost, Authorize]
        public ActionResult Addbookingdetails(Decobooking model)
        {
            Decobooking dbk = db.Decobookings.Find(model.Id);
            dbk.Numberofguest = model.Numberofguest;
            dbk.Isdeliveryrequested = model.Isdeliveryrequested;
            dbk.Address = model.Address;
            dbk.Statusnum = 1;

            TimeSpan days = dbk.Returndate - dbk.Bookingdate;

            double price = dbk.Numberofguest * 10 + dbk.Deco.Package_price * days.Days;

            dbk.Bookingfee = int.Parse(price.ToString());

            db.SaveChanges();

            TempData["Success"] = "Booking details updated";

            return Redirect("/home/startpayment?dbkid=" + model.Id);
        }


        public ActionResult startpayment(int dbkid)
        {

            Decobooking dbk = db.Decobookings.Find(dbkid);

            return View(dbk);
        }

        public ActionResult Payed(int dbkid)
        {

            Decobooking dbk = db.Decobookings.Find(dbkid);

            dbk.Status = "Payment recived";
            dbk.Statusnum = 2;
            db.SaveChanges();

            TempData["Success"] = "Payment success";

            return Redirect("/home/mydecobookings");
        }

        [Authorize]
        public ActionResult Mydecobookings()
        {

            var dbs = db.Decobookings.Where(x => x.Useremail == User.Identity.Name).ToList();

            return View(dbs);
        }

        [HttpGet]
        public ActionResult Cancelbooking(int boid, Cancelation model)
        {
            model.Bookingid = boid;

            Decobooking dbk = db.Decobookings.Find(boid);
            ViewBag.C = dbk.Bookingfee * 0.1;


            return View(model);
        }

        [HttpPost]
        public ActionResult Cancelbooking(Cancelation model)
        {
            Cancelation c = new Cancelation()
            {
                Bookingid = model.Bookingid,
                Date = DateTime.Now,
                Reason = model.Reason,
                Status = "Cancellation loged",
                Statusnum = 1,
            };
            db.Cancelations.Add(c);
            db.SaveChanges();


            Decobooking dbk = db.Decobookings.Find(model.Bookingid);
            dbk.Status = "Cancellation loged";
            dbk.Statusnum = 7;
            db.SaveChanges();

            return RedirectToAction("Mydecobookings");
        }

    }
}