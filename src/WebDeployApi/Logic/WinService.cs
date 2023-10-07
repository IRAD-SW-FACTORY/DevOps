using System.Diagnostics;

namespace WebDeployApi.Logic
{
    public class WinService
    {
        public static void Start(string name)
        {
            Run("C:\\Windows\\system32\\sc.exe", $"start {name}", $"starting service {name}");
        }
        public static void Stop(string name)
        {
            Run("C:\\Windows\\system32\\sc.exe", $"stop {name}", $"stoping service {name}");
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