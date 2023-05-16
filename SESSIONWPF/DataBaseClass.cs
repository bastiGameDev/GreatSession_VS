using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SESSIONWPF
{
    public class DataBaseClass
    {
        public static string ConnectionString = App.ConnectionString;
        public SqlConnection connection = new SqlConnection(ConnectionString);
        private SqlCommand command = new SqlCommand();
        public DataTable resultTable = new DataTable();
        public SqlDependency dependency = new SqlDependency();
        public enum act { select, manipulation };
        public void sqlExecute(string quety, act act)
        {
            command.Connection = connection;
            command.CommandText = quety;
            command.Notification = null;
            switch (act)
            {
                case act.select:
                    dependency.AddCommandDependency(command);
                    SqlDependency.Start(connection.ConnectionString);
                    connection.Open();
                    resultTable.Load(command.ExecuteReader());
                    connection.Close();
                    break;
                case act.manipulation:
                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка:" + ex.Message.ToString());
                    }
                    connection.Close();
                    break;
            }
        }
    }
}
