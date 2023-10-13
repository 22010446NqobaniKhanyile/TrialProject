using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bakery_Shop.Controllers;
using System.Web.Mvc;

namespace Bakery_Shop.Tests.Controllers
{
    [TestClass]
    public class Bakery
    {
        [TestMethod]
        public void AddProduct() { 
            BakeryController bakery = new BakeryController();
            //Act
            ViewResult result = bakery.NewProduct() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Shop()
        {
            BakeryController bakery = new BakeryController();
            //Act
            ViewResult result = bakery.Shop() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Products()
        {
            BakeryController bakery = new BakeryController();
            //Act
            ViewResult result = bakery.Products() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
