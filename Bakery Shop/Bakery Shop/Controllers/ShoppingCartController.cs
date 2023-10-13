using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bakery_Shop.Models;
using Microsoft.AspNet.Identity;

namespace Bakery_Shop.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        // GET: ShoppingCart
        [Authorize]
        public ActionResult Cart()
        {
            if (!User.IsInRole("Customer"))
            {
                return HttpNotFound();
            }
            List<Cart> ProductList = (List<Cart>)Session["cart"];
            List<CartView> Cart = new List<CartView>();

            if (ProductList != null)
            {
                foreach (var item in ProductList)
                {
                    CartView cartView = new CartView();
                    Product _Product = _db.products.Where(x => x.ProductId == item.pid).FirstOrDefault();
                    cartView.ProdId = _Product.ProductId;
                    cartView.ProdName = _Product.ProductName;
                    cartView.ProdImage = _Product.Image;
                    cartView.ProdPrice = Convert.ToDouble(_Product.ProductPrice);
                    cartView.ProdQty = item.qty;
                    cartView.Total = item.qty * Convert.ToDecimal(_Product.ProductPrice);
                    Cart.Add(cartView);
                }
                ViewBag.CartCount = Cart.Count;
                return View(Cart);
            }
            return View();

        }

        public ActionResult Checkout()
        {
            List<Cart> ProductList = (List<Cart>)Session["cart"];
            List<CartView> Cart = new List<CartView>();

            if (ProductList != null)
            {
                foreach (var item in ProductList)
                {
                    CartView cartView = new CartView();
                    Product _Product = _db.products.Where(x => x.ProductId == item.pid).FirstOrDefault();
                    cartView.ProdId = _Product.ProductId;
                    cartView.ProdName = _Product.ProductName;
                    cartView.ProdImage = _Product.Image;
                    cartView.ProdPrice = Convert.ToDouble(_Product.ProductPrice);
                    cartView.ProdQty = item.qty;
                    cartView.Total = item.qty * Convert.ToDecimal(_Product.ProductPrice);
                    Cart.Add(cartView);
                }
                ViewBag.CartCount = Cart.Count;
                return View(Cart);
            }
            return View("CheckOutError");
        }
        [HttpPost]
        public ActionResult ProceedCheckout(Order order,int Total)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    OrderDetails orderProduct = new OrderDetails();
                    Random OrderNoGenerator = new Random();
                    Random random = new Random();


                    var TrackingNo = "";

                    for (int i = 0; i < 10; i++)
                    {
                        var Tracking = random.Next(0, 9);
                        TrackingNo += Convert.ToString(Tracking);
                    }
                    var UserId = User.Identity.GetUserId();
                    order.TrackingNumber = TrackingNo;
                    order.CustomerId = UserId;
                    order.Total = Total;
                    context.Orders.Add(order);
                    context.SaveChanges();

                    List<Cart> ProductList = (List<Cart>)Session["cart"];
                    List<CartView> Cart = new List<CartView>();

                    if (ProductList != null)
                    {
                        foreach (var item in ProductList)
                        {
                            CartView cartView = new CartView();
                            Product _Product = _db.products.Where(x => x.ProductId == item.pid).FirstOrDefault();
                            cartView.ProdId = _Product.ProductId;
                            cartView.ProdName = _Product.ProductName;
                            cartView.ProdImage = _Product.Image;
                            cartView.ProdPrice = Convert.ToDouble(_Product.ProductPrice);
                            cartView.ProdQty = item.qty;
                            cartView.Total = item.qty * Convert.ToDecimal(_Product.ProductPrice);
                            Cart.Add(cartView);
                        }

                        foreach (var item in Cart)
                        {
                            orderProduct.OrderId = order.OrderId;
                            orderProduct.ProductId = item.ProdId;
                            orderProduct.ProductPrice = Convert.ToDouble(item.ProdPrice);
                            orderProduct.Quantity = item.ProdQty;
                            
                            _db.OrderDetails.Add(orderProduct);
                            _db.SaveChanges();
                        }
                        TempData["OrderNo"] = order.OrderId;
                    }
                }
            }
            return RedirectToAction("MakePayment");
        }

        public ActionResult MakePayment()
        {
            var UserId = User.Identity.GetUserId();
            var notPayed = _db.Orders.Where(x => x.CustomerId == UserId && x.isPayed == false).FirstOrDefault();
            ViewBag.OrderNo = TempData["OrderNo"];
            if (notPayed != null)
            {
                ViewBag.Total = notPayed.Total;
            }
            return View();
        }

        public ActionResult PaymentSuccess()
        {
            var UserId = User.Identity.GetUserId();
            var notPayed = _db.Orders.Where(x => x.CustomerId == UserId && x.isPayed == false).FirstOrDefault();

            if (notPayed != null)
            {
                notPayed.isPayed = true;
                _db.SaveChanges();
                List<Cart> ProductList = (List<Cart>)Session["cart"];
                List<CartView> Cart = new List<CartView>();

                if (ProductList != null || Cart != null || Session["cart"] != null)
                {
                    Cart = null;
                    Session["cart"] = null;
                    ProductList = null;
                }
            }

            return View();
        }



        public ActionResult CheckOutError()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddToCart(int id)
        {
            Product _Product = _db.products.Where(x => x.ProductId == id).FirstOrDefault();

            if (Session["cart"] != null)
            {
                List<Cart> ProductList = (List<Cart>)Session["cart"];

                int check = 0;

                foreach (var item in ProductList)
                {


                    if (item.pid == id)
                    {
                        item.qty = item.qty + 1;
                        check = 0;
                        break;
                    }
                    else
                    {
                        check = 1;
                    }
                }
                if (check == 1)
                {
                    Cart obj = new Cart();
                    obj.pid = id;
                    obj.qty = 1;
                    ProductList.Add(obj);
                }

                Session["cart"] = (List<Cart>)ProductList;
            }
            else
            {
                List<Cart> Products = new List<Cart>();
                Cart obj = new Cart
                {
                    pid = id,
                    qty = 1
                };
                Products.Add(obj);
                Session["cart"] = Products;


            }
            List<Cart> Cart = (List<Cart>)Session["cart"];
            var Result = new
            {
                UserCart = Session["cart"],
                _ProductName = _Product.ProductName,
                CartCount = Convert.ToInt32(Cart.Count)
            };

            

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        //Increase Quantity
        public ActionResult IncrsQty(int id)
        {
            int Quantity = 0;
            List<Cart> ProductList = (List<Cart>)Session["cart"];
            for (int i = 0; i < ProductList.Count; i++)
            {
                if (ProductList[i].pid == id)
                {
                    ProductList[i].qty = ProductList[i].qty + 1;
                    Quantity = ProductList[i].qty;
                    break;
                }
            }
            Session["cart"] = (List<Cart>)ProductList;
            return Json(Quantity, JsonRequestBehavior.AllowGet);
        }

        //Decrease Quantity
        public ActionResult DecrsQty(int id)
        {
            int Quantity = 0;
            List<Cart> ProductList = (List<Cart>)Session["cart"];
            for (int i = 0; i < ProductList.Count; i++)
            {
                if (ProductList[i].pid == id)
                {
                    if (ProductList[i].qty > 1)
                    {
                        ProductList[i].qty = ProductList[i].qty - 1;
                        Quantity = ProductList[i].qty;
                    }
                    else
                    {
                        Quantity = 1;
                    }
                    break;
                }
            }
            Session["cart"] = (List<Cart>)ProductList;
            return Json(Quantity, JsonRequestBehavior.AllowGet);
        }

        //Remove Item From the Cart
        public ActionResult removeItem(int id)
        {
            List<Cart> ProductList = (List<Cart>)Session["cart"];

            if (ProductList.Count > 1)
            {
                for (int i = 0; i < ProductList.Count; i++)
                {
                    if (ProductList[i].pid == id)
                    {
                        ProductList.Remove(ProductList[i]);
                    }
                }
                Session["cart"] = (List<Cart>)ProductList;
            }
            else
            {
                ProductList.Clear();
                Session["cart"] = null;
            }

            return Json(Session["cart"] ?? "Empty");
        }
    }
}