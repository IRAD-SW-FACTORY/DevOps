using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;

namespace WebDeployApi.Controllers
{
    public class DeployController : ApiController
    {
        [HttpGet]
        public string[] Get()
        {
            var deploymentPath = HostingEnvironment.MapPath($"~/App_Data/");
            return System.IO.Directory.GetFiles(deploymentPath, "*.json").Select(path => System.IO.Path.GetFileNameWithoutExtension(path)).ToArray();
        }

        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            var deploymentPath = HostingEnvironment.MapPath($"~/App_Data/{id}.json");
            if(System.IO.File.Exists(deploymentPath))
            {
                var deployment = JsonConvert.DeserializeObject<Models.Deployment>(System.IO.File.ReadAllText(deploymentPath));
                return Ok(deployment);
            }
            return NotFound();
        }


        [HttpPost]
        public IHttpActionResult Post([FromBody] Models.Deployment deployment)
        {
            if(Get(deployment.id) != null)
            {
                return Conflict();
            }

            var deploymentPath = HostingEnvironment.MapPath($"~/App_Data/{deployment.id}.json");
            System.IO.File.WriteAllText(deploymentPath, JsonConvert.SerializeObject(deployment));
            return Ok(deployment);
        }
    }
}
