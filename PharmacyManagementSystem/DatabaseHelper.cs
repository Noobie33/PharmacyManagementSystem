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
        public static string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;

        private static string _connectionString = string.Empty;

        public static string ConnectionString
        {
            get
            {
                if (_connectionString == string.Empty)
                {
                    _connectionString = CONNECTION_STRING;
                }
                return _connectionString;
            }
        }

        // Static method for getting a SQL command
        public static SqlCommand GetCommand(string sql)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand sqlCmd = new SqlCommand(sql, conn);
            return sqlCmd;
        }

        public static SqlCommand GetCommand(string sql, params SqlParameter[] parameters)
        {
            SqlCommand cmd = GetCommand(sql);
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);
            return cmd;
        }

        // Static method to execute SQL and return DataTable
        public static DataTable GetDataTable(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // Execute the command and return a DataTable
        public static DataTable Execute(SqlCommand command)
        {
            DataTable dt = new DataTable();
            try
            {
                command.Connection.Open(); // Open connection
                dt.Load(command.ExecuteReader()); // Load data from SqlDataReader into DataTable
            }
            catch (Exception ex)
            {
                // Log exception or handle it (optional)
                throw new Exception("Database operation failed: " + ex.Message);
            }
            finally
            {
                command.Connection.Close(); // Always close the connection
            }

            return dt; // Return the DataTable with data
        }


        // Execute Non-Query commands (Insert, Update, Delete)
        public static int ExecuteNonQuery(string sql)
        {
            SqlCommand cmd = GetCommand(sql);
            int result = 0;
            try
            {
                cmd.Connection.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            finally
            {
                cmd.Connection.Close();
            }
            return result;
        }

        // Overload ExecuteNonQuery for SqlCommand
        public static int ExecuteNonQuery(SqlCommand command)
        {
            int result = 0;
            try
            {
                command.Connection.Open();
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            finally
            {
                command.Connection.Close();
            }
            return result;
        }
    }
}
