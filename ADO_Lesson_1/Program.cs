using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Http.Headers;

namespace ConsoleApplication3
{
    class Program
    {
        static string connetionString;

        static Program()
        {
            connetionString = @"Data Source = SD\ITSTEP_SQLSERVER; Initial Catalog=Kassa24; User Id=Shag; Password=1;";
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Exmpl04();
        }

        static void Exmpl04()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connetionString;

            //событие на измнения состояния Connection
            con.StateChange += Con_StateChange;
            con.InfoMessage += Con_InfoMessage;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Table_Operator; select * from Table_Prefix";

            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            Exmpl04_1(ref ds);
            //ds.Tables[0].AcceptChanges();

            //foreach (DataRow row in ds.Tables[0]
            //    .GetChanges(DataRowState.Modified)
            //    .Rows)
            //{

            //}

            try
            {
                //ds.Tables[0].TableName = "Table_Operator";


                #region select data
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (row.RowState == DataRowState.Deleted)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (row.RowState == DataRowState.Added)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("{0} - {1}\t{2}",
                        row["OperatorId"], row["Name"], row.RowState);

                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        Console.WriteLine("Before {0}: After  {1}",
                       row[i, DataRowVersion.Original],
                       row[i, DataRowVersion.Current]);
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                }
                #endregion


                DataRow dr = ds.Tables[0].NewRow();
                dr["Logo"] = "logo url2";
                dr["Name"] = "name oper2";
                dr["Tax"] = 5;
                ds.Tables[0].Rows.Add(dr);
                ds.AcceptChanges();

                try
                {
                    SqlCommandBuilder scb = new SqlCommandBuilder(da);
                    da.Update(ds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }



                #region

                //foreach (DataTable table in ds.Tables)
                //{
                //    Console.WriteLine(table.TableName);
                //    //foreach (DataColumn column in table.Columns)
                //    //{
                //    //    Console.WriteLine("\t-> {0}", column.ColumnName);
                //    //}

                //    Console.WriteLine("-------------------------------------");
                //    foreach (DataRow row in table.Rows)
                //    {
                //        var items = row.ItemArray;
                //        foreach (var cells in row.ItemArray)
                //        {
                //            Console.Write("{0}\t", cells);
                //        }
                //        Console.WriteLine("");
                //    }
                //}

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

        static void Exmpl04_1(ref DataSet ds)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                row["Logo"] = "-";
            }

            ////edit row
            //ds.Tables[0].Rows[0].ItemArray[1] = "-";
            //ds.Tables[0].Rows[1].ItemArray[1] = "-";
            //ds.Tables[0].Rows[2].ItemArray[1] = "-";
            //ds.Tables[0].Rows[3].ItemArray[1] = "-";

            ds.Tables[0].Rows.RemoveAt(6);

            DataRow dr = ds.Tables[0].NewRow();
            dr["Logo"] = "logo url2";
            dr["Name"] = "name oper2";
            ds.Tables[0].Rows.Add(dr);
        }


        //function return Cursor
        static void Exmpl03()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connetionString;

            //событие на измнения состояния Connection
            con.StateChange += Con_StateChange;
            con.InfoMessage += Con_InfoMessage;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "";

            try
            {
                con.Open();

                var result = cmd.ExecuteNonQuery();
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

        //procedure
        static void Exmpl02()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connetionString;

            //событие на измнения состояния Connection
            con.StateChange += Con_StateChange;
            con.InfoMessage += Con_InfoMessage;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;


            SqlParameter pramLogo = new SqlParameter
            {
                ParameterName = "Logo",
                Value = "SQL LOGO",
                DbType = DbType.String
            };
            cmd.Parameters.Add(pramLogo);

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "Name",
                Value = "NAME COMPANY",
                DbType = DbType.String
            });

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "Tax",
                Value = 20,
                DbType = DbType.Double
            });

            cmd.CommandText = "execute [dbo].[CreateOperator] @Logo, @Name, @Tax";

            try
            {
                con.Open();

                var result = cmd.ExecuteNonQuery();
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

        static void exmpl01()
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString = connetionString;

            //событие на измнения состояния Connection
            con.StateChange += Con_StateChange;
            con.InfoMessage += Con_InfoMessage;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "print '-> Table_Operator'; select * from Table_Operator; print '-> Table_Prefix'; select * from Table_Prefix";

            try
            {
                con.Open();

                #region  ExecuteReader
                SqlDataReader dr = cmd.ExecuteReader();

                //while (dr.Read())
                //{
                //    string str = string.Format("ID: {0}\t{1} - {2}",
                //        dr[0], dr[2], dr[3]);

                //    Console.WriteLine(str);
                //}

                do
                {
                    //if (dr.GetSchemaTable().TableName == "Table_Operator")
                    //{
                    while (dr.Read())
                    {
                        string str = string.Format("ID: {0}\t{1} ",
                            dr[0], dr[2]);

                        Console.WriteLine(str);
                    }
                    //}
                } while (dr.NextResult());

                dr.Close();

                #endregion

                #region  ExecuteScalar
                cmd.CommandText = "select count(*) from Table_Operator";
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                Console.WriteLine("Table_Operator {0} count", count);
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
            Console.WriteLine("{0:hh:mm:ss} - {1}",
                DateTime.Now, e.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Con_StateChange(object sender, StateChangeEventArgs e)
        {
            SqlConnection con = sender as SqlConnection;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0:hh:mm:ss} - {1}",
                DateTime.Now, con.State);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}