using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace wndHandlerCapture
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        static void Main(string[] args)
        {
            Console.WriteLine("Press enter, then switch focus to the window you want to get info from.");
            Console.WriteLine("Then wait for 2 seconds, and check back here! :)");
            Console.ReadLine();
            Console.Clear();

            Thread.Sleep(2000);                                                             // Wait for 2 seconds

            IntPtr hWnd;                                                                    // A place to store the window-handle
            hWnd = GetForegroundWindow();                                                   // This is why you should switch to the right window
            StringBuilder className = new StringBuilder(GetWindowTextLength(hWnd) + 1);     // Creates the storage for classname based on actual length
            StringBuilder wndName = new StringBuilder(GetWindowTextLength(hWnd) + 1);       // Creates the storage for windowname based on actual length
            GetClassName(hWnd, className, className.Capacity);                              // Fetches classname and saves it into className
            GetWindowText(hWnd, wndName, className.Capacity);                               // Fetches windowname and saves it into wndName

            var cP = Process.GetCurrentProcess();                                           // Gets our current process
            var proc = Process.GetProcessesByName(cP.ProcessName);                          // Filters out the current processes from stufz
            var p = proc.FirstOrDefault(pr => pr != cP);                                    // Make sure that the process isnt active already

            if (p != null)
            {
                SetForegroundWindow(p.MainWindowHandle);                                    // Bring our window to the front
            }

            Console.WriteLine("Class-name: {0}", className.ToString());                     // Print the classname
            Console.WriteLine("Window-name: {0}", wndName.ToString());                      // Print the windowname
            Console.ReadLine();
        }
    }
}
