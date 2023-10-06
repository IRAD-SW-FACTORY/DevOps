using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WebDeployApi.Controllers;

namespace WebDeployApi.Tests.Controllers
{
    [TestClass]
    public class DeployControllerTest
    {
        [TestMethod]
        public void GetById()
        {
            // Disponer
            DeployController controller = new DeployController();

            // Actuar
            var result = controller.Get(Guid.NewGuid().ToString());

            // Declarar
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Post()
        {
            // Disponer
            DeployController controller = new DeployController();

            // Actuar
            var result = controller.Post(new Models.Deployment
            {

            });

            // Declarar
            Assert.IsNotNull(result);
        }
    }
}
