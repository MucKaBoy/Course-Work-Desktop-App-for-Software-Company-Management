using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace Курсовая_работа
{
    class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-UBR7CD6\SQLEXPRESS;Initial Catalog=Program;Integrated Security=True");
        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
        public int ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            OpenConnection();
            SqlCommand command = new SqlCommand(query, getConnection());
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            int result = command.ExecuteNonQuery();
            closeConnection();
            return result;
        }
        public int ExecuteUpdateQuery(string query, SqlParameter[] parameters, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(query, getConnection(), transaction);
            command.Parameters.AddRange(parameters);
            return command.ExecuteNonQuery();
        }


    }
}
