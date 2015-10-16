using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FuzzyLogicWebService;
using FuzzyLogicWebService.Controllers;

namespace FuzzyLogicWebService.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        //[TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(null, null);

            // Act
            ViewResult result = controller.Index(null) as ViewResult;

            // Assert
            Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }

        //[TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(null, null);

            // Act
            ViewResult result = controller.AboutFuzzyLogic() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
