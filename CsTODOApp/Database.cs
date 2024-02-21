using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;


namespace CsTODOApp
{
    internal class Database
    {
        private MySqlConnection mysqlconnection;
        private string serverName = "localhost";
        private string databaseName = "csharptest";
        private string username = "root";
        private string password = "";
        private string tableName = "todo";

        public Database()
        {
            Init();
        }

        private void Init()
        { 
            string connectionString = $"SERVER={serverName};DATABASE={databaseName};UID={username};PASSWORD={password};";
            mysqlconnection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                mysqlconnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                mysqlconnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Insert(string message)
        {
            string table = $"{tableName} (EntryDate, Message)";
            string values = $"(NOW(), '{message}')";
            string query = $"INSERT INTO {table} VALUES {values}";
            Console.WriteLine(query);
            if (this.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand(query, mysqlconnection);
                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void Update()
        {
            string query = $"UPDATE {tableName} SET name='Joe', age='25' WHERE name='John'";

            if (this.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand(query, mysqlconnection);
                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void Delete()
        {
            string query = $"DELETE FROM {tableName} WHERE name='Joe'";

            if (this.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand (query, mysqlconnection);
                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public List<string>[] Select(int quantity)
        {
            string query = $"SELECT * FROM {tableName} LIMIT {quantity}";

            List <string>[] li = new List<string>[3];
            li[0] = new List<string>();
            li[1] = new List<string>();
            li[2] = new List<string>();

            if (this.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand(query, mysqlconnection);
                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    li[0].Add($"{dataReader["id"]}");
                    li[1].Add($"{dataReader["name"]}");
                    li[2].Add($"{dataReader["age"]}");
                }

                dataReader.Close();
                this.CloseConnection();

                return li;
            }
            else
            {
                li[0].Add("ERROR");
                li[1].Add("CONNECTING");
                li[2].Add("TO DATABASE");

                return li;
            }
        }
    }
}
