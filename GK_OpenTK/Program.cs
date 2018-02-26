using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK_OpenTK
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string profileName = Environment.UserName;
            string exutable = GetExecutableName();
            if (!SOP.SOP.SOP_CheckProfile(profileName))
                Console.WriteLine("SOP_PROFILE_NAME_ERROR!");
            SOP.SOP.SOP_SetProfile(profileName, exutable);
            new GameWindow().Run(100);
        }
        private static string GetExecutableName()
        {
            string path = Process.GetCurrentProcess().MainModule.FileName;
            int lastPos = path.LastIndexOf('\\');

            return path.Substring(lastPos + 1, path.Length - lastPos - 1);
            ;
        }
    }
}
