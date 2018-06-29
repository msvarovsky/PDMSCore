using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.BusinessObjects
{
    public class PanelInfoRow
    {
        public int PanelID;
        public string FieldType, Label;

        public PanelInfoRow(int ID, string ft, string l)
        {
            PanelID = ID;
            FieldType = ft;
            Label = l;
        }

        public string Get(int ID, string ft)
        {
            return null;

        }

    }
    public class Panels
    {


        public void ProcessPanelsInfo(DataTable dt)
        {
            object obj;
            int PanelID;
            string PanelFieldType, Label;

            List<PanelInfoRow> l = new List<PanelInfoRow>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj = dt.Rows[i].ItemArray[0];
                if (obj == null)
                    continue;
                PanelID = Int32.Parse(obj.ToString());

                obj = dt.Rows[i].ItemArray[1];
                if (obj == null)
                    continue;
                PanelFieldType = obj.ToString();

                obj = dt.Rows[i].ItemArray[2];
                if (obj == null)
                    continue;
                Label = obj.ToString();

                l.Add(new PanelInfoRow(PanelID, PanelFieldType, Label));
            }





            //if (PanelExists(PanelID) == null)
            //{
            //    Panel p = new Panel(PanelID,)
            //        PanelList.Add(new Panel())
            //}

        }

        private Panel PanelExists(int PanelID)
        {
            for (int i = 0; i < PanelList.Count; i++)
            {
                if (PanelList[i].id == PanelID)
                    return PanelList[i];
            }
            return null;
        }
    }
}
