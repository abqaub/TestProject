using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TestProject
{
    class UtilityClass
    {
        public string GetJsonFile(string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
            string expectedResponse;

            using (var configFile = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(configFile, true))
                {
                    expectedResponse = reader.ReadToEnd();
                }
            }

            return expectedResponse;
        }

        public void WriteFile(IList<string> list, string fileName)
        {
            var timestamp = DateTime.Now.ToFileTime();
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            TextWriter tw = new StreamWriter(projectDirectory+"\\Results\\"+fileName+"_"+timestamp+".txt");
 
            foreach (var s in list)
                tw.WriteLine(s);

            tw.Close();
        }

        public void WriteCountFile(IList<string> list, string fileName)
        {
            var timestamp = DateTime.Now.ToFileTime();
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            TextWriter tw = new StreamWriter(projectDirectory + "\\Results\\" + fileName + "_" + timestamp + ".txt");

            tw.WriteLine("Total number of count " + list.Count);        
            tw.Close();
        }       
    }
}
