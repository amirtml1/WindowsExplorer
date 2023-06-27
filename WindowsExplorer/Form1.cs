using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        private void button4_Click(object sender, EventArgs e)
        {
            // Create a new instance of your tree and populate it with data
            FileSystemTree myTree = new FileSystemTree();
            // Populate the tree with nodes and data

            // Specify the desired file size in bytes
            long fileSize = Convert.ToInt32(sizetextbox.Text)* 1024 ; //KB

            // Save the tree to a file with the specified file size
            TreeSerializer.SerializeTree(myTree, "defualt.dsfs", fileSize);

            // Load the tree from the file
            FileSystemTree loadedTree = TreeSerializer.DeserializeTree("defualt.dsfs");


            // Now you can use the loadedTree object to work with the tree data structure.
            loadedTree.AddFolder("D", loadedTree.Root.Name);


        }
        public static class TreeSerializer
        {
            public static void SerializeTree(FileSystemTree tree, string filename, long fileSize)
            {
                using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                {
                    fileStream.SetLength(fileSize); // Set the desired file size
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, tree);
                }
            }

            public static FileSystemTree DeserializeTree(string filename)
            {
                using (FileStream fileStream = new FileStream(filename, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    return (FileSystemTree)binaryFormatter.Deserialize(fileStream);
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            UpdateTreeView();


        }
        // UpdateTreeView method to populate the TreeView control
        private void UpdateTreeView()
        {
            driveTreeView.Nodes.Clear(); // Clear existing nodes
            FileSystemTree loadedTree = TreeSerializer.DeserializeTree("defualt.dsfs");
            // Populate the TreeView with DSFS directories and files
            foreach (var directory in loadedTree.Root.Children)
            {
                TreeNode directoryNode = CreateTreeNode(directory);
                driveTreeView.Nodes.Add(directoryNode);
            }

            foreach (var file in loadedTree.Root.Children)
            {
                TreeNode fileNode = new TreeNode(file.Name);
                driveTreeView.Nodes.Add(fileNode);
            }
        }

        // CreateTreeNode method to recursively create TreeNodes for directories and files
        private TreeNode CreateTreeNode(Node directory)
        {
            TreeNode node = new TreeNode(directory.Name);

            foreach (var file in directory.Children)
            {
                TreeNode fileNode = new TreeNode(file.Name);
                node.Nodes.Add(fileNode);
            }

            foreach (var subDirectory in directory.Children)
            {
                TreeNode subDirectoryNode = CreateTreeNode(subDirectory);
                node.Nodes.Add(subDirectoryNode);
            }

            return node;
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
                    ListDirectory(driveTreeView, fdb.SelectedPath);
                }
            }
            
        }


        











        //متد برای اوردن فایل ها در تری ویو
        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);

            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }
        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
            {
                
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            }
            foreach (var file  in directoryInfo.GetFiles())
            {
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            }
            return directoryNode;
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
