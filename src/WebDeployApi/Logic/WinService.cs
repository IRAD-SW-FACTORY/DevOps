using System.Diagnostics;

namespace WebDeployApi.Logic
{
    public class WinService
    {
        const string cmd = "C:\\Windows\\system32\\sc.exe";
        public static void Start(string name)
        {
            IO.Run(cmd, $"start {name}", $"starting service {name}");
        }
        public static void Stop(string name)
        {
            IO.Run(cmd, $"stop {name}", $"stoping service {name}");
        }
    }
}