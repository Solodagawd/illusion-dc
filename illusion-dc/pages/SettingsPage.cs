using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace illusion_dc.pages
{
    public partial class SettingsPage : Form
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
        // this page was never released to the public :(
        public SettingsPage()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
            rpcToggle.Checked = Program.rpcEnabled;
        }
        Point _mouseLoc = new Point();
        private void SettingsPage_Load(object sender, EventArgs e)
        {

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void SettingsPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.Location.X - _mouseLoc.X;
                int dy = e.Location.Y - _mouseLoc.Y;
                this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
            }
        }

        private void SettingsPage_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseLoc = e.Location;
        }


        private void rpcToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (rpcToggle.Checked)
            {
                Program.rpcEnabled = true;
                MainForm.EditRpcValueInConfig(Program.rpcEnabled);
            }
            else
            {
                Program.rpcEnabled = false;
                MainForm.EditRpcValueInConfig(Program.rpcEnabled);
            }

        }
    }
}
