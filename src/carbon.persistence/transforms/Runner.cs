using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using carbon.persistence.transforms.scripts;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace carbon.persistence.transforms
{
    public class Runner
    {
        private MySqlConnection _connection;

        private static string dbName = "carbon"; 

        public Runner(string connectionString, bool dropAll = false, bool startingData = false)
        {
            _connection = new MySqlConnection {ConnectionString = connectionString};
            
            _connection.Open();

            if (_connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Could not open connection to MySql server using connection string.");
            }
            
            if (_connection.State == ConnectionState.Open)
            {
                CompleteUpgrate(dropAll, startingData);
            }
                
        }

        private void CompleteUpgrate(bool resetTheWorld, bool startingData)
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

            var schemaversions = RunCommand("SELECT `name` FROM `schemaversions`;");
            
            var executed = new List<string>();
            
            while (schemaversions.Read())
            {
                var currentRow = "";
                for (int i = 0; i < schemaversions.FieldCount; i++)
                    currentRow += schemaversions.GetValue(i);
                
                executed.Add(currentRow);
                Console.WriteLine("Already run");
                Console.WriteLine(currentRow);
                
            }
      
            foreach (var transform in Resources.Transforms())
            {
                if (executed.Contains(transform.Key)) continue;
                RunScript(transform.Value,transform.Key);
                RunScript(@"INSERT INTO `schemaversions` (`name`, `executed`) VALUES ('" + transform.Key + "', CURRENT_TIMESTAMP)");
            }

            if (startingData)
            {
                foreach (var transform in Resources.TestData())
                {
                    if (executed.Contains(transform.Key)) continue;
                    RunScript(transform.Value,transform.Key);
                    RunScript(@"INSERT INTO `schemaversions` (`name`, `executed`) VALUES ('" + transform.Key + "', CURRENT_TIMESTAMP)");

                }
                
            }

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
        
        private MySqlDataReader RunCommand(string script, string fileName = null)
        {
            _connection.Close();
            _connection.Open();
            _connection.ChangeDatabase(dbName);

            if (fileName != null)
            {
                Console.WriteLine("Execute: " + fileName);
            }
            
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.CommandText = script;

            return command.ExecuteReader();

        }
        
    }
    
}