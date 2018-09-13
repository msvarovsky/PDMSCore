using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static PDMSCore.DataManipulation.WebStuffHelper;

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
        public DataGridField DataGrid { get; set; }
        //public List<LabelItem> list { get; set; } = new List<LabelItem>();
        public string ID { get; set; }

        public Labels(string ID)
        {
            this.ID = ID;
            

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
            if (ID == null || ID == "")
                DataGrid = new DataGridField("Dg" + DateTime.Now.Millisecond, dt);
            else
                DataGrid = new DataGridField(ID, dt);

            DataGrid.DbTableUniqueIDColumnNumber = 0;
            DataGrid.Columns[0].ReadOnly = true;
            DataGrid.Columns[0].Visible = false;
            DataGrid.Columns[0].Type = ColumnType.Text;

            DataGrid.Columns[1].ReadOnly = true;
            DataGrid.Columns[1].Type = ColumnType.Text;

            DataGrid.Columns[2].ReadOnly = true;
            DataGrid.Columns[2].Type = ColumnType.Text;

            DataGrid.Columns[3].Type = ColumnType.Text;


            //DataGrid.SetData();

            //DataGrid.AddRowDialog = new ModalDialog("TODO: en", "TODO: Add new label");
            //DataGrid.AddRowDialog.AddField
        }
       
        public TagBuilder HtmlText()
        {
            return null;
        }

        public TagBuilder AddLabelDialogHtml()
        {
            LoadLabelsFromDB();
            TagBuilder tbDiv = new TagBuilder("div");

            for (int c = 0; c < DataGrid.Columns.Count; c++)
            {
                LabelTextBoxField t = new LabelTextBoxField("ParentID", -1, DataGrid.Columns[c].Label, "");
                tbDiv.InnerHtml.AppendHtml(t.BuildHtmlTag());
            }

            TagBuilder tbRet = WebStuffHelper.ModalDialog(ID, "Add new row", "Description to label row add.", tbDiv,true);

            return tbRet;
        }

        public void Save(Dictionary<string, string> ClientDG)
        {
            List<DBTableUpdateTrio> differences = DataGrid.GetDifferences(ClientDG);


            string SQL = GetUpdateSQL(differences);
            if (SQL.Length != 0)
            {
                using (SqlConnection con = new SqlConnection(DBUtil.GetSqlConnectionString()))
                {
                    con.Open();
                    DBUtil.RunSQLQuery(con, SQL);
                }
            }
            return;
        }
        private string GetUpdateSQL(List<DBTableUpdateTrio> differences)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < differences.Count; i++)
            {
                string UpdateSQL =  "UPDATE Labels" +
                                    " SET " + differences[i].newValue.Key + " = '" + differences[i].newValue.Value + "'" +
                                    " WHERE " + DataGrid.Columns[DataGrid.DbTableUniqueIDColumnNumber].Label + " = " + differences[i].IDOfRowToBeUpdated + ";";
                sb.AppendLine(UpdateSQL);
            }
            return sb.ToString();
        }




        public void Save(IFormCollection fc)
        {
            string SQL = GetDifferences(fc);
            if (SQL.Length != 0)
            {
                using (SqlConnection con = new SqlConnection(DBUtil.GetSqlConnectionString()))
                {
                    con.Open();
                    DBUtil.RunSQLQuery(con, SQL);
                }
            }
            return;
        }

        private string GetDifferences(IFormCollection fc)
        {
            StringBuilder sb = new StringBuilder();
            for (int r = 0; r < DataGrid.RowCount; r++)
            {
                TableRow tr = DataGrid.GetRow(r);
                for (int c = 0; c < tr.Cells.Count; c++)
                {
                    if (!DataGrid.Columns[c].ReadOnly)
                    {
                        string HTMLId = tr.Cells[c].HTMLFieldID;
                        StringValues NewValue = new StringValues();
                        if (fc.TryGetValue(HTMLId, out NewValue))
                        {
                            if (tr.Cells[c].GetValue() == NewValue.ToString())
                                continue;
                            else
                            {   //  Zmena hodnoty. TODO
                                string UpdateSQL = "UPDATE Labels" +
                                            " SET " + DataGrid.Columns[c].Label + " = '" + NewValue + "'" +
                                            " WHERE LabelID = " + tr.Cells[0].GetValue() +
                                            " AND LanguageID = '" + tr.Cells[1].GetValue() + "';";
                                sb.AppendLine(UpdateSQL);
                            }

                        }
                        else
                        {

                        }
                    }
                }
            }
            return sb.ToString();
        }
    }

}
