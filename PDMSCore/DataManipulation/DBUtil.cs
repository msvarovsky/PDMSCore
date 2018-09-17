using PDMSCore.BusinessObjects;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PDMSCore.DataManipulation
{
    public static class DBUtil
    {
        public static string GetString(DataRow dr, int column)
        {
            if (dr.ItemArray[column] == System.DBNull.Value)
                return "";
            return dr.ItemArray[column].ToString().Trim();
        }
        public static int GetInt(DataRow dr, int column)
        {
            //int? FieldID = dr.ItemArray[0] == null ? (int?)null : (int)dr.ItemArray[0];
            if (dr == null || dr.ItemArray.Length < column || dr.ItemArray[column] == System.DBNull.Value)
                return -1;
            return (int)dr.ItemArray[column];
        }

        public static int RunSQLQuery(FieldValueUpdateInfo UpdateInfo, string sql)
        {
            try
            {
                UpdateInfo.con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, UpdateInfo.con))
                {
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
            }
            finally
            {
                UpdateInfo.con.Close();
            }
            return -1;
        }

        public static int RunSQLQuery(SqlConnection con, string sql)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    /*begin tran
                    GRANT INSERT ON Labels TO[martin]
                    GRANT SELECT ON Labels TO[martin]
                    GRANT UPDATE ON Labels TO[martin]
                    exec sp_table_privileges[Labels]
                    rollback    */

                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
            }
            finally
            {
            }
            return -1;
        }

        public static string GetSqlConnectionString()
        {
            string cd = Directory.GetCurrentDirectory();
            string l = "PDMSCore";
            int a = cd.IndexOf(l);
            string AttachDbFilename = cd.Substring(0, a + l.Length) + "\\PDMSCore\\wwwroot\\TestDB\\System.mdf;";

            string ret = "Data Source=(LocalDB)\\MSSQLLocalDB;" 
                + "AttachDbFilename=" + AttachDbFilename 
                //+ "Database = " + cd.Substring(0, a + l.Length) + "\\PDMSCORE\\WWWROOT\\TESTDB\\SYSTEM.MDF;"
                + "Connect Timeout=30;" 
                + "User Id=martin;" 
                + "Password=martin;";

            return ret;
        }
    }
}
