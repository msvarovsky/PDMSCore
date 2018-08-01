using Microsoft.AspNetCore.Http;
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
            int PanelID;
            string PanelLabel, PanelDecription;

            //Title = DBUtil.GetString(dt.Rows[0], 1);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PanelID = DBUtil.GetInt(dt.Rows[i], 0);
                PanelLabel = DBUtil.GetString(dt.Rows[i], 1);
                PanelDecription = DBUtil.GetString(dt.Rows[i], 2);

                GetPanel(PanelID).Label = PanelLabel;
                GetPanel(PanelID).Desc = PanelDecription;
            }
        }

        public void ProcessFieldsInfo(DataTable dt)
        {
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                DataRow dr = dt.Rows[r];
                int PanelID = DBUtil.GetInt(dr, 0);

                Panel p = GetPanel(PanelID);
                if (p == null)
                    continue;
                else
                    p.ProcessFields(dr, 1);
            }
            for (int i = 0; i < PanelList.Count; i++)
                PanelList[i].AssignMultiSelectItemsToControls();
        }

        public void SavePanels(IFormCollection fc)
        {
            for (int i = 0; i < PanelList.Count; i++)
            {
                PanelList[i].Save(fc);
            }

        }
    }
}
