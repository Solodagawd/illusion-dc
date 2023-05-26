using illusion_dc.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace illusion_dc
{

    internal static class Program
    {
        public static string version = "1.4.1";
        public static string uid;
        public static string expire;
        public static string uses;
        public static string global_uses;
        public static string total_users;
        public static string key;
        public static bool rpcEnabled = true;
        public static string rpcdetails = "";
        public static string discord = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]


        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginPage());
        }
    }
}
