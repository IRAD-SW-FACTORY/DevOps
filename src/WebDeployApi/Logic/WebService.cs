using Microsoft.Web.Administration;
using System.Diagnostics;
using System.Xml.Linq;

namespace WebDeployApi.Logic
{
    public class WebService
    {
        const string cmd = "C:\\Windows\\system32\\inetsrv\\appcmd.exe";
        public static void Start(string name)
        {
            IO.Run(cmd, $"start apppool /apppool.name:{name}", $"starting app pool {name}");
        }
        public static void Stop(string name)
        {
            IO.Run(cmd, $"stop apppool /apppool.name:{name}", $"stopping app pool {name}");
        }      
    }
}