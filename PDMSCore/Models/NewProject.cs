using PDMSCore.BusinessObjects;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.Models
{
    public class NewProject
    {
        public Page page;
        //        ret, nav, lang
        //exec GetPageContent 2, 6, 'en'

        public NewProject LoadNewPageFromDB(int RetailerID = -1, string NavID = "-1", string LanguageID = "en")
        {
            using (SqlConnection con = new SqlConnection(DBUtil.GetSqlConnectionString()))
            {
                con.Open();
                DataTable table = new DataTable();
                SqlDataAdapter sqlDataAdapter;

                List<Field> ret = new List<Field>();
                SqlCommand sql = new SqlCommand("GetPageContent", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add(new SqlParameter("RetailerID", RetailerID));
                sql.Parameters.Add(new SqlParameter("NavID", NavID));
                sql.Parameters.Add(new SqlParameter("LanguageID", LanguageID));


                //sqlDataAdapter = new SqlDataAdapter(sql);
                //sqlDataAdapter.Fill(dataSet);
                //if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                //    ProcessNavigation(dataSet.Tables[0]);
                //else
                //{
                //    //Page.GenerateUnknownPageInfo();
                //    Console.WriteLine("No matching records found.");
                //}


                try
                {
                    sqlDataAdapter = new SqlDataAdapter(sql);
                    sqlDataAdapter.Fill(table);
                    ProcessPage(table);
                }
                catch (Exception eee)
                {
                    ret.Add(new LabelTextAreaField("1", "Exception in LoadNewPageFromDB(..)", eee.ToString()));
                }
            }
            return this;
        }

        private void ProcessPage(DataTable dt)
        {
            page = new Page();
            page.ProcessPage(dt);
            return;
        }


    }
}
