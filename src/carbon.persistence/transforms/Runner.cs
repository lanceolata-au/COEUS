using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using carbon.persistence.transforms.scripts;
using MySql.Data.MySqlClient;

namespace carbon.persistence.transforms
{
    public class Runner
    {
        private MySqlConnection _connection;

        private static string dbName = "carbon"; 

        public Runner(string connectionString, bool resetTheWorld = false)
        {
            _connection = new MySqlConnection {ConnectionString = connectionString};
            
            _connection.Open();

            if (_connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Could not open connection to MySql server using connection string.");
            }


            if (_connection.State == ConnectionState.Open)
            {
                CompleteUpgrate();
            }
                
        }

        private void CompleteUpgrate()
        {
            //TODO create DB method if not exists
            _connection.ChangeDatabase(dbName);

            var command = _connection.CreateCommand();
            command.CommandText = "SHOW TABLES;";

            var reader = command.ExecuteReader();

            var tables = new List<string>();
            
            while (reader.Read())
            {
                var currentRow = "";
                for (int i = 0; i < reader.FieldCount; i++)
                    currentRow += reader.GetValue(i);
                
                tables.Add(currentRow); 
                
            }

            if (!tables.Contains("schemaversions"))
            {
                Console.WriteLine("init.sql");
                var script = new MySqlScript(_connection, CoreResources.Init());
                script.Execute();
            }

        }
        
    }
}