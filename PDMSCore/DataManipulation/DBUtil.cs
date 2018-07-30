using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

    }
}
