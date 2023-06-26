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
        private void PopulateTreeView(TreeNode node)
        {
            // Clear existing nodes
            node.Nodes.Clear();

            // Get the directory path from the node tag
            string path = (string)node.Tag;

            try
            {
                // Create a DirectoryInfo object for the directory path
                DirectoryInfo directory = new DirectoryInfo(path);

                // Get the subdirectories of the directory and create tree nodes for them
                foreach (DirectoryInfo subdirectory in directory.GetDirectories())
                {
                    TreeNode subnode = new TreeNode(subdirectory.Name);
                    subnode.Tag = subdirectory.FullName;
                    subnode.ImageIndex = 0;
                    subnode.SelectedImageIndex = 1;
                    node.Nodes.Add(subnode);
                }

                // Get the files in the directory and create tree nodes for them
                foreach (FileInfo file in directory.GetFiles())
                {
                    TreeNode subnode = new TreeNode(file.Name);
                    subnode.Tag = file.FullName;
                    subnode.ImageIndex = 2;
                    subnode.SelectedImageIndex = 3;
                    node.Nodes.Add(subnode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // If we are not authorized to access the directory, add a dummy node indicating this
                node.Nodes.Add("Access Denied");
            }
        }

        public FileSystemTree fileSystemTree = new FileSystemTree();
        private void Form1_Load(object sender, EventArgs e)
        {
            // لود درخت فایل از فایل dsfs
            var dataSaver = new DataSaver();
            
            var rootNode = dataSaver.Load("defualt.dsfs");
            if (rootNode != null)
            {
                fileSystemTree.Root = rootNode;
                PopulateTreeView(fileSystemTree.Root);
            }

        }

      
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ذخیره درخت فایل در فایل dsfs
            var dataSaver = new DataSaver();
            dataSaver.Save(fileSystemTree, "tree.dsfs");
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {

        }

        private void BackButton_Click(object sender, EventArgs e)
        {

        }
    }
}
