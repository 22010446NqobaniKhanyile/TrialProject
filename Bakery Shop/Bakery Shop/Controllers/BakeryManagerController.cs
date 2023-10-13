using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bakery_Shop.Controllers
{
    public class BakeryManagerController : Controller
    {
        // GET: BakeryManager
        public ActionResult Index()
        {
            return View();
        }
    }
}