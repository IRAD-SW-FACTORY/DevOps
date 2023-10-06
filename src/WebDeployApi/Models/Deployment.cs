using System;

namespace WebDeployApi.Models
{
    public class Deployment
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DeploymentKind kind { get; set; }
        public string name { get; set; }
        public string deploymentUrl { get; set; }
        public DeploymentStatus deploymentStatus { get; set; } = DeploymentStatus.Pending;
        public DateTime created { get; set; } = DateTime.UtcNow;
        public DateTime? updated { get; set; }

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
}