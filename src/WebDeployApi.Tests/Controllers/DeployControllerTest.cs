using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WebDeployApi.Controllers;

namespace WebDeployApi.Tests.Controllers
{
    [TestClass]
    public class DeployControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Disponer
            DeployController controller = new DeployController();

            // Actuar
            IEnumerable<string> result = controller.Get();

            // Declarar
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Disponer
            DeployController controller = new DeployController();

            // Actuar
            string result = controller.Get(5);

            // Declarar
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Disponer
            DeployController controller = new DeployController();

            // Actuar
            controller.Post("value");

            // Declarar
        }

        [TestMethod]
        public void Put()
        {
            // Disponer
            DeployController controller = new DeployController();

            // Actuar
            controller.Put(5, "value");

            // Declarar
        }

        [TestMethod]
        public void Delete()
        {
            // Disponer
            DeployController controller = new DeployController();

            // Actuar
            controller.Delete(5);

            // Declarar
        }
    }
}
