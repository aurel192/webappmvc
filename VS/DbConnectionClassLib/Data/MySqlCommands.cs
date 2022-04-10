using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DbConnectionClassLib.Data
{
    public static class MySqlCommands
    {
        public static List<db_log> Get_DbLog()
        {
            List<db_log> list = new List<db_log>();
            try
            {
                string connectionString = ConnectionString.Get();
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT Id, 'Key', TimeStamp, Value FROM db_log", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            db_log logEntry = new db_log()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Key = reader["Key"].ToString(),
                                TimeStamp = Convert.ToDateTime(reader["TimeStamp"]),
                                Value = reader["Value"].ToString()
                            };
                            list.Add(logEntry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            return list;
        }
    }

    public class db_log
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
    }
}


