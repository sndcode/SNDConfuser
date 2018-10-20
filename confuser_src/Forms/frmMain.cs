using dnlib.DotNet;
using SNDC.Helpers;
using SNDC.Protections;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Management;

namespace SNDC.Forms
{
    public partial class frmMain : Form
    {
        public static string setting;
        public static string input_path;
        public static string output_path;
        public static bool chaos = false;
        public static string version = "REV.A1";
        public static string userdb;
        public static string shwid;
        public static bool authenticated = false;

        List<IProtection> protections = new List<IProtection>();

        public frmMain()
        {
            InitializeComponent();
        }
        public static void hwid()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            shwid = cpuInfo;
        }

        public static void auth()
        {
            try
            { 
                WebClient wc = new WebClient();
                byte [] users = wc.DownloadData("http://web/auth/users.db");
                userdb = users.ToString();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            if (userdb.Contains(shwid))
            {
                authenticated = true;
            }
            else
            {
                MessageBox.Show("Either your hardware ID is not activated on our servers or the software encountered an critical error while calculating it.");
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + version;
            //auth();
        }
        private void runProtections()
        {
            foreach (IProtection protection in protections)
                protection.Protect();
        }
        private void loadProtections()
        {
            //protections.Add(new Proxy());
            protections.Add(new Constants());
            protections.Add(new Renamer());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void confuse(string input , string output , string setting)
        {
            Globals.asm = AssemblyDef.Load(input_path);

            if (setting == "Standard")
            {
                Generator.type = Generator.sType.Standard;
            }
            else if (setting == "Special")
            {
                Generator.type = Generator.sType.Special;
            }
            else if (setting == "Foreign")
            {
                Generator.type = Generator.sType.Foreign;
            }
            else if (setting == "Normal")
            {
                Generator.type = Generator.sType.Normal;
            }
            else if (setting == "Unreadable")
            {
                Generator.type = Generator.sType.Unreadable;
            }
            else if (setting == "beta")
            {
                Generator.type = Generator.sType.beta;
            }

            loadProtections();
            
            int countset = Convert.ToInt32(comboBox2.Text.Replace("x", ""));
            while(countset > 0)
            {
                runProtections();
                countset = countset - 1;
            }
            
            Globals.asm.Write(output_path);
            MessageBox.Show("File built at : " + output_path);
        }

        //void Form_DragDrop(object sender, DragEventArgs e)
        //{
        //    string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
        //    textBox1.Text = FileList.ToString() ;
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            textBox1.Text = ofd.FileName;
            input_path = ofd.FileName;
            textBox2.Text = ofd.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            textBox2.Text = sfd.FileName;
            output_path = sfd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "Standard")
            {
                setting = comboBox1.Text;
            }
            else if(comboBox1.Text == "Special")
            {
                setting = comboBox1.Text;
            }
            else if (comboBox1.Text == "Foreign")
            {
                setting = comboBox1.Text;
            }
            else if (comboBox1.Text == "Normal")
            {
                setting = comboBox1.Text;
            }
            else if (comboBox1.Text == "Unreadable")
            {
                setting = comboBox1.Text;
            }

            input_path = textBox1.Text;
            output_path = textBox2.Text;
            setting = comboBox1.Text;

            try
            {
                confuse(input_path, output_path , setting);
            }
            catch(Exception exception)
            {
                MessageBox.Show("Could not confuse file" + "\n" + exception);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("--== SND Confuser ==--" +
                "\n " +
                "\nCredits : " +
                "\n " +
                "\nSplamy " +
                "\nUC Forum " +
                "\n " + "\n " +
                "\nPRIVATE VERSION - REV.A1");
        }
    }
}
