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
            
            _connection.Close();

            if (resetTheWorld)
            {
                _connection.Open();
                Console.WriteLine("Dropping all tables");
                var script = new MySqlScript(_connection, CoreResources.DropAll(dbName));
                script.Execute();
            }
            
            if (_connection.State == ConnectionState.Open)
            {
                CompleteUpgrate();
            }
                
        }

        private void CompleteUpgrate()
        {
            //TODO create DB method if not exists
            try
            {
                _connection.ChangeDatabase(dbName);
            }
            catch (Exception e) 
            {
                if (e.Message.Contains("database doesn't exist"))
                {
                    _connection.Open();
                    Console.WriteLine("Creating DB" + dbName);
                    var script = new MySqlScript(_connection, CoreResources.CreateDb(dbName));
                    script.Execute();
                    _connection.Close();
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            
            var command = _connection.CreateCommand();
            command.CommandText = "SHOW TABLES;";

            _connection.Open();
            var reader = command.ExecuteReader();

            var tables = new List<string>();
            
            while (reader.Read())
            {
                var currentRow = "";
                for (int i = 0; i < reader.FieldCount; i++)
                    currentRow += reader.GetValue(i);
                
                tables.Add(currentRow); 
                Console.WriteLine(currentRow);
                
            }

            _connection.Close();
            
            if (!tables.Contains("schemaversions"))
            {
                _connection.Open();
                Console.WriteLine("init.sql");
                var script = new MySqlScript(_connection, CoreResources.Init());
                script.Execute();
            }
            
            

        }
        
    }
}