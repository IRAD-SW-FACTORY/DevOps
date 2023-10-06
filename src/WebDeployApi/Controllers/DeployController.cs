using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Results;

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
            if (System.IO.File.Exists(deploymentPath))
            {
                var deployment = JsonConvert.DeserializeObject<Models.Deployment>(System.IO.File.ReadAllText(deploymentPath));
                return Ok(deployment);
            }
            return NotFound();
        }


        [HttpPost]
        public IHttpActionResult Post([FromBody] Models.Deployment deployment)
        {
            if (Get(deployment.id) as NotFoundResult == null)
                return Conflict();


            var taskDeploy = new Task(() =>
            {
                // Init status
                var deploymentPath = HostingEnvironment.MapPath($"~/App_Data/{deployment.id}.json");

                // In progress
                try
                {
                    // TODO add deploy logic
                    deployment.deploymentStatus = Models.DeploymentStatus.Success;
                }
                catch (Exception ex)
                {
                    deployment.log.Add(ex.ToString());
                    deployment.deploymentStatus = Models.DeploymentStatus.Error;
                }
                finally
                {
                    // End
                    deployment.updated = DateTime.UtcNow;
                    System.IO.File.WriteAllText(deploymentPath, JsonConvert.SerializeObject(deployment));
                }
            });

            // Deploy
            taskDeploy.Start();
            if (!deployment.asyncDeploy)
                taskDeploy.Wait();

            return Ok(deployment);
        }
    }
}
