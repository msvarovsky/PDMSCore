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
            Panel np = new Panel(PanelID);
            PanelList.Add(np);
            return np;
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

        private string GetString(DataTable dt, int row, int column)
        {
            if (dt.Rows[row].ItemArray[column] == null)
                return "";
            return dt.Rows[row].ItemArray[column].ToString().Trim();
        }
        private int GetInt(DataTable dt, int row, int column)
        {
            int? FieldID = dt.Rows[row].ItemArray[0] == null ? (int?)null : (int)dt.Rows[i].ItemArray[0];

            if (dt.Rows[row].ItemArray[column] == null)
                return -1;
            return (int)dt.Rows[row].ItemArray[column];
        }

        public void ProcessFieldsInfo(DataTable dt)
        {
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                Field f = null;

                int PageID = GetInt(dt, r, 0);
                int PanelID = GetInt(dt, r, 1);
                int FieldID = GetInt(dt, r, 2);
                string FieldType = GetString(dt, r, 3);
                string Label = GetString(dt, r, 4);
                string StringValue = GetString(dt, r, 5);

                // ZDE JSEM SKONCIL

                switch (FieldType)
                {
                    case "tx":
                        f = new LabelTextBoxField((int)FieldID, Label, StringValue);
                        break;

                    case "rb":
                        f = new LabelRBCBControl<LabelRadioButtonField>(FieldID.ToString(), Label);
                        ((LabelRBCBControl<LabelRadioButtonField>)f).OtherRef = (sdr.IsDBNull(7) ? -1 : sdr.GetInt32(7));
                        ((LabelRBCBControl<LabelRadioButtonField>)f).SelectedValues = StringValue.Split(',');
                        break;

                    case "cb":
                        f = new LabelRBCBControl<LabelCheckBoxField>(FieldID.ToString(), Label);
                        ((LabelRBCBControl<LabelCheckBoxField>)f).OtherRef = (sdr.IsDBNull(7) ? -1 : sdr.GetInt32(7));
                        ((LabelRBCBControl<LabelCheckBoxField>)f).SelectedValues = StringValue.Split(',');
                        break;

                    case "rb-item":
                    case "cb-item":
                        TempMultiSelectItem tmsi = new TempMultiSelectItem();
                        tmsi.StringValue = (sdr.IsDBNull(3) ? "" : sdr.GetString(3).Trim());
                        tmsi.OtherRef = sdr.GetInt32(7);
                        tmsi.MultiSelectItemID = sdr.GetInt32(8);
                        AllMultiSelectItem.Add(tmsi);
                        break;

                    default:
                        break;
                }
                if (f != null)
                    ret.Add(f);
            }

        }
    }
}
