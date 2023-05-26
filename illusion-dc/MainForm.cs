using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using illusion_dc.manual;
using System.Threading;
using illusion_dc.security;
using Newtonsoft.Json;
using System.Net;
using System.Xml.Linq;
using illusion_dc.pages;
using DiscordRPC.Logging;
using DiscordRPC;
using System.IO;
using Newtonsoft.Json.Linq;

namespace illusion_dc
{
    public partial class MainForm : Form
    {
        public static string proccessname = "";
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

        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
            UsersLabel.Text = Program.total_users;
            UsesLabel.Text = Program.global_uses;
            ExpireLabel.Text = Program.expire;
            PersonalUsesLabel.Text = Program.uses;
            GamesLabel.Text = GameSelectionBox.Items.Count.ToString();
            Thread thread = new Thread(() =>
            {

                while (true)
                {
                    WebClient client = new WebClient();
                    try
                    {
                        string response = client.DownloadString($"https://api.illusion.wtf/endpoints/login/?key={Program.key}&hwid={hwid.GenerateHWID()}&version={Program.version}");
                        dynamic json = JsonConvert.DeserializeObject(response);
                        if (json.status == false)
                        {
                            Application.Exit();
                        }
                        Program.uid = json.uid;
                        Program.expire = json.expire;
                        Program.uses = json.uses;
                        Program.global_uses = json.global_uses;
                        Program.total_users = json.total_users;
                        Program.rpcdetails = json.rpcdetails;
                        Program.discord = json.discord;
                        Form mainForm = Application.OpenForms[0];
                        mainForm.Invoke((MethodInvoker)delegate
                        {
                            UsersLabel.Text = Program.total_users;
                            UsesLabel.Text = Program.global_uses;
                            PersonalUsesLabel.Text = Program.uses;
                            CooldownProgress.Value = (int)json.cooldown_left;
                        });


                    }
                    catch
                    {
                        Environment.Exit(0);
                    }
                    Thread.Sleep(5000);
                }
            });

            thread.Start();

            string configFileName = "config.json";
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string configFilePath = Path.Combine(appDataPath, configFileName);

            if (!File.Exists(configFilePath))
            {
                CreateConfigFile(configFilePath);
            }
            else
            {
                string jsonContent = File.ReadAllText(configFilePath);
                JObject configObject = JObject.Parse(jsonContent);
                bool rpcValue = (bool)configObject["rpcenabled"];

                Program.rpcEnabled = rpcValue;

            }

        }
        private Point _mouseLoc;


        private DiscordRpcClient client;


        static void CreateConfigFile(string filePath)
        {
            string jsonContent = "{ \"rpcenabled\": true }";
                File.WriteAllText(filePath, jsonContent);
               
            

        }

        public static void EditRpcValueInConfig(bool newRpcValue)
        {
            string configFileName = "config.json";
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(appDataPath, configFileName);

                string jsonContent = File.ReadAllText(filePath);
                JObject configObject = JObject.Parse(jsonContent);
                configObject["rpcenabled"] = newRpcValue;
                File.WriteAllText(filePath, configObject.ToString());

        }
        public void InitializeDiscordRPC()
        {
            client = new DiscordRpcClient("1110002031227306064");
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            client.Initialize();
        }

        public void SetRichPresence()
        {

            Thread thread = new Thread(() =>
            {

                DateTime currentDate = DateTime.UtcNow;
                string state = "";
                string details = Program.rpcdetails;
                while (true)
                {
                    if (Program.rpcEnabled)
                    {

                        if (string.IsNullOrEmpty(GameName))
                        {
                            state = "Idling";
                        }
                        else
                        {
                            state = "Crashing " + GameName + " lobbies!";
                        }
                        var presence = new RichPresence()
                        {
                            Details = details,
                            State = state,

                            Assets = new Assets()
                            {
                                LargeImageKey = "banner",
                                LargeImageText = ""
                            },
                            Buttons = new DiscordRPC.Button[]
                            {
                                new DiscordRPC.Button() { Label = "Discord", Url = Program.discord },
                                new DiscordRPC.Button() { Label = "Website", Url = "https://illusion.wtf" }
                            }
                        };

                        client.SetPresence(presence);
                    }
                    else
                    {
                        client.ClearPresence();
                    }
                    Thread.Sleep(1500);
                }
            });

            thread.Start();
        }



            private void MainForm_Load(object sender, EventArgs e)
            {
                InitializeDiscordRPC();
                SetRichPresence();
                
            }

            private void ExitButton_Click(object sender, EventArgs e)
            {
                Environment.Exit(0);
            }

            private void MainForm_MouseDown(object sender, MouseEventArgs e)
            {
                _mouseLoc = e.Location;

            }

            private void MainForm_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    int dx = e.Location.X - _mouseLoc.X;
                    int dy = e.Location.Y - _mouseLoc.Y;
                    this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
                }
            }
            private void OpenManual()
            {
                Manual frm2 = new Manual();
                frm2.ShowDialog();
            }

            private void CrashButton_Click(object sender, EventArgs e)
            {

                switch (GameSelectionBox.Text)
                {
                    case "":
                        MessageBox.Show("Please select a game first", "illusion");
                        break;
                    case "Call Of Duty*":
                        proccessname = "cod";
                        OpenManual();
                        break;
                    case "Fortnite":
                        proccessname = "FortniteClient-Win64-Shipping";
                        OpenManual();
                        break;
                    case "Rainbow Six Siege":
                        proccessname = "r6";
                        OpenManual();
                        break;
                    case "VRChat":
                        proccessname = "vrchat";
                        OpenManual();
                        break;
                    case "Rocket League":
                        proccessname = "rocketleague";
                        OpenManual();
                        break;
                    case "Apex Legends":
                        proccessname = "r5apex";
                        OpenManual();
                        break;
                    case "Minecraft":
                        proccessname = "Minecraft";
                        OpenManual();
                        break;
                    case "Sea Of Thieves":
                        proccessname = "seaofthieves";
                        OpenManual();
                        break;
                    case "ChilloutVR":
                        proccessname = "chilloutvr";
                        OpenManual();
                        break;
                    case "Roblox":
                        proccessname = "roblox";
                        OpenManual();
                        break;
                    case "FiveM":
                        proccessname = "fivem";
                        OpenManual();
                        break;
                    case "Rust (Beta)":
                        proccessname = "rust";
                        OpenManual();
                        break;
                    default:
                        MessageBox.Show("Selected game does not exist, contact support", "illusion");
                        break;
                }


            }


        
        public static string GameName;
        private void GameSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameName = GameSelectionBox.Text;
            
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsPage frm2 = new SettingsPage();
            frm2.ShowDialog();
        }

        //never was implemented due to shutdown
        private void StrengthBar_Scroll(object sender, ScrollEventArgs e)
        {
            StrengthLB.Text = "Strength: " + StrengthBar.Value.ToString();
        }
    }
}
