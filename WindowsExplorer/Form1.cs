using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

      
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {

        }

        private void BackButton_Click(object sender, EventArgs e)
        {

        }

        private void driveTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void pictureBoxPreview_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fdb = new FolderBrowserDialog() { Description = "Select your path." })
            {
                if (fdb.ShowDialog() == DialogResult.OK)
                {
                    webBrowser1.Url = new Uri(fdb.SelectedPath);
                    textBox1.Text = fdb.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoForward)
            {
                webBrowser1.GoForward();
            }
            textBox1.Text = webBrowser1.Url.ToString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoBack)
            {
                webBrowser1.GoBack();
            }
            textBox1.Text = webBrowser1.Url.ToString();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            textBox1.Text = webBrowser1.Url.ToString();
        }
    }
}
