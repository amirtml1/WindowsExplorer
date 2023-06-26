using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace WindowsExplorer
{
   
    public class DataSaver
    {
        public void Save(FileSystemTree tree, string fileName)
        {
            var json = JsonConvert.SerializeObject(tree.Root);
            File.WriteAllText(fileName, json);
        }

        public Node Load(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            var json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<Node>(json);
        }
    }
}
