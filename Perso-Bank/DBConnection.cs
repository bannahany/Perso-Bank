using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Perso_Bank
{
    internal class DBConnection
    {

        private readonly static string connectionString = "Data Source=localhost;Initial Catalog=Perso-BankDB;User ID=sa;Password=sa;Pooling=true;TrustServerCertificate=true;";

        //private readonly static string connectionString = "server=127.0.0.1;database=emh_db;uid=dbuser;password=Marwareem2000;Pooling=true;";
        //   private readonly static string connectionString = "server=192.168.0.7;database=emh_db;uid=dbuser;password=Marwareem2000;Pooling=true;";
        private static DBConnection? DBConn = null;
        private SqlConnection? connection = null;

        protected DBConnection()
        {

        }




        public static SqlConnection? getConnection()
        {
            DBConn ??= new DBConnection();
            if (DBConn.connection == null)
            {
                try
                {
                    SqlConnection connection = new(connectionString);
                    connection.Open();
                    return connection;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            else
            {
                ConnectionState isAlive = DBConn.connection.State;
                if (isAlive == ConnectionState.Open)
                {
                    return DBConn.connection;
                }
                else
                {
                    try
                    {
                        DBConn.connection.Close();
                        DBConn.connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    isAlive = DBConn.connection.State;
                    if (isAlive == ConnectionState.Open) return DBConn.connection;
                    else throw new Exception("Database Connection Error. \nTried to bring online but ping failed on second attempt.");
                }
            }
        }
    }
}
