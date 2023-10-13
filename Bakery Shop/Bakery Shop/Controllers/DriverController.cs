using Bakery_Shop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Bakery_Shop.Controllers
{
    public class DriverController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public DriverController()
        {

        }

        public DriverController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Driver
        public ActionResult Index()
        {
            return View(context.Drivers.ToList());
        }

       

        public ActionResult NewDriver()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> NewDriver(Driver driver)
        {

            if (ModelState.IsValid)
            {
                var Password = "Driver@21";
                var user = new ApplicationUser { UserName = driver.Email, Email = driver.Email };
                var result = await UserManager.CreateAsync(user, Password);

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (result.Succeeded)
                {
                    Driver _driver = new Driver();
                    _driver.Email = user.Email;
                    _driver.UserId = user.Id;
                    _driver.Name = driver.Name;
                    _driver.Phone = driver.Phone;

                    context.Drivers.Add(_driver);
                    context.SaveChanges();
                    if (!roleManager.RoleExists("Driver"))
                    {
                        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                        {
                            Name = "Driver"
                        };
                        roleManager.Create(role);
                        UserManager.AddToRole(user.Id, "Driver");
                    }
                    else
                    {
                        UserManager.AddToRole(user.Id, "Driver");
                    }
                    return RedirectToAction("Success");
                }
                ModelState.AddModelError("", "User Already Exist.");
            }
            return View();
        }

        [Authorize(Roles ="Driver")]
        public ActionResult DriverDeliveries()
        {
            var UserId = User.Identity.GetUserId();
            var member = context.Drivers.Where(d=>d.UserId == UserId).FirstOrDefault();
            int? MemberId = member.DriverId;
            var Orders = context.Orders.Where(u => u.AssignedDriverId == MemberId && u.isDelivered == true).ToList();
            return View(Orders);
        }

        public ActionResult Success()
        {
            return View();
        }

        //Pick up order
        public ActionResult OrderPickup(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order order = context.Orders.Find(id);
            order.isArriving = true;
            order.Status = "Order Is On Route";
            context.SaveChanges();
            return View(order);
        }
        //Pick up Custom
        public ActionResult OrderPickupCustom(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            CustomOrder order = context.CustomOrders.Find(id);
            order.isArriving = true;
            order.Status = "Order Is On Route";
            context.SaveChanges();
            return View(order);
        }



        public ActionResult Startcustomdelivery(int ordid)
        {
            CustomOrder order = context.CustomOrders.Find(ordid);
            order.isArriving = true;
            order.Status = "Order Is On Route";
            context.SaveChanges();

            return View(order);
        }



        public ActionResult Startdelivery(int ordid)
        {
            Order order = context.Orders.Find(ordid);
            order.isArriving = true;
            order.Status = "Order Is On Route";
            context.SaveChanges();

            return View(order);
        }

        public ActionResult Confirmcustomdelivery(int ordid)
        {
            CustomOrder order = context.CustomOrders.Find(ordid);

            return View(order);
        }


        public ActionResult Confirmdelivery(int ordid)
        {
            Order order = context.Orders.Find(ordid);

            return View(order);
        }


        public ActionResult DeliverOrder(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order order = context.Orders.Find(id);
            if (order != null)
            {
                order.Status = "Delivered";
                order.isDelivered = true;
                context.SaveChanges();
            }
            return View("Delivered");
        }

        //Custom Deliver
        public ActionResult DeliverOrderCustom(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            CustomOrder order = context.CustomOrders.Find(id);
            if (order != null)
            {
                order.Status = "Delivered";
                order.isDelivered = true;
                context.SaveChanges();
            }
            return View("Delivered");
        }

        public ActionResult Delivered()
        {
            return View();
        }


        [Authorize(Roles ="Driver")]
        public ActionResult Deliveries()
        {
            var UserID = User.Identity.GetUserId();
            var Driver = context.Drivers.Where(d => d.UserId == UserID).FirstOrDefault();
            return View(context.Orders.Where(d => d.AssignedDriverId == Driver.DriverId).ToList());
        }

        public ActionResult CustomDeliveries()
        {
            var UserID = User.Identity.GetUserId();
            var Driver = context.Drivers.Where(d => d.UserId == UserID).FirstOrDefault();
            return View(context.CustomOrders.Where(d => d.AssignedDriver == Driver.DriverId).ToList());
        }

    }
}