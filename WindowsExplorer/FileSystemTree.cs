using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsExplorer
{
    [Serializable]
    public class Node
    {
        // نام درخت
        public string Name { get; set; }

        // آیا این گره یک پوشه است؟ 
        public bool IsDirectory { get; set; }

        // لیست فرزندان
        public List<Node> Children { get; set; }

        // محتوای فایل
        public string Content { get; set; }
    }
    [Serializable]
    public class FileSystemTree
    {
        // ریشه درخت
        public Node Root { get; set; }

        public FileSystemTree()
        {
            this.Root = new Node() { Name = " This PC", IsDirectory = true, Children = new List<Node>() };
        }

        // تابع پیدا کردن گره با نام مشخص شده
        public Node FindNode(string name)
        {
            return this.FindNodeHelper(this.Root, name);
        }

        private Node FindNodeHelper(Node node, string name)
        {
            if (node.Name == name)
                return node;

            if (node.IsDirectory)
            {
                foreach (var child in node.Children)
                {
                    var foundNode = this.FindNodeHelper(child, name);
                    if (foundNode != null)
                        return foundNode;
                }
            }
           
            return null;
        }

       
        
     

        private Node FindParentNode(string name)
        {
            return this.FindParentNodeHelper(this.Root, name);
        }

        private Node FindParentNodeHelper(Node node, string name)
        {
            if (node.IsDirectory)
            {
                foreach (var child in node.Children)
                {
                    if (child.Name == name)
                        return node;

                    var parentNode = this.FindParentNodeHelper(child, name);
                    if (parentNode != null)
                        return parentNode;
                }
            }

            return null;
        }
        // تابع افزودن یک پوشه با نام مشخص شده به درخت
        public void AddFolder(string name, string parentName)
        {
            var parentNode = this.FindNode(parentName);

            if (parentNode.IsDirectory)
            {
                var folderNode = new Node() { Name = name, IsDirectory = true, Children = new List<Node>() };
                parentNode.Children.Add(folderNode);
            }
        }

        public void AddFile(string name, string parentName, string content)
        {
            var parentNode = this.FindNode(parentName);

            if (parentNode.IsDirectory)
            {
                var fileNode = new Node() { Name = name, IsDirectory = false, Content = content };
                parentNode.Children.Add(fileNode);
            }
        }

        public void RemoveNode(string name)
        {
            var parentNode = this.FindParentNode(name);

            if (parentNode != null && parentNode.IsDirectory)
            {
                var nodeToRemove = parentNode.Children.FirstOrDefault(n => n.Name == name);
                parentNode.Children.Remove(nodeToRemove);
            }
        }

        public void Display()
        {
            Console.WriteLine("PC This");
            DisplayHelper(this.Root, "   ");
        }

        private void DisplayHelper(Node node, string indent)
        {
            if (node == null)
                return;

            Console.Write(indent);
            Console.Write("--");
            Console.Write(node.Name);

            if (!node.IsDirectory)
                Console.Write($" ({node.Content.Length} bytes)");

            Console.WriteLine();

            if (node.IsDirectory)
            {
                foreach (var child in node.Children)
                {
                    DisplayHelper(child, indent + "   ");
                }
            }
        }



        public void RemoveEmptyDirectories()
        {
            RemoveEmptyDirectoriesHelper(this.Root);
        }

        private bool RemoveEmptyDirectoriesHelper(Node node)
        {
            if (!node.IsDirectory)
                return false;

            bool isEmpty = true;

            for (int i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                if (RemoveEmptyDirectoriesHelper(child))
                {
                    node.Children.Remove(child);
                    i--;
                }
                else
                {
                    isEmpty = false;
                }
            }

            return isEmpty;
        }

        public void CopyFile(string sourceName, string destinationName)
        {
            var sourceNode = this.FindNode(sourceName);
            var destinationParentName = destinationName.Substring(0, destinationName.LastIndexOf('/'));
            var destinationParentNode = this.FindNode(destinationParentName);

            if (!sourceNode.IsDirectory && destinationParentNode.IsDirectory)
            {
                var newFileNode = new Node() { Name = destinationName.Substring(destinationName.LastIndexOf('/') + 1), IsDirectory = false, Content = sourceNode.Content };
                destinationParentNode.Children.Add(newFileNode);
            }
        }

        public void MoveFile(string sourceName, string destinationName)
        {
            var sourceNode = this.FindNode(sourceName);
            var destinationParentName = destinationName.Substring(0, destinationName.LastIndexOf('/'));
            var destinationParentNode = this.FindNode(destinationParentName);

            if (!sourceNode.IsDirectory && destinationParentNode.IsDirectory)
            {
                var newFileNode = new Node() { Name = destinationName.Substring(destinationName.LastIndexOf('/') + 1), IsDirectory = false, Content = sourceNode.Content };
                destinationParentNode.Children.Add(newFileNode);
                var parentNode = this.FindParentNode(sourceName);
                parentNode.Children.Remove(sourceNode);
            }
        }


        public void ConvertContentToString(string fileName)
        {
            var fileNode = this.FindNode(fileName);

            if (!fileNode.IsDirectory)
            {
                fileNode.Content = fileNode.Content.ToString();
            }
        }

        

        public string ReadFile(string fileName)
        {
            var fileNode = this.FindNode(fileName);

            if (!fileNode.IsDirectory)
            {
                return fileNode.Content;
            }

            return null;
        }


        public List<string> SearchFiles(string extension)
        {
            var result = new List<string>();
            SearchFilesHelper(this.Root, extension, result);
            return result;
        }

        private void SearchFilesHelper(Node node, string extension, List<string> result)
        {
            if (node == null)
                return;

            if (!node.IsDirectory && node.Name.EndsWith(extension))
            {
                result.Add(node.Name);
            }

            if (node.IsDirectory)
            {
                foreach (var child in node.Children)
                {
                    SearchFilesHelper(child, extension, result);
                }
            }
        }



        public List<string> SearchFolders(string folderName)
        {
            var result = new List<string>();
            SearchFoldersHelper(this.Root, folderName, result);
            return result;
        }

        private void SearchFoldersHelper(Node node, string folderName, List<string> result)
        {
            if (node == null)
                return;

            if (node.IsDirectory && node.Name == folderName)
            {
                result.Add(node.Name);
            }

            if (node.IsDirectory)
            {
                foreach (var child in node.Children)
                {
                    SearchFoldersHelper(child, folderName, result);
                }
            }
        }




    }
}
