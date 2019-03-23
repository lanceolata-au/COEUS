using System.Dynamic;
using System.IO;
using System.Reflection;

namespace carbon.persistence.transforms.scripts
{
    public static class CoreResources
    {
        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
        private const string BasePath = "carbon.persistence.transforms.scripts.core";

        public static string Init()
        {
            using (Stream stream = Assembly.GetManifestResourceStream(BasePath + ".init.sql"))
            using (StreamReader reader = new StreamReader(stream))
                         {
                return reader.ReadToEnd();
            }
        }

        public static string DropAll(string dbName)
        {

            return
                @"DROP DATABASE " + dbName + @";"
              + @"GO;"
              + @"CREATE DATABASE " + dbName + @";";

        }

        public static string CreateDb(string dbName)
        {
            return @"CREATE DATABASE " + dbName + @";";
        }
        
    }
}