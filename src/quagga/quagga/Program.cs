using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.IO;
using System.Diagnostics;

namespace quagga
{
    class Program
    {
        // TODO:
        // Add logger.
        // Movie methods to classes.

        static void Main(string[] args)
        {
            KillWScriptProcesses();
            Console.ReadLine();
        }

        private static bool KillWScriptProcesses()
        {
            int count = 0;
            foreach(Process proc in Process.GetProcesses())
            {
                if(proc.ProcessName.Contains("wscript", StringComparison.OrdinalIgnoreCase))
                {
                    proc.Kill();
                    // TODO: Write to log.
                    Console.WriteLine("Process terminated: " + proc.ProcessName);
                    count++;
                }
            }
            if (count == 0)
            {
                Console.WriteLine("No wscript.exe process is running!");
            }
            return false;
        }

        private static bool CleanUpRegistry()
        {
            // Note: Debug needs to be configured to x64 bits in order to show some keys under
            // http://stackoverflow.com/questions/13324920/regedit-shows-keys-that-are-not-listed-using-getsubkeynames
            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (regKey != null)
                {
                    // Look for values containing wscript.
                    foreach (string valueName in regKey.GetValueNames())
                    {
                        // Delete anykey that contains wscript....
                        if (valueName.Contains("wscript"))
                            regKey.DeleteValue(valueName);

                        // If value-data contains wscript also delete the key.
                        var valueData = regKey.GetValue(valueName) as string;
                        if (valueData != null && valueData.Contains("wscript", StringComparison.OrdinalIgnoreCase))
                        {
                            regKey.DeleteValue(valueName);
                        }
                    }
                }
            }
            return true;
        }

        public static bool CleanUpTasksScheduler()
        {
            throw new NotImplementedException();
        }

        private static bool CleanUpFileSystem()
        {
            // All Users startup folder: C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup
            // Personal startup folder: C:\Users\<user name>\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup
            throw new NotImplementedException();
        }

        public static string GetLnkTarget(string lnkPath)
        {
            var shl = new Shell32.Shell(); // Move this to class scope
            lnkPath = Path.GetFullPath(lnkPath);
            var dir = shl.NameSpace(Path.GetDirectoryName(lnkPath));
            var itm = dir.Items().Item(Path.GetFileName(lnkPath));
            var lnk = (Shell32.ShellLinkObject)itm.GetLink;
            return lnk.Target.Path;
        }

        // TODO: Restore any EXTERNAL (USB) hidden files (only root/top level) and delete any link files in USB.
    }
}
