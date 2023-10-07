using System.Diagnostics;

namespace WebDeployApi.Logic
{
    public class WinService
    {
        public static void Start(string name)
        {
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Windows\\system32\\sc.exe";
            p.StartInfo.Arguments = $"start {name}";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new System.Exception($"Error starting service {name}");
        }
        public static void Stop(string name)
        {
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Windows\\system32\\sc.exe";
            p.StartInfo.Arguments = $"stop {name}";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new System.Exception($"Error starting service {name}");
        }
    }
}