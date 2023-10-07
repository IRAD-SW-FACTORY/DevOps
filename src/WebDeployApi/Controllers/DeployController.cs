using Newtonsoft.Json;
using System;
using System.IO;
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
            if (deployment == null)
                return BadRequest();
            if (Get(deployment.id) as NotFoundResult == null)
                return Conflict();


            var taskDeploy = new Task(() =>
            {
                // Init status
                var deploymentPath = HostingEnvironment.MapPath($"~/App_Data/{deployment.id}.json");
                var tempDir = HostingEnvironment.MapPath($"~/App_Data/Temp/{deployment.id}");
                

                // In progress
                try
                {
                    deployment.deploymentStatus = Models.DeploymentStatus.Inprogress;
                    // Create temp directory, will be removed on success
                    if (!System.IO.Directory.Exists(tempDir))
                        System.IO.Directory.CreateDirectory(tempDir);

                    // Download and unzip release
                    var tempZip = HostingEnvironment.MapPath($"~/App_Data/Temp/{deployment.id}/{deployment.id}.zip");
                    if (System.IO.File.Exists(tempZip))
                        System.IO.File.Delete(tempZip);
                    new System.Net.WebClient().DownloadFile(deployment.deploymentUrl, tempZip);
                    Logic.Zip.Unzip(tempZip);
                    System.IO.File.Delete(tempZip);
                    deployment.log.Add(new Models.DeploymentLog($"Release downloaded and unzipped to {tempDir}"));

                    // TODO Stop service
                    deployment.log.Add(new Models.DeploymentLog($"Stopping {deployment.name}"));
                    switch (deployment.kind)
                    {
                        case Models.DeploymentKind.Web:
                            Logic.WebService.Stop(deployment.name);
                            break; ;
                        case Models.DeploymentKind.WinService:
                            Logic.WinService.Stop(deployment.name);
                            break;
                    }
                    deployment.log.Add(new Models.DeploymentLog($"{deployment.name} Stopped"));

                    var localPath = Path.Combine(deployment.deploymentLocalPath, deployment.name);
                    var backupPath = Path.Combine(deployment.deploymentLocalPath, $"{deployment.name}_{deployment.id}" );

                    
                    // Backup files
                    deployment.log.Add(new Models.DeploymentLog($"Creating backup of path {deployment.deploymentLocalPath}"));
                    var localDir = new DirectoryInfo(localPath);
                    Logic.IO.DeepCopy(localDir, backupPath);
                    Logic.IO.Empty(localDir);
                    deployment.log.Add(new Models.DeploymentLog($"Backup done"));
                    // Copy files
                    deployment.log.Add(new Models.DeploymentLog($"Copying files from  {deployment.deploymentUrl}"));
                    Logic.IO.DeepCopy(new DirectoryInfo(tempDir), localPath);
                    deployment.log.Add(new Models.DeploymentLog($"Files copying done"));
                    // Start service
                    deployment.log.Add(new Models.DeploymentLog($"Starting {deployment.name}"));
                    switch (deployment.kind)
                    {
                        case Models.DeploymentKind.Web:
                            Logic.WebService.Start(deployment.name);
                            break;
                        case Models.DeploymentKind.WinService:
                            Logic.WinService.Start(deployment.name);
                            break;
                    }
                    deployment.log.Add(new Models.DeploymentLog($"{deployment.name} Started"));
                    deployment.deploymentStatus = Models.DeploymentStatus.Success;

                    //Clean all
                    System.IO.Directory.Delete(tempDir, true);
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
