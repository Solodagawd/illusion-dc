using illusion_dc.manual;
using illusion_dc.security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace illusion_dc.pages
{
    public partial class LoginPage : Form
    {

        public LoginPage()
        {
            InitializeComponent();
            string path = "key.txt";
            if (File.Exists(path) && new FileInfo(path).Length > 0)
            {
                KeyBox.Text = File.ReadAllText(path);
                LoginButton.PerformClick();

            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private Point _mouseLoc;

        private void LoginPage_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseLoc = e.Location;
        }

        private void LoginPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.Location.X - _mouseLoc.X;
                int dy = e.Location.Y - _mouseLoc.Y;
                this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string path = "key.txt";
            if (KeyBox.Text == "")
            {
                MessageBox.Show("Please enter a key first", "illusion");
                return;

            }
            WebClient client = new WebClient();
            try
            {

                string response = client.DownloadString($"https://api.illusion.wtf/endpoints/login/?key={KeyBox.Text}&hwid={hwid.GenerateHWID()}&version={Program.version}");
                dynamic json = JsonConvert.DeserializeObject(response);
                if(json.status == false){
                    MessageBox.Show($"{json.message}", "illusion");
                    return;
                }
                
                if(!File.Exists(path) || new FileInfo(path).Length == 0)
                {
                    File.WriteAllText(path, $"{KeyBox.Text}");
                }
                
                
                Program.uid = json.uid;
                Program.expire = json.expire;
                Program.uses = json.uses;
                Program.global_uses = json.global_uses;
                Program.total_users = json.total_users;
                Program.key = KeyBox.Text;
                Program.rpcdetails = json.rpcdetails;
                Program.discord = json.discord;
                if($"{json.alert}".Length > 0)
                {
                    MessageBox.Show($"{json.alert}", "illusion");
                }
                this.Hide();
                MainForm frm2 = new MainForm();
                frm2.ShowDialog();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send authentication request {ex}", "illusion");
                return;
            }
        }
    }
}
