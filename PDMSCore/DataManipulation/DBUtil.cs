using PDMSCore.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.DataManipulation
{
    public static class DBUtil
    {
        public enum UpdatedColumn
        {
            StringValue,
            IntValue,
            DateValue,
            FileValue
        }

        public static string GetString(DataRow dr, int column)
        {
            if (dr.ItemArray[column] == System.DBNull.Value)
                return "";
            return dr.ItemArray[column].ToString().Trim();
        }
        public static int GetInt(DataRow dr, int column)
        {
            int? FieldID = dr.ItemArray[0] == null ? (int?)null : (int)dr.ItemArray[0];

            if (dr.ItemArray[column] == System.DBNull.Value)
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
            
            return 0;
        }

        public static int UpdateFieldsValuesTable(FieldValueUpdateInfo UpdateInfo, UpdatedColumn uc, Object newValue)
        {

            //List<Field> ret = new List<Field>();
            //SqlCommand sql = new SqlCommand("UPDATE Fields f SET Address = @add, City = @cit Where FirstName = @fn and LastName = @add");

            //SqlCommand sql = new SqlCommand("GetPanelFromID", UpdateInfo.con);
            //sql.CommandType = CommandType.StoredProcedure;
            //sql.Parameters.Add(new SqlParameter("PanelID", this.id));
            //sql.Parameters.Add(new SqlParameter("CompanyID", CompanyID));
            //sql.Parameters.Add(new SqlParameter("LanguageID", LanguageID));

            //try
            //{
            //    using (SqlDataReader sdr = sql.ExecuteReader())
            //    {
            //        while (sdr.Read())
            //        {
            //            return sdr.GetString(2).Trim();
            //        }
            //    }
            //}



            return -1;
        }

    }
}
