﻿using PDMSCore.BusinessObjects;
using System;
using System.Data;
using System.Data.SqlClient;

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
            return -1;
        }
    }
}
