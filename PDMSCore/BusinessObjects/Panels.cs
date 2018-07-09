using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Data;


namespace PDMSCore.BusinessObjects
{
    public class Panels
    {
        public List<Panel> PanelList { get; set; }

        public Panels()
        {
            PanelList = new List<Panel>();
        }

        private Panel GetPanel(int PanelID)
        {
            for (int i = 0; i < PanelList.Count; i++)
            {
                if (PanelList[i].id == PanelID)
                    return PanelList[i];
            }
            if (PanelID == -1)
                return null;
            else
            {
                Panel np = new Panel(PanelID);
                PanelList.Add(np);
                return np;
            }
        }

        public void ProcessPanelsInfo(DataTable dt)
        {
            object obj;
            int PanelID;
            string PanelFieldType, Label;

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

                GetPanel(PanelID).AddParam(PanelFieldType, Label);
            }
        }

        public void ProcessFieldsInfo(DataTable dt)
        {
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                DataRow dr = dt.Rows[r];

                //int PageID = DBUtil.GetInt(dr, 0);
                int PanelID = DBUtil.GetInt(dr, 1);

                Panel p = GetPanel(PanelID);
                if (p == null)
                    continue;
                else
                    p.ProcessFields(dr, 2);
            }
            for (int i = 0; i < PanelList.Count; i++)
                PanelList[i].AssignMultiSelectItemsToControls();
        }
    }
}
