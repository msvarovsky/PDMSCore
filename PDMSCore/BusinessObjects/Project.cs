using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace PDMSCore.BusinessObjects
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Panel> ToShow { get; set; }

        public Project()
        {
            ToShow = new List<Panel>();
        }
        public bool Create()
        {
            // jdi do DB a vytvor novy projekt.
            Id = (Int32)DateTime.Now.Ticks;
            return true;
        }

        public bool SaveFromHtml(IFormCollection fc)
        {
            StringValues sv = new StringValues();

            if (!fc.TryGetValue("PanelId", out sv))
                return false;

            Panel p = GetPanel(sv.ToString());
            return p.Save(fc);
        }

        private Panel GetPanel(string id)
        {
            int iId;
            if (Int32.TryParse(id, out iId))
            {
                for (int i = 0; i < ToShow.Count; i++)
                {
                    if (ToShow[i].id == iId)
                        return ToShow[i];
                }
            }
            return null;
        }

        public void AddPanel(Panel newPanel)
        {
            ToShow.Add(newPanel);
        }

        public void GetFieldTemplate()
        {
            //GetRandom
        }

        //private bool AuthUser()
        //{
            
        //}

        //public static void GetProject(int ProjectID)
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

        public void GetRandom()
        {
            int id = 1;
            long randonName = DateTime.Now.Ticks;

            List<Field> fields = new List<Field>();

            //TextBoxField tb = new TextBoxField((randonName++).ToString(), "Pocet projektu", "5");
            
            fields.Add(new LabelField("Normalni text"));
            fields.Add(Field.NewLine());
            fields.Add(new LabelField("Bold text",true));
            fields.Add(Field.NewLine());

            fields.Add(LabelTextBoxField.GetRandom((id++).ToString()));
            fields.Add(LabelTextAreaField.GetRandom((id++).ToString()));
            fields.Add(Field.NewLine());

            fields.Add(LabelRBCBControl<LabelRadioButtonField>.GetRandom((id++).ToString(),3));
            fields.Add(LabelRBCBControl<LabelCheckBoxField>.GetRandom((id++).ToString(), 4));
            fields.Add(LabelDropDownField.GetRandom((id++).ToString(),4));
            fields.Add(LabelDatePickerField.GetRandom((id++).ToString()));

            fields.Add(LabelFileUploadField.GetRandom());
            fields.Add(LabelFileUploadField.GetRandom(true));

            fields.Add(LabelFileDownloadField.GetRandom());


            /*fields.Add(LabelCheckBoxesControl.GetRandom(2));
            fields.Add(LabelRadioButtonsControl.GetRandom(2));*/


            //fields.Add(new Field { Label = "Pocet projektu", tagName = 1, Type = FieldType.Indicator, Value = "5" });
            //fields.Add(new Field { Label = "Pocet otevrenych projektu", tagName = 2, Type = FieldType.Indicator, Value = "5" });

            Panel panel = new Panel(1,"GetRandom",1);
            panel.Content = fields;
            //ToShow.Add(panel);

            Panel panel2 = new Panel(2, "Grid", 1);
            List<Field> fields2 = new List<Field>();
            fields2.Add(GridViewField.GetRandom("Grid"));
            panel2.Content = fields2;


            ToShow.Add(panel2);
        }


    }
}