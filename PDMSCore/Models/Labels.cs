using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
        public DataGridField2 DataGrid { get; set; }
        //public List<LabelItem> list { get; set; } = new List<LabelItem>();
        public string ID { get; set; }

        public Labels(string ID)
        {
            this.ID = ID;
            //list = new List<LabelItem>();
        }

        public Labels LoadLabelsFromDB(int CompanyID = -1, string LanguageID = "", int LabelID = -1)
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
            return this;
        }

        

        private void ProcessLabels(DataTable dt)
        {
            //list = new List<LabelItem>();
            //for (int r = 0; r < dt.Rows.Count; r++)
            //{
            //    int LabelID = DBUtil.GetInt(dt.Rows[r], 0);
            //    string LanguageID = DBUtil.GetString(dt.Rows[r], 1);
            //    string Label = DBUtil.GetString(dt.Rows[r], 2);
            //    int CompanyID = DBUtil.GetInt(dt.Rows[r], 3);

            //    LabelItem li = new LabelItem() { ID = LabelID, Language = LanguageID, Label = Label, CompanyID = CompanyID };
            //    list.Add(li);
            //}

            if (ID == null || ID == "")
                DataGrid = new DataGridField2("Dg" + DateTime.Now.Millisecond,dt);
            else
                DataGrid = new DataGridField2("Dg" + ID, dt);
        }
       
        public TagBuilder HtmlText()
        {
            return null;
        }

        public void Save(IFormCollection fc)
        {
            using (SqlConnection con = new SqlConnection(DBUtil.GetSqlConnectionString()))
            {
                SaveDifferences(fc);
            }

            return;
        }

        private void SaveDifferences(IFormCollection fc)
        {
            for (int r = 0; r < DataGrid.RowCount; r++)
            {
                TableRow2 tr = DataGrid.GetRow(r);
                for (int c = 0; c < tr.Cells.Count; c++)
                {
                    string HTMLId = tr.Cells[c].HTMLFieldID;
                    StringValues NewValue = new StringValues();
                    if (fc.TryGetValue(HTMLId, out NewValue))
                    {
                        if (tr.Cells[c].GetValue() == NewValue.ToString())
                            continue;
                        else
                        {   //  Zmena hodnoty. TODO

                        }

                    }
                    else
                    {

                    }




                }


            }


        }


    }

}
