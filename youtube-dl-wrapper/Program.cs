using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtube_dl_wrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            string pythonExe = string.Empty;
            string youtubeDlExe = string.Empty;
            string progDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string[] youtubeDlScripts = new string[]
            {
                @"youtube-dl",
                @"youtube-dl.py",
            };

            foreach (string ytdl in youtubeDlScripts)
            {
                string possibleYtdl = Path.Combine(progDir, ytdl);
                if (File.Exists(possibleYtdl))
                {
                    youtubeDlExe = possibleYtdl;
                    break;
                }
            }

            if (!File.Exists(youtubeDlExe))
            {
                Console.WriteLine("WRAPPER: Could not find youtube-dl");
                Console.WriteLine("WRAPPER: Path's checked:");

                foreach (string ytdl in youtubeDlScripts)
                {
                    string possibleYtdl = Path.Combine(progDir, ytdl);
                    Console.WriteLine("WRAPPER: " + possibleYtdl);
                }
                System.Environment.Exit(1);
            }


            List<Dictionary<string, string>> pythonRegKeys = new List<Dictionary<string, string>>();
            pythonRegKeys.Add(
                new Dictionary<string, string>() {
                    { "Path", @"SOFTWARE\Python\PythonCore\3.7\InstallPath" },
                    { "Dir", "HKLM" },
                    { "Key", "ExecutablePath" },
                }
            );
            pythonRegKeys.Add(
                new Dictionary<string, string>() {
                    { "Path", @"SOFTWARE\Python\PythonCore\3.7\InstallPath" },
                    { "Dir", "HKCU" },
                    { "Key", "ExecutablePath" },
                }
            );
            pythonRegKeys.Add(
                new Dictionary<string, string>() {
                    { "Path", @"SOFTWARE\Python\PythonCore\3.7-32\InstallPath" },
                    { "Dir", "HKLM" },
                    { "Key", "ExecutablePath" },
                }
            );
            pythonRegKeys.Add(
                new Dictionary<string, string>() {
                    { "Path", @"SOFTWARE\Python\PythonCore\3.7-32\InstallPath" },
                    { "Dir", "HKCU" },
                    { "Key", "ExecutablePath" },
                }
            );
            pythonRegKeys.Add(
                new Dictionary<string, string>() {
                    { "Path", @"SOFTWARE\Python\PythonCore\3.6\InstallPath" },
                    { "Dir", "HKLM" },
                    { "Key", "ExecutablePath" },
                }
            );
            pythonRegKeys.Add(
                new Dictionary<string, string>() {
                    { "Path", @"SOFTWARE\Python\PythonCore\3.6\InstallPath" },
                    { "Dir", "HKCU" },
                    { "Key", "ExecutablePath" },
                }
            );
            pythonRegKeys.Add(
                 new Dictionary<string, string>() {
                    { "Path", @"SOFTWARE\Python\PythonCore\3.5\InstallPath" },
                    { "Dir", "HKLM" },
                    { "Key", "ExecutablePath" },
                 }
             );
            pythonRegKeys.Add(
                new Dictionary<string, string>() {
                    { "Path", @"SOFTWARE\Python\PythonCore\3.5\InstallPath" },
                    { "Dir", "HKCU" },
                    { "Key", "ExecutablePath" },
                }
            );

            string[] pythonPaths = new string[]
            {
                @"C:\Python37\python.exe",
                @"C:\Python37-32\python.exe",
                @"C:\Python36\python.exe",
                @"C:\Python35\python.exe",
                @"C:\Python3\python.exe",
            };

            foreach (Dictionary<string, string> key in pythonRegKeys)
            {
                if (key["Dir"] == "HKLM")
                {
                    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key["Path"]);

                    if (registryKey != null)
                    {
                        object value = registryKey.GetValue(key["Key"]);
                        if (value != null)
                        {
                            pythonExe = (string)value;
                            break;
                        }
                    }
                }
            }
            if (!File.Exists(pythonExe))
            {
                foreach (string path in pythonPaths)
                {
                    if (File.Exists(path))
                    {
                        pythonExe = path;
                        break;
                    }
                }
            }
            if (!File.Exists(pythonExe))
            {
                Console.WriteLine("WRAPPER: Could not determine Python3 Path");
                Console.WriteLine("WRAPPER: Path's checked:");
                foreach (string path in pythonPaths)
                {
                    Console.WriteLine("WRAPPER: " + path);
                }
                Console.WriteLine("WRAPPER: Registry Paths's checked:");
                foreach (Dictionary<string, string> key in pythonRegKeys)
                {
                    Console.WriteLine(String.Format("WRAPPER: {0}/{1} - {2}", key["Dir"], key["Path"], key["Key"]));
                }
                System.Environment.Exit(1);
            }
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = pythonExe;
            startInfo.Arguments = youtubeDlExe;
            if (args.Length >= 1)
            {
                startInfo.Arguments += " " + String.Join(" ", args);
            }
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            p.EnableRaisingEvents = true;
            p.StartInfo = startInfo;
            p.OutputDataReceived += new DataReceivedEventHandler
            (
                delegate (object sender, DataReceivedEventArgs e)
                {
                    Console.WriteLine(e.Data);
                }
            );
            p.ErrorDataReceived += new DataReceivedEventHandler
            (
                delegate (object sender, DataReceivedEventArgs e)
                {
                    Console.WriteLine(e.Data);
                }
            );
            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.CancelOutputRead();
            p.CancelErrorRead();
        }
    }
}
