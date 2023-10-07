using Microsoft.Web.Administration;
using System.Diagnostics;

namespace WebDeployApi.Logic
{
    public class WebService
    {
        public static void Start(string name)
        {
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Windows\\system32\\inetsrv\\appcmd.exe";
            p.StartInfo.Arguments = $"start apppool /apppool.name:{name}";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new System.Exception($"Error starting app pool {name}");
        }
        public static void Stop(string name)
        {
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Windows\\system32\\inetsrv\\appcmd.exe";
            p.StartInfo.Arguments = $"stop apppool /apppool.name:{name}";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new System.Exception($"Error stopping app pool {name}");
        }
    }
}