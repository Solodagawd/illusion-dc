using illusion_dc.security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace illusion_dc.API
{
    internal class Request
    {
        public static void AttackServer(string host, int port, string game)
        {

            WebClient client = new WebClient();
            try
            {
                string response = client.DownloadString($"https://api.illusion.wtf/endpoints/crash/?key={Program.key}&hwid={hwid.GenerateHWID()}&host={host}&port={port}&game={game}&version={Program.version}&timecheck={security.timepin.GetPin()}");
                dynamic json = JsonConvert.DeserializeObject(response);
                if (json.status == false)
                {
                    MessageBox.Show($"{json.message}", "illusion");
                    return;
                }

                MessageBox.Show($"{json.message}", "illusion");


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send crash request: {ex}", "illusion");
            }
        }
    }

}
