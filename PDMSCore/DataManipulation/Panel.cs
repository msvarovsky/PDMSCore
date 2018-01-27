using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace PDMSCore.DataManipulation
{
    public class Panel
    {
        public int id { get; set; }
        public string Size { get; set; }
        public string Label { get; set; }
        public List<Field> Content { get; set; }

        public Panel(int id, string Label, int xSize)
        {
            this.id = id;
            this.Label = Label;

            xSize = (xSize > 2) ? 2 : xSize;
            xSize = (xSize < 1) ? 1 : xSize;
            this.Size = "x" + xSize;

            this.Content = new List<Field>();
        }

        //private bool AuthUser()
        //{
        //    bool ret = false;
        //    using (SqlConnection con = new SqlConnection(
        //                                   "user id=Martin;" +
        //                                   "password=6835001;" +
        //                                   "server=192.168.2.101;" +
        //                                   "database=test; " +
        //                                   "connection timeout=10"
        //                                   ))
        //    {
        //        SqlCommand sql = new SqlCommand("AuthUser", con);
        //        sql.CommandType = CommandType.StoredProcedure;
        //        sql.Parameters.Add(new SqlParameter("@username", tbName.Text));
        //        sql.Parameters.Add(new SqlParameter("@pass", tbPass.Text));
        //        try
        //        {
        //            con.Open();
        //            int r = (int)sql.ExecuteScalar();
        //            if (r == 0)
        //            {   //  Not OK
        //                ret = false;
        //            }
        //            else
        //            {   //  OK
        //                ret = true;
        //            }
        //        }
        //        catch (Exception eee)
        //        {
        //            eee = null;
        //        }
        //    }
        //    return ret;
        //}

        public bool Save(IFormCollection fc)
        {
            foreach (var FieldId in fc.Keys)
            {
                if (FieldId == "PanelId")
                    continue;

                string FieldValue = fc[FieldId];




            }
            return false;
        }

        public void AddFields(Field newField)
        {
            this.Content.Add(newField);
        }
    }
}