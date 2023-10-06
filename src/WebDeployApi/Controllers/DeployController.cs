﻿using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
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
            if (deployment == null)
                return BadRequest();
            if (Get(deployment.id) as NotFoundResult == null)
                return Conflict();


            var taskDeploy = new Task(() =>
            {
                // Init status
                var deploymentPath = HostingEnvironment.MapPath($"~/App_Data/{deployment.id}.json");

                // In progress
                try
                {
                    deployment.deploymentStatus = Models.DeploymentStatus.Inprogress;
                    // TODO Stop service
                    deployment.log.Add(new Models.DeploymentLog($"Stopping {deployment.name}"));
                    // TODO Copy files
                    System.IO.File.WriteAllText(deploymentPath, JsonConvert.SerializeObject(deployment));
                    Thread.Sleep(30000);
                    deployment.log.Add(new Models.DeploymentLog($"Copying files from  {deployment.deploymentUrl}"));
                    // TODO Backup files
                    deployment.log.Add(new Models.DeploymentLog($"Creating backup of path {deployment.deploymentPath}"));
                    // TODO Start service
                    deployment.log.Add(new Models.DeploymentLog($"Starting {deployment.name}"));
                    deployment.deploymentStatus = Models.DeploymentStatus.Success;
                }
                catch (Exception ex)
                {
                    deployment.log.Add(new Models.DeploymentLog(ex));
                    deployment.deploymentStatus = Models.DeploymentStatus.Error;
                }
                finally
                {
                    // End
                    deployment.updated = DateTime.UtcNow;
                    deployment.log.Add(new Models.DeploymentLog($"Deployment {deployment.name} finished"));
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