using System;
using System.Collections.Generic;

namespace WebDeployApi.Models
{
    public class Deployment
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DeploymentKind kind { get; set; }
        public string name { get; set; }
        public string deploymentUrl { get; set; }
        public string deploymentPath { get; set; }
        public bool asyncDeploy { get; set; } = true;
        public DeploymentStatus deploymentStatus { get; set; } = DeploymentStatus.Pending;
        public DateTime created { get; set; } = DateTime.UtcNow;
        public DateTime? updated { get; set; }
        public List<DeploymentLog> log { get; set; } = new List<DeploymentLog>();
    }
    public enum DeploymentKind
    {
        Web = 0,
        WinService = 1
    }

    public enum DeploymentStatus
    {
        Pending = 0,
        Inprogress = 1,
        Success = 2,
        Error = 3
    }

    public class DeploymentLog
    {
        public DeploymentLog()
        {
        }
        public DeploymentLog(Exception ex)
        {
            this.ex = ex;
        }
        public DeploymentLog(string status)
        {
            this.status = status;
        }
        public DeploymentLog(string status, Exception ex)
        {
            this.ex = ex;
            this.status = status;
        }

        public string status { get; set; }
        public Exception ex { get; set; }
        public DateTime created { get; set; } = DateTime.UtcNow;
    }
}