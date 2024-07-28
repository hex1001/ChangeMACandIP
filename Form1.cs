using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ChangeMACandIP
{
    public partial class Form1 : Form
    {
        string[] str;
        bool almashdi1, almashdi2;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            str = Form1.ReadConfigFile("config.ini");
            Thread thread1 = new Thread(CheckStatus);
            thread1.Start();
            thread1.IsBackground = true;
            Hide();
            notifyIcon1.Visible = true;
        }

        public static bool IsNetworkCardAvailable(string networkCardName) {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in adapters){
                if (adapter.Name == networkCardName) {
                    return adapter.OperationalStatus == OperationalStatus.Up; 
                }
            }
            return false;
        }

        private void CheckStatus() {
            while (true) {
                if (!IsNetworkCardAvailable(str[4]) && IsNetworkCardAvailable(str[5]) && !almashdi1) {
                    //MessageBox.Show(str[4] + " bilan aloqa uzildi.");
                    almashdi1 = true;
                    almashdi2 = false;
                    ChangeAll(2);
                    //Thread.Sleep(10000);
                } 

                if (!IsNetworkCardAvailable(str[5]) && IsNetworkCardAvailable(str[4]) && !almashdi2) {
                    //MessageBox.Show(str[5] + " bilan aloqa uzildi.");
                    almashdi2 = true;
                    almashdi1 = false;
                    ChangeAll(1);
                    //Thread.Sleep(10000);
                } 
                Thread.Sleep(10000);
            }
        }
        private void ChangeAll(int num)
        {
            Process.Start("cmd.exe", $"/c macchng{num}.cmd");
            //RunCommand($"/c macchng{num}.cmd");
            //switch (num)
            //{
            //    case 1:
            //        RunCommand($"macshift -i \"{str[5]}\" {str[2]} && macshift -i \"{str[4]}\" {str[3]}");
            //        Thread.Sleep(10000);
            //        RunCommand($"netsh interface ipv4 set address name=\"{str[5]}\" static {str[0]} 255.255.255.0");
            //        Thread.Sleep(500);
            //        RunCommand($"netsh interface ipv4 set address name=\"{str[4]}\" static {str[1]} 255.255.255.0");
            //        break;
            //    case 2:
            //        RunCommand($"macshift -i \"{str[4]}\" {str[2]} && macshift -i \"{str[5]}\" {str[3]}");
            //        Thread.Sleep(10000);
            //        RunCommand($"netsh interface ipv4 set address name=\"{str[4]}\" static {str[0]} 255.255.255.0");
            //        Thread.Sleep(500);
            //        RunCommand($"netsh interface ipv4 set address name=\"{str[5]}\" static {str[1]} 255.255.255.0");
            //        break;
            //    default:
            //        break;
            //}
            

        }
        public static string[] ReadConfigFile(string filename)
        {
            string[] res = { "192.168.1.12", "192.168.1.112", "AA-11-BB-22-CC-33", "EE-44-CC-55-DD-66", "Ethernet 1", "Ethernet 4" };
            try
            {
                var input = new StreamReader(Directory.GetCurrentDirectory() + "\\" + filename);

                string[] IP1 = input.ReadLine().Split('=');
                string[] IP2 = input.ReadLine().Split('=');
                string[] MAC1 = input.ReadLine().Split('=');
                string[] MAC2 = input.ReadLine().Split('=');
                string[] NAME1 = input.ReadLine().Split('=');
                string[] NAME2 = input.ReadLine().Split('=');

                input.Close();
                if (!String.IsNullOrEmpty(IP1[1]))
                {
                    res[0] = IP1[1].Trim();
                }
                if (!String.IsNullOrEmpty(IP2[1]))
                {
                    res[1] = IP2[1].Trim();
                }
                if (!String.IsNullOrEmpty(MAC1[1]))
                {
                    res[2] = MAC1[1].Trim().Replace("-", "");
                }
                if (!String.IsNullOrEmpty(MAC2[1]))
                {
                    res[3] = MAC2[1].Trim().Replace("-", "");
                }
                if (!String.IsNullOrEmpty(NAME1[1]))
                {
                    res[4] = NAME1[1].Trim();
                }
                if (!String.IsNullOrEmpty(NAME2[1]))
                {
                    res[5] = NAME2[1].Trim();
                }

            }
            catch
            {
                MessageBox.Show("'config.ini' файл топилмади.");
            }
            return res;
        }

        
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!IsNetworkCardAvailable(str[4]) && IsNetworkCardAvailable(str[5]) && !almashdi1)
            {
                //MessageBox.Show(str[4] + " bilan aloqa uzildi.");
                almashdi1 = true;
                almashdi2 = false;
                ChangeAll(2);
                //Thread.Sleep(10000);
            }

            if (!IsNetworkCardAvailable(str[5]) && IsNetworkCardAvailable(str[4]) && !almashdi2)
            {
                //MessageBox.Show(str[5] + " bilan aloqa uzildi.");
                almashdi2 = true;
                almashdi1 = false;
                ChangeAll(1);
                //Thread.Sleep(10000);
            }
        }

        public void RunCommand(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                process.StandardInput.WriteLine(command);
                process.StandardInput.Flush();
                process.WaitForExit();
            }
            MessageBox.Show("It's OK!");
        }
    }
}
