using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsExplorer
{
    public partial class newFolder : Form
    {
        public newFolder()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FileSystemTree loadedTree = Form1.TreeSerializer.DeserializeTree("default.dsfs");
            Node node;
            if (Form1.IsOne == true)
            {
                node = loadedTree.FindParentNode(Form1.dataforform2);
            }
            else
            {
                node = loadedTree.FindNode(Form1.dataforform2);
            }
            
            loadedTree.AddFolder(textBox1.Text, node.Name);
            Form1.TreeSerializer.SerializeTree(loadedTree, "default.dsfs");
            var mainForm = Application.OpenForms.OfType<Form1>().Single();
            mainForm.UpdateListView();
            mainForm.UpdateTreeView();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
