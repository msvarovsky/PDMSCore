using Microsoft.AspNetCore.Http;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
                if (PanelList[i].PanelID == PanelID)
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

                //GetPanel(PanelID).Label = PanelLabel;
                GetPanel(PanelID).Label = PanelLabel + DateTime.Now.ToLongTimeString();
                GetPanel(PanelID).Desc = PanelDecription;
            }
        }

        public void ProcessFieldsInfo2(DataTable dt)
        {   //  ...., FieldID, LabelID, Label, FieldType, PredecessorFieldID, ParentFieldID
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                DataRow dr = dt.Rows[r];
                int PanelID = DBUtil.GetInt(dr, 0);

                Panel p = GetPanel(PanelID);
                if (p == null)
                    continue;
                else
                {
                    int FieldID = DBUtil.GetInt(dr, 1);
                    int FieldLabelID = DBUtil.GetInt(dr, 2);
                    string FieldLabel = DBUtil.GetString(dr, 3);
                    string FieldType = DBUtil.GetString(dr, 4);
                    int PredecessorFieldID = DBUtil.GetInt(dr, 5);
                    int ParentFieldID= DBUtil.GetInt(dr, 6);

                    //string StringValue = DBUtil.GetString(dr, 6);
                    //string FileValue = DBUtil.GetString(dr, 7);
                    //string OtherRef = DBUtil.GetString(dr, 8);
                    //string SelectedItemsIDs= DBUtil.GetString(dr, 9);

                    string StringValue = "";
                    string FileValue = "";
                    string OtherRef = "";
                    string SelectedItemsIDs = "";


                    p.ProcessFields2(FieldID, FieldLabelID, FieldLabel, FieldType, PredecessorFieldID, ParentFieldID, StringValue, FileValue, OtherRef, SelectedItemsIDs);
                }
            }
            for (int i = 0; i < PanelList.Count; i++)
                PanelList[i].AssignMultiSelectItemsToControls();
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

        public void SavePanels(IFormCollection fc, FieldValueUpdateInfo UpdateInfo)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < PanelList.Count; i++)
            {
                string a = PanelList[i].GetSaveSQL(fc, UpdateInfo);
                if (a != null)
                    sb.Append(a);
            }
            if (sb.Length != 0)
                DBUtil.RunSQLQuery(UpdateInfo, sb.ToString().Trim());
        }
    }
}
