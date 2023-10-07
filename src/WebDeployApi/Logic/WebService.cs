using Microsoft.Web.Administration;
using System.Diagnostics;
using System.Xml.Linq;

namespace WebDeployApi.Logic
{
    public class WebService
    {
        public static void Start(string name)
        {
            Run("C:\\Windows\\system32\\inetsrv\\appcmd.exe", $"start apppool /apppool.name:{name}", $"starting app pool {name}");
        }
        public static void Stop(string name)
        {
            Run("C:\\Windows\\system32\\inetsrv\\appcmd.exe", $"stop apppool /apppool.name:{name}", $"stopping app pool {name}");
        }

        static void Run(string cmd, string args, string message)
        {
            Process p = new Process();
            p.StartInfo.FileName = cmd;
            p.StartInfo.Arguments = args;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new System.Exception($"Error {message}");
        }
    }
}