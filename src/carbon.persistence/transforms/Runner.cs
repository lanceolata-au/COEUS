using System;
using System.Collections.Generic;
using System.Data;
using carbon.persistence.transforms.scripts;
using MySql.Data.MySqlClient;

namespace carbon.persistence.transforms
{
    public class Runner
    {
        private readonly MySqlConnection _connection;

        private readonly string _dbName;

        public Runner(string connectionString, bool dropAll = false, bool startingData = false, string dbName = "carbon")
        {
            _connection = new MySqlConnection {ConnectionString = connectionString};

            this._dbName = dbName;
            
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
            try
            {
                _connection.ChangeDatabase(_dbName);
                
            }
            catch (Exception e) 
            {
                if (e is MySqlException && e.Message.Contains("Unknown database"))
                {
                    _connection.Close();
                    
                    Console.WriteLine("Creating DB " + _dbName);
                    var script = new MySqlScript(_connection, Resources.CreateDb(_dbName));
                    _connection.Open();
                    script.Execute();
                    
                    _connection.ChangeDatabase(_dbName);

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
                var script = new MySqlScript(_connection, Resources.DropAll(_dbName));
                script.Execute();
                
                _connection.ChangeDatabase(_dbName);
                
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

            var schemaVersions = RunCommand(@"SELECT `name` FROM `schemaversions`;");
            
            var executed = new List<string>();
            
            while (schemaVersions.Read())
            {
                var currentRow = "";
                for (int i = 0; i < schemaVersions.FieldCount; i++)
                    currentRow += schemaVersions.GetValue(i);
                
                executed.Add(currentRow);
                Console.WriteLine("Already run: " + currentRow);
                
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
            _connection.ChangeDatabase(_dbName);

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
            _connection.ChangeDatabase(_dbName);

            if (fileName != null)
            {
                Console.WriteLine("Execute: " + fileName);
            }

            var command = new MySqlCommand
            {
                Connection = _connection,
                CommandText = script
            };

            return command.ExecuteReader();

        }
        
    }
    
}