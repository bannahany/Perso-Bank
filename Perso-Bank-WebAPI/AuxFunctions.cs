using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Perso_Bank
{
    internal class AuxFunctions
    {

        public enum DB_CRUD_TYPE
        {
            INSERT = 0,
            DELETE = 1,
            UPDATE = 2,
            INSERT_NO_AUTO = 3,
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;
            string pattern = @"^(\(\d{3}\)|\d{3})\d{3}-?\d{4}$";


            return Regex.IsMatch(phoneNumber, pattern);
        }




        public static bool executeMultipleStatements(List<string> stmts)
        {

            SqlConnection? conn = DBConnection.getConnection();
            SqlCommand cmd = new();
            SqlTransaction? transaction = null;

            try
            {
                transaction = conn?.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                foreach (string sql in stmts)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                transaction?.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction?.Rollback();
                throw;
            }

        }

        public static int executeSingleNonQuery(string sql, DB_CRUD_TYPE TYPE)
        {

            SqlConnection? conn = DBConnection.getConnection();
            SqlCommand cmd = new();
            SqlTransaction? transaction = null;

            try
            {
                transaction = conn?.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandText = sql;
                int x = cmd.ExecuteNonQuery();
                transaction?.Commit();
                if (TYPE == DB_CRUD_TYPE.INSERT)
                {
                    cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                    int ret = Convert.ToInt32(cmd.ExecuteScalar());

                    return ret;

                }
                else if (TYPE == DB_CRUD_TYPE.DELETE || TYPE == DB_CRUD_TYPE.UPDATE)
                {
                    return x;
                }
                else if (TYPE != DB_CRUD_TYPE.INSERT_NO_AUTO)
                {
                    return x;
                }

            }
            catch (SqlException)
            {
                transaction?.Rollback();
                throw;
            }
            catch (Exception)
            {
                transaction?.Rollback();


                throw;
            }

            return -1;
        }

        public static DataTable? executeSQLQuery(string sql)
        {
            SqlConnection? conn = DBConnection.getConnection();
            SqlCommand cmd = new(sql, conn)
            {
                CommandType = CommandType.Text
            };


            SqlDataAdapter adapter = new(cmd);
            DataTable dataTable = new();
            try
            {
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (SqlException) { throw; }
            catch (Exception) { throw; }
        }

        public static int executeNon_QueryStoredProcedure(string spName, object[,] p, DB_CRUD_TYPE TYPE)
        {
            SqlConnection? conn = DBConnection.getConnection();
            SqlCommand cmd = new(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (p != null)
            {
                for (int i = 0; i <= p.GetLength(0) - 1; i++)
                {
                    string pName = (string)p[i, 0];
                    object value = p[i, 1];
                    cmd.Parameters.AddWithValue(pName, value);
                } // end for
            } // end if

            int x;
            try
            {
                x = cmd.ExecuteNonQuery();
                if (TYPE == DB_CRUD_TYPE.INSERT)
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                else if (TYPE == DB_CRUD_TYPE.DELETE || TYPE == DB_CRUD_TYPE.UPDATE)
                {
                    return x;
                }
                else
                {
                    return -1;
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        } // end if

        public static DataTable? executeQueryStoredProcedure(string spName, object[,] p)
        {

            SqlConnection? conn = DBConnection.getConnection();
            SqlCommand cmd = new(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (p != null)
            {
                for (int i = 0; i <= p.GetLength(0) - 1; i++)
                {
                    string pName = (string)p[i, 0];
                    object value = p[i, 1];
                    cmd.Parameters.AddWithValue(pName, value);
                }
            }

            SqlDataAdapter adapter = new(cmd);
            DataTable dataTable = new();
            try
            {
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

