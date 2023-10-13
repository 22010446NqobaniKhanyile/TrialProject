using System;
using System.Web.Mvc;
using Bakery_Shop.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bakery_Shop.Tests.Controllers
{
    [TestClass]
    public class Orders
    {
        [TestMethod]
        public void CheckCart()
        {
            ShoppingCartController shopping = new ShoppingCartController();
            //Act
            ViewResult result = shopping.Cart() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CheckOut()
        {
            ShoppingCartController shopping = new ShoppingCartController();
            //Act
            ViewResult result = shopping.Checkout() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
