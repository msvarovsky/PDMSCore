﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using PDMSCore.BusinessObjects;

namespace PDMSCore.DataManipulation
{
    public class Panel
    {
        public int PanelID { get; set; }
        public string PanelIDString { get { return "P" + PanelID.ToString(); } }
        public string Size { get; set; }
        public string Label { get; set; }
        public string Desc { get; set; }
        public List<Field> Fields { get; set; }
        public PanelMenu menu { get; set; }

        private List<TempMultiSelectItem> AllMultiSelectItem;

        public Panel(int id, string Label, int xSize)
        {
            Init();

            this.PanelID = id;
            this.Label = Label;

            xSize = (xSize > 2) ? 2 : xSize;
            xSize = (xSize < 1) ? 1 : xSize;
            this.Size = "x" + xSize;
        }
        public Panel(int PanelID)
        {
            Init();
            this.PanelID = PanelID;
        }
        private void Init()
        {
            AllMultiSelectItem = new List<TempMultiSelectItem>();
            this.Fields = new List<Field>();
            menu = new PanelMenu(PanelID);
        }

        public void AddParam(string FieldType, string value)
        {
            switch (FieldType)
            {
                case "Title":
                    Label = value;
                    break;

                case "Desc":
                    Desc = value;
                    break;

                default:
                    break;
            }
        }

        //public void ProcessFields2(DataRow dr, int col)
        public void ProcessFields2( int FieldID, int FieldLabelID, string FieldLabel, string FieldType, int PredecessorFieldID, int ParentFieldID,
                                    string StringValue, string FileValue, string OtherRef, string SelectedItemsIDs)
        {
            try
            {
                Field f = null;

                switch (FieldType)
                {
                    case "tb":
                        f = new LabelTextBoxField(PanelIDString, FieldID, FieldLabel, StringValue);
                        break;
                    case "rb":
                        f = new LabelRadioButtonFields(PanelIDString, FieldID.ToString(), FieldLabel);
                        ((LabelRadioButtonFields)f).RadioButtons.SetSelectedItems(SelectedItemsIDs);
                        break;
                    case "cb":
                        f = new LabelCheckBoxFields(PanelIDString, FieldID.ToString(), FieldLabel);
                        ((LabelCheckBoxFields)f).CheckBoxes.SetSelectedItems(SelectedItemsIDs);
                        break;
                    case "ddlb":
                        f = new LabelDropDownListBox(PanelIDString, FieldID.ToString(), FieldLabel);
                        ((LabelDropDownListBox)f).DropDown.SetSelectedItems(SelectedItemsIDs);
                        break;
                    case "rb-item":
                    case "cb-item":
                    case "ddlb-item":
                        TempMultiSelectItem tmsi = new TempMultiSelectItem();
                        tmsi.StringValue = FieldLabel;
                        //tmsi.OtherRef = DBUtil.GetInt(dr, col + 8);
                        tmsi.ParentFieldID = ParentFieldID.ToString();
                        tmsi.MultiSelectItemID = FieldID.ToString();

                        AllMultiSelectItem.Add(tmsi);
                        break;

                    default:
                        break;
                }
                if (f != null)
                {
                    Fields.Add(f);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
            }

        }

        public void ProcessFields(DataRow dr, int col)
        {
            try
            {
                Field f = null;

                int FieldID = DBUtil.GetInt(dr, col);
                string Label = DBUtil.GetString(dr, col + 1);
                int PredecessorFieldID = DBUtil.GetInt(dr, col + 2);
                string FieldType = DBUtil.GetString(dr, col + 3);

                string StringValue = DBUtil.GetString(dr, col + 5);

                switch (FieldType)
                {
                    case "tb":
                        f = new LabelTextBoxField(PanelIDString, FieldID, Label, StringValue);
                        break;
                    case "rb":
                        //f = new LabelRBCBControl<LabelRadioButtonField>(FieldID.ToString(), Label);
                        //((LabelRBCBControl<LabelRadioButtonField>)f).OtherRef = DBUtil.GetInt(dr, col+8);
                        //((LabelRBCBControl<LabelRadioButtonField>)f).SelectedValues = StringValue.Split(',');

                        //f = new LabelRBCBControl<RadioButtonField>(FieldID.ToString(), Label);
                        //((LabelRBCBControl<RadioButtonField>)f).RBCBControl.items.OtherRef = DBUtil.GetInt(dr, col + 8);
                        //((LabelRBCBControl<RadioButtonField>)f).RBCBControl.items.SelectedValues = StringValue.Split(',');

                        f = new LabelRadioButtonFields(PanelIDString, FieldID.ToString(), Label);
                        ((LabelRadioButtonFields)f).RadioButtons.SetSelectedItems(DBUtil.GetString(dr, col + 8));

                        break;
                    case "cb":
                        //f = new LabelRBCBControl<LabelCheckBoxField>(FieldID.ToString(), Label);
                        //((LabelRBCBControl<LabelCheckBoxField>)f).OtherRef = DBUtil.GetInt(dr, col + 8);
                        //((LabelRBCBControl<LabelCheckBoxField>)f).SelectedValues = StringValue.Split(',');

                        //f = new LabelRBCBControl<CheckBoxField>(FieldID.ToString(), Label);
                        //((LabelRBCBControl<CheckBoxField>)f).RBCBControl.items.OtherRef = DBUtil.GetInt(dr, col + 8);
                        //((LabelRBCBControl<CheckBoxField>)f).RBCBControl.items.SelectedValues = StringValue.Split(',');

                        f = new LabelCheckBoxFields(PanelIDString, FieldID.ToString(), Label);
                        ((LabelCheckBoxFields)f).CheckBoxes.SetSelectedItems(DBUtil.GetString(dr, col + 8));

                        break;
                    case "ddlb":
                        //f = new LabelDropDownField(FieldID, Label);
                        //((LabelDropDownField)f).Dropdown.OtherRef = DBUtil.GetInt(dr, col + 8);
                        //((LabelDropDownField)f).Dropdown.SelectedValues = StringValue.Split(',');

                        f = new LabelDropDownListBox(PanelIDString, FieldID.ToString(), Label);
                        ((LabelDropDownListBox)f).DropDown.SetSelectedItems(DBUtil.GetString(dr, col + 8));
                        break;

                    case "rb-item":
                    case "cb-item":
                    case "ddlb-item":
                        TempMultiSelectItem tmsi = new TempMultiSelectItem();
                        tmsi.StringValue = Label;
                        //tmsi.OtherRef = DBUtil.GetInt(dr, col + 8);
                        tmsi.ParentFieldID = DBUtil.GetString(dr, col + 4);
                        tmsi.MultiSelectItemID = FieldID.ToString();

                        AllMultiSelectItem.Add(tmsi);
                        break;

                    default:
                        break;
                }
                if (f != null)
                {
                    Fields.Add(f);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());

            }

        }

        private string LoadPanelInfo(SqlConnection con, int CompanyID, string LanguageID)
        {
            List<Field> ret = new List<Field>();
            SqlCommand sql = new SqlCommand("GetPanelFromID", con);
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add(new SqlParameter("PanelID", this.PanelID));
            sql.Parameters.Add(new SqlParameter("CompanyID", CompanyID));
            sql.Parameters.Add(new SqlParameter("LanguageID", LanguageID));

            try
            {
                using (SqlDataReader sdr = sql.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        return sdr.GetString(2).Trim();
                    }
                }
            }
            catch (Exception)
            {
                return "Exception in LoadPanelInfo(CompanyID = " + CompanyID + ", Lang = " + LanguageID + ")";
            }
            return "Something went wrong at LoadPanelInfo(CompanyID = " + CompanyID + ", Lang = " + LanguageID + ")";
        }

        //private List<Field> LoadPanelContent(SqlConnection con, int CompanyID, string LanguageID)
        //{
        //    List<TempMultiSelectItem> AllMultiSelectItem = new List<TempMultiSelectItem>();
        //    List<Field> ret = new List<Field>();
        //    SqlCommand sql = new SqlCommand("GetPanelFields", con);
        //    sql.CommandType = CommandType.StoredProcedure;
        //    sql.Parameters.Add(new SqlParameter("PanelID", this.id));
        //    sql.Parameters.Add(new SqlParameter("CompanyID", CompanyID));
        //    sql.Parameters.Add(new SqlParameter("LanguageID", LanguageID));

        //    try
        //    {
        //        using (SqlDataReader sdr = sql.ExecuteReader())
        //        {
        //            while (sdr.Read())
        //            {
        //                Field f = null;

        //                int?   FieldID = sdr.IsDBNull(0) ? (int?)null : sdr.GetInt32(0);
        //                string FieldType = sdr.GetString(1).Trim();
        //                string Label = sdr.GetString(2).Trim();
        //                string StringValue = (sdr.IsDBNull(3) ? "" : sdr.GetString(3).Trim());

        //                switch (FieldType)
        //                {
        //                    case "tx":
        //                        f = new LabelTextBoxField((int)FieldID, Label, StringValue);
        //                        break;

        //                    case "rb":
        //                        f = new LabelRBCBControl<LabelRadioButtonField>(FieldID.ToString(), Label);
        //                        ((LabelRBCBControl<LabelRadioButtonField>)f).OtherRef = (sdr.IsDBNull(7) ? -1 : sdr.GetInt32(7));
        //                        ((LabelRBCBControl<LabelRadioButtonField>)f).SelectedValues = StringValue.Split(',');
        //                        break;

        //                    case "cb":
        //                        f = new LabelRBCBControl<LabelCheckBoxField>(FieldID.ToString(), Label);
        //                        ((LabelRBCBControl<LabelCheckBoxField>)f).OtherRef = (sdr.IsDBNull(7) ? -1 : sdr.GetInt32(7));
        //                        ((LabelRBCBControl<LabelCheckBoxField>)f).SelectedValues = StringValue.Split(',');
        //                        break;

        //                    case "rb-item":
        //                    case "cb-item":
        //                        TempMultiSelectItem tmsi = new TempMultiSelectItem();
        //                        tmsi.StringValue = (sdr.IsDBNull(3) ? "" : sdr.GetString(3).Trim());
        //                        tmsi.OtherRef = sdr.GetInt32(7);
        //                        tmsi.MultiSelectItemID = sdr.GetInt32(8);
        //                        AllMultiSelectItem.Add(tmsi);
        //                        break;

        //                    default:
        //                        break;
        //                }
        //                if (f != null)
        //                    ret.Add(f);
        //            }
        //        }
        //    }
        //    catch (Exception eee)
        //    {
        //        ret.Add(new LabelTextAreaField(1,"Exception in LoadPanelContent(..)",eee.ToString()));
        //    }

        //    AssignMultiSelectItemsToControls();


        //    return ret;
        //}

        public string GetSaveSQL(IFormCollection fc, FieldValueUpdateInfo UpdateInfo)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Fields.Count; i++)
            {
                List<Field> DBFields = Fields[i].GetDBFields();

                for (int f = 0; f < DBFields.Count; f++)
                {
                    string UpdateString = null;
                    StringValues NewValue = new StringValues();
                    if (fc.TryGetValue(DBFields[f].HTMLFieldID, out NewValue))
                        UpdateString = DBFields[f].CreateDBUpdateStringIfNecessary(NewValue, UpdateInfo);
                    else
                        UpdateString = DBFields[f].GetResetDBUpdateString(UpdateInfo);         //  Update DB value to default val.

                    if (UpdateString != null)
                        sb.AppendLine(UpdateString);
                }
            }
            return sb.ToString();
        }

        public void AddFields(Field newField)
        {
            this.Fields.Add(newField);
        }

        public void GenerateRandomPanelMenuItems(int count)
        {
            string PanelID = this.PanelID.ToString();

            menu.items.Add(new PanelMenuItem(true, "Refresh", "refresh", PanelID));
            menu.items.Add(new PanelMenuItem(true, "Save", "save", PanelID));

            for (int i = 2; i < count; i++)
                menu.items.Add(new PanelMenuItem(false, i.ToString(), "www" + i + ".cz", PanelID));
        }

        /// <summary>
        /// Priradi vsechny sub-items (v AllMultiSelectItem) tem spravnym fields.
        /// Jak: Prochazi jednotlive Fields a (pokud se shoduji typy) vola na nich AddRelevantItems(sub-items).
        /// </summary>
        public void AssignMultiSelectItemsToControls()
        {
            for (int i = 0; i < Fields.Count; i++)
            {
                if (Fields[i] == null)
                    continue;
                //if (Fields[i].GetType() == typeof(LabelRBCBControl<LabelRadioButtonField>))
                //    ((LabelRBCBControl<LabelRadioButtonField>)Fields[i]).AddRelevantItems(AllMultiSelectItem);

                //else if (Fields[i].GetType() == typeof(LabelRBCBControl<LabelCheckBoxField>))
                //    ((LabelRBCBControl<LabelCheckBoxField>)Fields[i]).AddRelevantItems(AllMultiSelectItem);

                if (Fields[i].GetType() == typeof(LabelRadioButtonFields))
                    ((LabelRadioButtonFields)Fields[i]).RadioButtons.AddRelevantItems(AllMultiSelectItem);

                else if (Fields[i].GetType() == typeof(LabelCheckBoxFields))
                    ((LabelCheckBoxFields)Fields[i]).CheckBoxes.AddRelevantItems(AllMultiSelectItem);

                else if (Fields[i].GetType() == typeof(LabelDropDownListBox))
                    ((LabelDropDownListBox)Fields[i]).DropDown.AddRelevantItems(AllMultiSelectItem);

            }
        }
    }

    public struct TempMultiSelectItem
    {
        public string StringValue;
        //public int OtherRef;
        public string ParentFieldID;
        public string MultiSelectItemID;
    }

    public class PanelMenuItem
    {/*     <a href="#">Link 1-</a>
                <a href="#">Link 3-</a>     */

        public bool IsActionAsync { get; set; }
        public string ItemText { get; set; }
        public string ItemLinkOrAsyncAction { get; set; }
        private string PanelMenuId { get; set; }

        public PanelMenuItem(bool Async, string Text, string LinkOrAsyncAction, string PanelMenuId)
        {
            IsActionAsync = Async;
            ItemText = Text;
            ItemLinkOrAsyncAction = LinkOrAsyncAction;
            this.PanelMenuId = PanelMenuId;
        }

        public TagBuilder HtmlText(string PanelOwnerID, string PanelID)
        {
            TagBuilder tb = new TagBuilder("a");
            if (IsActionAsync)
                //tb.Attributes.Add("onclick", "onPanelMenuItemClick('" + PanelID + "','"+ ItemLinkOrAsyncAction + "')");
                tb.Attributes.Add("onclick", "onPanelMenuItemClick('" + PanelOwnerID + "','" + PanelID + "','" + ItemLinkOrAsyncAction + "')");
            else
                tb.Attributes.Add("href", ItemLinkOrAsyncAction);

            tb.InnerHtml.AppendHtml(ItemText);
            return tb;
        }
    }

    public class PanelMenu
    {   
        /*  <div class="dd-menu">
                <i class="fa fa-gear dd-menu-btn" onclick="onPanelMenuClick()" aria-hidden="true"></i>
                <div id="myDropdown" class="dd-menu-content">
                </div>
            </div>          */

        public List<PanelMenuItem> items { get; set; }
        private string PanelId { get; set; }

        public PanelMenu(int id)
        {
            //PanelMenuId = "PanelMenu" + id;
            PanelId = id.ToString();

            items = new List<PanelMenuItem>();
        }

        public void AddMenuItem(PanelMenuItem pmi)
        {
            items.Add(pmi);
        }

        public TagBuilder HtmlText(object PanelOwner)
        {
            string PanelOwnerID = (string)PanelOwner;
            TagBuilder tbDiv = new TagBuilder("div");
            tbDiv.AddCssClass("dd-menu");

            TagBuilder tbI = new TagBuilder("i");
            tbI.AddCssClass("fa fa-gear dd-menu-btn");
            tbI.Attributes.Add("onclick", "onPanelMenuClick('" + HttpUtility.JavaScriptStringEncode(PanelId) + "')");
            tbI.Attributes.Add("aria-hidden", "true");
            tbI.RenderSelfClosingTag();

            TagBuilder tbDivContent = new TagBuilder("div");
            tbDivContent.Attributes.Add("id", PanelId);
            tbDivContent.AddCssClass("dd-menu-content");

            for (int i = 0; i < items.Count; i++)
            {
                tbDivContent.InnerHtml.AppendHtml(items[i].HtmlText(PanelOwnerID, PanelId));
            }

            tbDiv.InnerHtml.AppendHtml(tbI);
            tbDiv.InnerHtml.AppendHtml(tbDivContent);

            return tbDiv;
        }
    }
}