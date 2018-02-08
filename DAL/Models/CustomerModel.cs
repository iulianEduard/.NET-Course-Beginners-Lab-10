using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CustomerModel
    {
        private readonly string _connectionString = string.Empty;

        public CustomerModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DisconectedExample()
        {
            DbProviderFactory df = DbProviderFactories.GetFactory("System.Data.SqlClient");

            using (DbConnection conn = df.CreateConnection())
            {
                conn.ConnectionString = _connectionString;

                conn.Open();

                DbCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Customer";

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    Console.WriteLine("Your data reader object is a: {0}", dr.GetType().Name);
                    while (dr.Read())
                    {
                        Console.WriteLine("-> Customer #{0}, name {1}.", dr["Id"], dr["Name"].ToString());
                    }
                }
                Console.ReadLine();

                conn.Close();
            }
        }

        public void ConnectedExample()
        {
            string strSQL = "Select * From Customer";
            SqlConnection sqlConn = new SqlConnection(_connectionString);
            SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn);

            using (SqlDataReader myDataReader = sqlCmd.ExecuteReader())
            {
                // Loop over the results.
                while (myDataReader.Read())
                {
                    Console.WriteLine("-> Id: {0}, Name: {1}, Email: {2}.",
                    myDataReader["Id"].ToString(),
                    myDataReader["Name"].ToString(),
                    myDataReader["Email"].ToString());
                }
            }
            Console.ReadLine();
        }

        public void ExecuteStoredProcedure(int id, string name)
        {
            SqlConnection sqlConn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = new SqlCommand("[usp_GetCustomerById]", sqlConn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@Id";
                param1.SqlDbType = SqlDbType.Int;
                param1.Value = id;

                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@Name";
                param2.SqlDbType = SqlDbType.VarChar;
                param2.Value = name;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("-> Id: {0}, Name: {1}, Email: {2}.",
                        reader["Id"].ToString(),
                        reader["Name"].ToString(),
                        reader["Email"].ToString());
                    }
                }
            }
        }
    }
}
