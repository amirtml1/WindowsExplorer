using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        public long fileSize;
        private void button4_Click(object sender, EventArgs e)
        {
            // Create a new instance of your tree and populate it with data
            FileSystemTree myTree = new FileSystemTree();
            // Populate the tree with nodes and data

            // Specify the desired file size in bytes
            fileSize = Convert.ToInt32(sizetextbox.Text)* 1024 ; //KB

            // Save the tree to a file with the specified file size
            TreeSerializer.SerializeTree(myTree, "default.dsfs", fileSize);

            // Load the tree from the file
            FileSystemTree loadedTree = TreeSerializer.DeserializeTree("default.dsfs");


            // Now you can use the loadedTree object to work with the tree data structure.
            
            //save the tree back to file 
            TreeSerializer.SerializeTree(loadedTree, "default.dsfs", fileSize);
        }

        public static class TreeSerializer
        {
            public static void SerializeTree(FileSystemTree tree, string filename)
            {
                using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                {
                    
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, tree);
                }
            }
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
        public void UpdateTreeView()
        {
            driveTreeView.Nodes.Clear(); // Clear existing nodes
            FileSystemTree loadedTree = TreeSerializer.DeserializeTree("default.dsfs");
            // Populate the TreeView with DSFS directories and files


            TreeNode directoryNode = CreateTreeNode(loadedTree.Root);
            driveTreeView.Nodes.Add(directoryNode);

         
        }


        // CreateTreeNode method to recursively create TreeNodes for directories and files
        private TreeNode CreateTreeNode(Node directory)
        {
            TreeNode node = new TreeNode(directory.Name);

            foreach (var file in directory.Children)
            {
                if (!file.IsDirectory)
                {
                    TreeNode fileNode = new TreeNode(file.Name);
                    node.Nodes.Add(fileNode);
                }
                
            }

            foreach (var subDirectory in directory.Children)
            {
                if (subDirectory.IsDirectory)
                {
                    TreeNode subDirectoryNode = CreateTreeNode(subDirectory);
                    node.Nodes.Add(subDirectoryNode);
                }
                
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

        //load listview
        private void button6_Click(object sender, EventArgs e)
        {
            UpdateListView();
        }

        public void UpdateListView()
        {
            fileListView.Items.Clear(); // Clear existing items
            FileSystemTree loadedTree = TreeSerializer.DeserializeTree("default.dsfs");

            // Populate the ListView with DSFS directories and files
            foreach (var item in loadedTree.Root.Children)
            {
                if (item.IsDirectory)
                {
                    PopulateListView(item, null);
                }
                else
                {
                    ListViewItem fileItem = new ListViewItem(item.Name);
                    fileItem.SubItems.Add("File"); // Add additional sub-items as needed
                    fileListView.Items.Add(fileItem);
                }
            }
        }

        // Recursive method to populate the ListView control
        // Recursive method to populate the ListView control
        private void PopulateListView(Node node, ListViewItem parentItem)
        {
            if (node.IsDirectory)
            {
                ListViewItem directoryItem = new ListViewItem(node.Name);
                directoryItem.SubItems.Add("Directory"); // Add additional sub-items as needed

                if (parentItem == null)
                {
                    fileListView.Items.Add(directoryItem); // Add top-level directory items
                }
                else
                {
                    parentItem.SubItems.Add(new ListViewItem.ListViewSubItem(directoryItem, "Directory")); // Add subdirectory items as sub-items of parent items
                }

                foreach (var child in node.Children)
                {
                    if (child.IsDirectory)
                    {
                        PopulateListView(child, directoryItem); // Recursively process subdirectories
                    }
                    else
                    {
                        ListViewItem fileItem = new ListViewItem(child.Name);
                        fileItem.SubItems.Add("File"); // Add additional sub-items as needed
                        fileListView.Items.Add(fileItem); // Add file items directly to the ListView control
                    }
                }
            }
        }

        private void fileListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Check if the right mouse button is clicked
            if (e.Button == MouseButtons.Right)
            {
                // Get the ListViewItem at the clicked location
                if (fileListView.GetItemAt(e.X, e.Y) is ListViewItem)
                {
                    // Show the context menu at the clicked location
                    contextMenuStrip.Show(fileListView, e.Location);
                }
            }
        }

        private void fileListView_MouseClick(object sender, MouseEventArgs e)
        {
            // Check if the right mouse button is clicked
            if (e.Button == MouseButtons.Right)
            {
                // Get the ListViewItem at the clicked location
                if (fileListView.GetItemAt(e.X, e.Y) is ListViewItem)
                {
                    // Show the context menu at the clicked location
                    contextMenuStrip.Show(fileListView, e.Location);
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = fileListView.SelectedItems[0];
                string selectedDirectory = selectedItem.Text;

                fileListView.Items.Clear(); // Clear existing items
                FileSystemTree loadedTree = TreeSerializer.DeserializeTree("default.dsfs");
                Node node = loadedTree.FindNode(selectedDirectory);
                // Populate the ListView with DSFS directories and files
                foreach (var item in node.Children)
                {
                    if (item.IsDirectory)
                    {
                        PopulateListView(item, null);
                    }
                    else
                    {
                        ListViewItem fileItem = new ListViewItem(item.Name);
                        fileItem.SubItems.Add("File"); // Add additional sub-items as needed
                        fileListView.Items.Add(fileItem);
                    }
                }
            }
        }
        public static string dataforform2;
        public static bool IsOne = false;

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsOne = true;
            ListViewItem selectedItem = fileListView.SelectedItems[0];
            dataforform2= selectedItem.Text;
            var m = new newFolder();
            m.Show();
            
        }

      

        private void fileListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            contextMenuStrip.Show( Cursor.Position.X, Cursor.Position.Y);
        }

        private void driveTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            // Check if the right mouse button is clicked
            if (e.Button == MouseButtons.Right)
            {
                // Get the ListViewItem at the clicked location
                if (driveTreeView.GetNodeAt(e.X, e.Y) is TreeNode)
                {
                    // Show the context menu at the clicked location
                    contextMenuStrip1.Show(driveTreeView, e.Location);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
                string selectedDirectory = driveTreeView.SelectedNode.Text;

                fileListView.Items.Clear(); // Clear existing items
                FileSystemTree loadedTree = TreeSerializer.DeserializeTree("default.dsfs");
                Node node = loadedTree.FindNode(selectedDirectory);
                // Populate the ListView with DSFS directories and files
                foreach (var item in node.Children)
                {
                    if (item.IsDirectory)
                    {
                        PopulateListView(item, null);
                    }
                    else
                    {
                        ListViewItem fileItem = new ListViewItem(item.Name);
                        fileItem.SubItems.Add("File"); // Add additional sub-items as needed
                        fileListView.Items.Add(fileItem);
                    }
                }
           
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            IsOne = false;
            dataforform2 = driveTreeView.SelectedNode.Text; 
            var m = new newFolder();
            m.Show();
        }
    }
}
