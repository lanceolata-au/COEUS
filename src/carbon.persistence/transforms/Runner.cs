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
                CompleteUpgrate(resetTheWorld);
            }
                
        }

        private void CompleteUpgrate(bool resetTheWorld)
        {
            //TODO create DB method if not exists
            try
            {
                _connection.ChangeDatabase(dbName);
                
            }
            catch (Exception e) 
            {
                if (e is MySql.Data.MySqlClient.MySqlException && e.Message.Contains("Unknown database"))
                {
                    _connection.Close();
                    
                    Console.WriteLine("Creating DB " + dbName);
                    var script = new MySqlScript(_connection, Resources.CreateDb(dbName));
                    _connection.Open();
                    script.Execute();
                    
                    _connection.ChangeDatabase(dbName);

                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            
            if (resetTheWorld)
            {
                _connection.Close();
                _connection.Open();
                _connection.ChangeDatabase("mysql");
                Console.WriteLine("Dropping all tables");
                var script = new MySqlScript(_connection, Resources.DropAll(dbName));
                script.Execute();
                
                _connection.ChangeDatabase(dbName);
                
            }
            
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
                Console.WriteLine(currentRow);
                
            }
            
            if (!tables.Contains("schemaversions"))
            {
                RunScript(Resources.Init(),"init.sql");
            }


            var transforms = Resources.Transforms();

        }

        private void RunScript(string script, string fileName = null)
        {
            _connection.Close();
            _connection.Open();
            _connection.ChangeDatabase(dbName);

            if (fileName != null)
            {
                Console.WriteLine("Execute: " + fileName);
            }
            
            var runner = new MySqlScript(_connection, script);
            runner.Execute();
        }
        
    }
}