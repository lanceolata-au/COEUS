using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace carbon.runner.database.transforms.scripts
{
    public static class Resources
    {
        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
        private const string BasePath = "carbon.runner.database.transforms.scripts";

        public static string Init()
        {
            using (Stream stream = Assembly.GetManifestResourceStream(BasePath + ".core.init.sql"))
            using (StreamReader reader = new StreamReader(stream))
                         {
                return reader.ReadToEnd();
            }
        }

        public static string DropAll(string dbName)
        {

            return
                @"DROP DATABASE " + dbName + @";"
              + @"CREATE DATABASE " + dbName + @"
                    CHARACTER SET UTF16 
                    COLLATE utf16_unicode_ci;";

        }

        public static string CreateDb(string dbName)
        {
            return @"CREATE DATABASE " + dbName + @";";
        }
        
        public static List<KeyValuePair<string,string>> Transforms()
        {
            
            var returnList = new List<KeyValuePair<string,string>>();
            
            foreach (var manifestResourceName in Assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(BasePath + ".transforms"))
                {
                    using (Stream stream = Assembly.GetManifestResourceStream(manifestResourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        
                        returnList.Add(new KeyValuePair<string,string>(manifestResourceName.Replace(BasePath + ".transforms.",""),reader.ReadToEnd()));
                    }
                }
            }

            return returnList;

        }
        
        public static List<KeyValuePair<string,string>> TestData()
        {
            
            var returnList = new List<KeyValuePair<string,string>>();
            
            foreach (var manifestResourceName in Assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(BasePath + ".testdata"))
                {
                    using (Stream stream = Assembly.GetManifestResourceStream(manifestResourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        
                        returnList.Add(new KeyValuePair<string,string>(manifestResourceName.Replace(BasePath + ".testdata.",""),reader.ReadToEnd()));
                    }
                }
            }

            return returnList;

        }
        
    }
    
}