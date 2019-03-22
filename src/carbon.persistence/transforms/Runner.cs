using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace carbon.persistence.transforms
{
    public class Runner
    {
        private MySqlConnection _connection;

        public Runner(string connectionString)
        {
            _connection = new MySqlConnection {ConnectionString = connectionString};
            
            _connection.Open();

                if (_connection.State != ConnectionState.Open)
                {
                    throw new InvalidOperationException("Could not open connection to MySql server using connection string.");
                }
                
        }
        
    }
}