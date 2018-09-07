using Microsoft.AspNetCore.Mvc.Rendering;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.Models
{
    public class LabelItem
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public string Language { get; set; }
        public int CompanyID { get; set; }

        public LabelItem()
        {

        }

    }
    public class Labels
    {
        public DataGridField2 dgf { get; set; }
        public List<LabelItem> list { get; set; } = new List<LabelItem>();

        public Labels()
        {
            dgf = DataGridField2.GetTestData(1);
        }

        public bool LoadLabelsFromDB(int CompanyID = -1, string LanguageID = "", int LabelID = -1)
        {
            using (SqlConnection con = new SqlConnection(DBUtil.GetSqlConnectionString()))
            {
                con.Open();
                DataTable table = new DataTable();
                SqlDataAdapter sqlDataAdapter;

                List<Field> ret = new List<Field>();
                SqlCommand sql = new SqlCommand("GetLabels", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add(new SqlParameter("CompanyID", CompanyID));
                sql.Parameters.Add(new SqlParameter("LanguageID", LanguageID));
                sql.Parameters.Add(new SqlParameter("LabelID", LabelID));

                try
                {
                    sqlDataAdapter = new SqlDataAdapter(sql);
                    sqlDataAdapter.Fill(table);
                    ProcessLabels(table);
                }
                catch (Exception eee)
                {
                    ret.Add(new LabelTextAreaField("1", "Exception in LoadNavigation(..)", eee.ToString()));
                }
            }
            return false;
        }

        private void ProcessLabels(DataTable dt)
        {
            list = new List<LabelItem>();
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                int LabelID = DBUtil.GetInt(dt.Rows[r], 0);
                string LanguageID = DBUtil.GetString(dt.Rows[r], 1);
                string Label = DBUtil.GetString(dt.Rows[r], 2);
                int CompanyID = DBUtil.GetInt(dt.Rows[r], 3);

                LabelItem li = new LabelItem() { ID = LabelID, Language = LanguageID, Label = Label, CompanyID = CompanyID };
                list.Add(li);
            }
        }

        public TagBuilder HtmlText()
        {
            


            return null;
        }



    }
}
