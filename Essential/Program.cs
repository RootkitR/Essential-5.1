using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Essential.Core;
using System.Net;
using System.IO;
using System.Diagnostics;
namespace Essential
{
    internal class Program
    {
        private delegate bool EventHandler(CtrlType sig);
        private enum CtrlType
        {
            CTRL_BREAK_EVENT = 1,
            CTRL_C_EVENT = 0,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool bool_0 = false;

        private static EventHandler delegate0_0;

        static ConsoleKeyInfo ConsoleKeyInfo;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(Program.EventHandler handler, bool add);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        public const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        int uFlags);

        private const int HWND_TOPMOST = -1;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;
        public static void Main(string[] args)
        {
            //LicenseFile lf = new LicenseFile(System.IO.File.ReadAllText("essential.license"));
            
            CustomCultureInfo.SetupCustomCultureInfo();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.smethod_0);

            Program.delegate0_0 = (Program.EventHandler)Delegate.Combine(Program.delegate0_0, new Program.EventHandler(Program.smethod_1));

            Program.SetConsoleCtrlHandler(Program.delegate0_0, true);
            
            try
            {
                Essential @class = new Essential();
                @class.Initialize();
                IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
                SetWindowPos(hWnd,
               new IntPtr(HWND_TOPMOST),
               0, 0, 0, 0,
               SWP_NOMOVE | SWP_NOSIZE);
                Program.bool_0 = true;
                DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
               /* Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\n~~~ IF YOU WANT CLOSE EMULATOR PLEASE PRESS ESCAPE (Esc) BUTTON ~~~\r\n");
                Console.ForegroundColor = ConsoleColor.Gray;*/
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            while (true)
            {
                ConsoleKeyInfo = Console.ReadKey();

                if (ConsoleKeyInfo.Key == ConsoleKey.Escape)
                    smethod_1(CtrlType.CTRL_CLOSE_EVENT);
            }
        }
        private static void smethod_0(object sender, UnhandledExceptionEventArgs e)
        {
            Logging.Disable();
            Exception ex = (Exception)e.ExceptionObject;
            Logging.LogCriticalException(ex.ToString());
        }
        private static bool smethod_1(CtrlType enum0_0)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), true), SC_CLOSE, MF_BYCOMMAND);
            if (Program.bool_0)
            {
                Logging.Disable();
                Console.Clear();
                Console.WriteLine("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!");
                Essential.Destroy("", true);
            }
            return true;
        }
    }
}