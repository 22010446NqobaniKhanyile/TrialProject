using Bakery_Shop.Models;
using Microsoft.AspNet.Identity;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Bakery_Shop.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        readonly ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(context.Orders.ToList());
        }

        //Client Custom Order(Admin)
        public ActionResult ClientCustomOrders()
        {
            return View(context.CustomOrders.ToList());
        }

        //Confirm Ready
        public ActionResult ConfirmOrderReady(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomOrder _Order = context.CustomOrders.Find(id);
            _Order.IsReady = true;
            _Order.isPreparing = true;
            context.SaveChanges();

           

            return View();
        }

        //Assign Driver Custom
        public ActionResult AssignDriverCustom(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var Drivers = context.Drivers.Where(d => d.IsAvailable).ToList();
            ViewBag.Drivers = new SelectList(Drivers, "DriverId", "Name");

            //var Driver = context.Drivers.Where(x => x.IsAvailable == true).ToList();
            var DriverList = context.Drivers.ToList();
            var Order = context.CustomOrders.Find(id);
            ViewBag.OrderId = id;
            ViewBag.OrderId = Order.CustomOrderID;
            if (Order != null)
            {
                ViewBag.Orders = Order;
            }
            ViewBag.DriversList = DriverList;
            return View();
        }
        [HttpPost]
        public ActionResult AssignDriverCustom(int? OrderId, CustomOrder order)
        {
            var AssignedId = order.AssignedDriver;
            if (OrderId == null)
            {
                return HttpNotFound();
            }
            CustomOrder _order = context.CustomOrders.Find(OrderId);
            if (_order != null)
            {
                _order.AssignedDriver = AssignedId;
                _order.Deliverydate = order.Deliverydate;
                context.SaveChanges();
                return RedirectToAction("AssignDriverSuccess");
            }
            return View();
        }

        //Assign Driver Success
        public ActionResult AssignDriverSuccess()
        {
            return View();
        }

        //More Details Custom Order
        public ActionResult More(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomOrder _Order = context.CustomOrders.Find(id);
            if (_Order == null)
            {
                return HttpNotFound();
            }
            return View(_Order);
        }

        //Custom Orders
        public ActionResult ViewCustom()
        {
            var UserId = User.Identity.GetUserId();
            return View(context.CustomOrders.Where(t=>t.UserId == UserId).ToList());
        }

        //Place Custom Order
        public ActionResult CustomizeOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PlaceCustomOrder(CustomOrder customOrder, string LoggedEmail)
        {
            if (ModelState.IsValid)
            {
                double Total = 0;

                //Flavour
                switch (customOrder.Flavour)
                {
                    case "Lemon":
                        Total += 66.7;
                        break;
                    case "Strawberry":
                        Total += 88.99;
                        break;
                    case "Chocolate":
                        Total += 101.3;
                        break;
                    case "Orange":
                        Total += 89.7;
                        break;
                    default:
                        Total += 30;
                        break;
                }

                //Icing
                switch (customOrder.Icing)
                {
                    case "Buttercream":
                        Total += 87.7;
                        break;
                    case "Cream Cheese":
                        Total += 95.79;
                        break;
                    case "Vanilla":
                        Total += 79.3;
                        break;
                    default:
                        Total += 20;
                        break;
                }

                //Filling
                switch (customOrder.Filling)
                {
                    case "Chocolate":
                        Total += 130.8;
                        break;
                    case "Vanilla":
                        Total += 242.7;
                        break;
                    case "Cream Cheese":
                        Total += 92.8;
                        break;
                    default:
                        Total += 20;
                        break;
                }

                //Filling
                switch (customOrder.CakeType)
                {
                    case "Birthday":
                        Total += 220.5;
                        break;
                    case "Wedding":
                        Total += 358.7;
                        break;
                    case "Graduation":
                        Total += 350.8;
                        break;
                    case "Valentines":
                        Total += 170.8;
                        break;
                    default:
                        Total += 20;
                        break;
                }
                string fileName = Path.GetFileNameWithoutExtension(customOrder.ImageFile.FileName);
                string extension = Path.GetExtension(customOrder.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                customOrder.SketchImage = "../Content/Products/" + fileName;
                fileName = Path.Combine(Server.MapPath("../Content/Products/"), fileName);
                customOrder.ImageFile.SaveAs(fileName);

                Random random = new Random();

                var TrackingNo = "";

                for (int i = 0; i < 10; i++)
                {
                    var Tracking = random.Next(0, 9);
                    TrackingNo += Convert.ToString(Tracking);
                }

                if (LoggedEmail != null)
                {
                    customOrder.Email = LoggedEmail;
                }
                else
                {
                    customOrder.Email = User.Identity.Name;
                }

                customOrder.TrackingNumber = TrackingNo;
                customOrder.TotalAmount = Math.Round(Total, 2);
                var UserID = User.Identity.GetUserId();
                customOrder.UserId = UserID;
                context.CustomOrders.Add(customOrder);
                context.SaveChanges();
                return RedirectToAction("CompletePayment", new { id = customOrder.CustomOrderID});
            }
            return View(customOrder);
        }

        public ActionResult CompletePayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomOrder _Order = context.CustomOrders.Find(id);
            if (_Order == null)
            {
                return HttpNotFound();
            }
            TempData["OrderID"] = id;
            return View(_Order);
        }

        //Custom Order Success
        public ActionResult CustomOrderSuccess()
        {
            var OrderID = TempData["OrderID"];
            if (OrderID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomOrder _Order = context.CustomOrders.Find(OrderID);
            if (_Order == null)
            {
                return HttpNotFound();
            }
            _Order.IsPaymentCompleted = true;
            context.SaveChanges();
            return View();
        }

        //Order Details
        public ActionResult OrderDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order order = context.Orders.Find(id);
            return View(order);
        }

        //Available Orders
        public ActionResult AssignDriver(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var Drivers = context.Drivers.Where(d=>d.IsAvailable).ToList();
            ViewBag.Drivers = new SelectList(Drivers, "DriverId", "Name");

            //var Driver = context.Drivers.Where(x => x.IsAvailable == true).ToList();
            var DriverList = context.Drivers.ToList();
            var Order = context.Orders.Find(id);
            ViewBag.OrderId = id;
            ViewBag.OrderId = Order.OrderId;
            if (Order != null)
            {
                ViewBag.Orders = Order;
            }
            ViewBag.DriversList = DriverList;
            return View();
        }

        [HttpPost]
        public ActionResult AssignDriver(int? OrderId, Order order)
        {
            var AssignedId = order.AssignedDriverId;
            if (OrderId == null)
            {
                return HttpNotFound();
            }
            Order _order = context.Orders.Find(OrderId);
            if (_order != null)
            {
                _order.AssignedDriverId = AssignedId;
                _order.Deliverydate = order.Deliverydate;
                context.SaveChanges();
                return RedirectToAction("AssignSuccess");
            }
            return View();
        }

        //Custom Driver Deliveries
        public ActionResult CustomDeliveries()
        {
            return View();
        }

        //Assign Success
        public ActionResult AssignSuccess()
        {
            return View();
        }

        //Track Order
        public ActionResult TrackOrder()
        {
            ViewBag.Error = "";
            return View();
        }

        //Order TRacking Progress
        public ActionResult OrderTrackingProgress(string TrackingNumber)
        {
            var Order = context.Orders.Where(x => x.TrackingNumber == TrackingNumber).FirstOrDefault();
            var Custom = context.CustomOrders.Where(x => x.TrackingNumber == TrackingNumber).FirstOrDefault();
            bool IsCustomized = false;

            if (Order != null && Custom == null)
            {
                ViewBag.OrderNo = Order.OrderId;
                ViewBag.Phone = Order.CustomerPhone;
                ViewBag.TrackingNo = Order.TrackingNumber;
                ViewBag.TotalAmount = Order.Total;
                ViewBag.OrderDetails = Order.OrderDetails.ToList().Take(1);
                ViewBag.Processing = Order.isPreparing;
                ViewBag.Arriving = Order.isArriving;
                ViewBag.Received = Order.isReceived;
                ViewBag.Delivered = Order.isDelivered;
                ViewBag.OrderDetailsAll = Order.OrderDetails.ToList();
                //Sessions 
                TempData["OrderNo"] = Order.OrderId;
                TempData["TrackingNo"] = Order.TrackingNumber;
                TempData["OrderDetails"] = Order.OrderDetails.ToList();
                TempData["TotalAmount"] = Order.Total;
                IsCustomized = false;
            }

            if (Order == null && Custom != null)
            {
                ViewBag.OrderNo = Custom.CustomOrderID;
                ViewBag.Phone = Custom.Phone;
                ViewBag.TrackingNo = Custom.TrackingNumber;
                ViewBag.TotalAmount = Custom.TotalAmount;
                ViewBag.Processing = Custom.isPreparing;
                ViewBag.Arriving = Custom.isArriving;
                ViewBag.Received = Custom.isReceived;
                ViewBag.Delivered = Custom.isDelivered;
                //Sessions 
                TempData["OrderNo"] = Custom.CustomOrderID;
                TempData["TrackingNo"] = Custom.TrackingNumber;
                TempData["TotalAmount"] = Custom.TotalAmount;
                IsCustomized = true;
            }

            ViewBag.IsCostomized = IsCustomized;

            if (Order == null && Custom == null)
            {
                ViewBag.Error = "Invalid Tracking Number.";
                return RedirectToAction("TrackOrder");
            }
            
            return View();
        }
        public ActionResult PrepareOrder(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order order = context.Orders.Find(id);
            if (order != null)
            {
                order.Status = "Order Prepared";
                order.isPreparing = true;
                context.SaveChanges();
            }
            return View(order);
        }

        
    }
}