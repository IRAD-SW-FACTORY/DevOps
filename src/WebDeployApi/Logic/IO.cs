using System.Diagnostics;
using System.IO;

namespace WebDeployApi.Logic
{
    public static class IO
    {
        public static void DeepCopy(this DirectoryInfo directory, string destinationDir, string filePattern = "*.*")
        {
            foreach (string dir in Directory.GetDirectories(directory.FullName, filePattern, SearchOption.AllDirectories))
            {
                string dirToCreate = dir.Replace(directory.FullName, destinationDir);
                Directory.CreateDirectory(dirToCreate);
            }

            foreach (string newPath in Directory.GetFiles(directory.FullName, filePattern, SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(directory.FullName, destinationDir), true);
            }
        }
        public static void Empty(this DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        internal static void Run(string cmd, string args, string message)
        {
            Process p = new Process();
            p.StartInfo.FileName = cmd;
            p.StartInfo.Arguments = args;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode != 0 && p.ExitCode != 1062)
                throw new System.Exception($"Error {message}");
        }
    }
}