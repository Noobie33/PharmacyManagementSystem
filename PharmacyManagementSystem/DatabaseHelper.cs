using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;
        }

        public SqlCommand GetCommand(string sql)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            return new SqlCommand(sql, conn);
        }

        public DataTable Execute(string sql)
        {
            using (SqlCommand cmd = GetCommand(sql))
            {
                return Execute(cmd);
            }
        }

        public DataTable Execute(SqlCommand command)
        {
            DataTable dt = new DataTable();
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            finally
            {
                if (command.Connection != null)
                {
                    command.Connection.Close();
                    command.Connection.Dispose();
                }
            }
            return dt;
        }

        public int ExecuteNonQuery(string sql)
        {
            using (SqlCommand cmd = GetCommand(sql))
            {
                return ExecuteNonQuery(cmd);
            }
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                return command.ExecuteNonQuery();
            }
            finally
            {
                if (command.Connection != null)
                {
                    command.Connection.Close();
                    command.Connection.Dispose();
                }
            }
        }

        public object ExecuteScalar(SqlCommand command)
        {
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                return command.ExecuteScalar();
            }
            finally
            {
                if (command.Connection != null)
                {
                    command.Connection.Close();
                    command.Connection.Dispose();
                }
            }
        }
    }
}
