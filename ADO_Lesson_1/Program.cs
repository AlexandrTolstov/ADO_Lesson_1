using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleApp1
{
    class Program
    {
        private static string connectionString = "";

        static Program()
        {
            connectionString = @"Data Source = SD\ITSTEP_SQLSERVER; Initial Catalog=Kassa24; User Id=Shag; Password=1;";
        }
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            //событие на изменение состояния connection
            con.StateChange += Con_StateChange;
            con.InfoMessage += Con_InfoMessage;

            SqlCommand smd = new SqlCommand();
            smd.Connection = con;

            SqlParameter paramLogo = new SqlParameter();
            paramLogo.ParameterName = "Logo";
            paramLogo.Value = "SQL LOGO";
            paramLogo.DbType = DbType.String;
            smd.Parameters.Add(paramLogo);

            smd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "Name",
                Value = "Name Company",
                DbType = DbType.String

            });

            smd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "Tax",
                Value = 20,
                DbType = DbType.Double

            });

            smd.CommandText = "execute [dbo].[CreateOperator] @Logo, @Name, @Tax";



            try
            {
                con.Open();

                var result = smd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Dispose();
            }
        }

        static void Example1()
        {
            Console.ForegroundColor = ConsoleColor.White;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            //событие на изменение состояния connection
            con.StateChange += Con_StateChange;
            con.InfoMessage += Con_InfoMessage;

            SqlCommand smd = new SqlCommand();
            smd.Connection = con;
            //smd.CommandText = "select * from Table_Operator; select * from Table_Prefix";
            smd.CommandText = "print ' -> Table_Operator'; select * from Table_Operator; print ' -> Table_Prefix'; select * from Table_Prefix";

            #region ExecuteReader
            try
            {
                con.Open();

                SqlDataReader dr = smd.ExecuteReader();

                //если только один запрос
                //while (dr.Read())
                //{
                //    string str = string.Format("ID: {0}\t{1} - {2}", dr[0], dr[2], dr[3]);

                //    Console.WriteLine(str);
                //}

                //если два запроса сразу
                do
                {
                    if (dr.GetSchemaTable().TableName == "Table_Operator")
                    {
                        while (dr.Read())
                        {
                            string str = string.Format("ID: {0}\t{1} - {2}", dr[0], dr[2], dr[3]);

                            Console.WriteLine(str);
                        }
                    }

                } while (dr.NextResult());

                dr.Close();
                #endregion
                #region Execute Scalar
                smd.CommandText = "select count(*) from Table_Operator";
                int count = Convert.ToInt32(smd.ExecuteScalar());
                Console.WriteLine("Table Operator {0} count", count);
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Dispose();
            }

        }

        private static void Con_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            SqlConnection con = sender as SqlConnection;
            Console.WriteLine("{0:hh:mm:ss} - {1}", DateTime.Now, e.Message);
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static void Con_StateChange(object sender, StateChangeEventArgs e)
        {
            SqlConnection con = sender as SqlConnection;
            Console.WriteLine("{0:hh:mm:ss} - {1}", DateTime.Now, con.State);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}