﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using PDMSCore.BusinessObjects;

namespace PDMSCore.DataManipulation
{
    public enum FieldType
    {
        Label,
        TextBoxNoLabel,
        TextBox,
        DropDownListBox,
        DatePicker,
        CheckBox,
        RadioButton,
        FileUpload,
        FileDownload,
        Indicator
    }

    public struct WebTagAttributes
    {
        public Boolean ReadOnly { get; private set; }
        public string CheckBoxGroupName { get; private set; }

        public WebTagAttributes(Boolean readOnly, string checkBoxGroupName)
        {
            ReadOnly = readOnly;
            CheckBoxGroupName = checkBoxGroupName;
        }
    }

    public interface IHtmlTag
    {
        TagBuilder HtmlText();
    }

    public interface IDBSaveable
    {
        string CreateDBUpdateStringIfNecessary(StringValues NewValue, FieldValueUpdateInfo UpdateInfo);
       
        string GetResetDBUpdateString(FieldValueUpdateInfo UpdateInfo);
    }
    public interface ILabelBeforeControl
    {
        List<Field> GetDBFields();
    }

    public interface IHtmlElement
    {
        TagBuilder BuildHtmlTag();
    }

    public class Field
    {
        public string NameAttribute { get; set; }
        public string DBFieldID { get; set; }
        public string HTMLFieldID { get; set; }
        public string HtmlTag { get; set; }
        public string VisibleText { get; set; }
        public string ValueAttribute { get; set; }
        public string TypeAttribute { get; set; }
        public string ParentID { get; set; }
        public string ParentDBID { get; set; }


        public Field(string ParentID, string DBFieldID, string HTMLFieldID, string HtmlTag, string VisibleText)
        {
            this.ParentID = ParentID;
            this.DBFieldID = DBFieldID;
            this.NameAttribute = HTMLFieldID;
            this.HTMLFieldID = HTMLFieldID;
            this.HtmlTag = HtmlTag;
            this.VisibleText = VisibleText;
        }

        public TagBuilder BuildBaseHtmlTag(string tag)
        {
            HtmlTag = tag;
            return BuildBaseHtmlTag();
        }
        public TagBuilder BuildBaseHtmlTag()
        {
            TagBuilder tb = null;
            if (HtmlTag != null)
            {
                tb = new TagBuilder(HtmlTag);
                if (NameAttribute != null)
                    tb.Attributes.Add("name", NameAttribute);

                if (HTMLFieldID != null)
                    tb.Attributes.Add("id", HTMLFieldID);
                    //tb.Attributes.Add("id", ParentID + "-" + HTMLFieldID);

                if (ValueAttribute != null)
                    tb.Attributes.Add("value", ValueAttribute);

                if (this.VisibleText != null)
                    tb.InnerHtml.AppendHtml(VisibleText);

                if (this.TypeAttribute != null)
                    tb.Attributes.Add("type", TypeAttribute);
            }
            return tb;
        }

        public string GetValue()
        {
            return VisibleText;
        }

        public virtual List<Field> GetDBFields()
        {
            throw new Exception("MS: Class derived from class 'Field' does not override method 'GetDBID'.");
        }
        public virtual string CreateDBUpdateStringIfNecessary(StringValues NewValue, FieldValueUpdateInfo con)
        {
            throw new Exception("MS: Class derived from class 'Field' does not override method 'Save'.");
        }
        public virtual string GetResetDBUpdateString(FieldValueUpdateInfo UpdateInfo)
        {
            if (TypeAttribute == "checkbox" || TypeAttribute == "radio")
            {
                string NewValue = "";

                UpdateInfo.FieldID = HTMLFieldID;
                string ret = "UPDATE FieldsValues" +
                    " SET StringValue = '" + NewValue + "'" +
                    " WHERE RetailerID = " + UpdateInfo.RetailerID +
                    " AND ProjectID = " + UpdateInfo.ProjectID +
                    " AND FieldID = " + UpdateInfo.FieldID;
                return ret + ";";

            }
            throw new Exception("MS: Class derived from class 'Field' does not override method 'GetResetDBUpdateString'.");
        }
    }

    public class NewLine : Field, IHtmlElement
    {
        public NewLine():base(null,null,null,"/br",null)
        {

        }
        public static Field GetRandom()
        {
            return null;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = new TagBuilder("/br");
            tb.TagRenderMode = TagRenderMode.SelfClosing;
            return tb;
        }


        //public override string GetValue()
        //{
        //    return "";
        //}

        //public override TagBuilder HtmlText()
        //{
        //    TagBuilder tb = new TagBuilder("/br");
        //    tb.TagRenderMode = TagRenderMode.SelfClosing;
        //    return tb;
        //}
    }
    public class HiddenField : Field, IHtmlElement
    {
        public string Value { get; set; }
        public HiddenField(string NameId):base(null,null,NameId,"input", null)
        {
            //this.NameId = NameId;
        }

        //public override TagBuilder HtmlText()
        //{
        //    TagBuilder tb = new TagBuilder("input");
        //    tb.Attributes.Add("type", "hidden");
        //    tb.Attributes.Add("name", NameId);
        //    tb.Attributes.Add("id", NameId);
        //    return tb;
        //}

        //public override string GetValue()
        //{
        //    return Value;
        //}

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.Attributes.Add("type", "hidden");
            return tb;
        }
    }

    public class LabelDataGridField : Field, IHtmlElement, ILabelBeforeControl
    {
        public LabelField label { get; set; }
        public DataGridField table { get; set; }

        public LabelDataGridField(string LabelText, DataGridField table):base("todo","","", "div", null)
        {
            label = new LabelField(LabelText, true);
            this.table = table;
        }

        //public override TagBuilder HtmlText()
        //{
        //    TagBuilder tb = new TagBuilder("div");
        //    tb.InnerHtml.AppendHtml(label.HtmlText());
        //    tb.InnerHtml.AppendHtml(table.HtmlText());
        //    return tb;
        //}

        public static Field GetRandom(string id)
        {
            LabelDataGridField n = new LabelDataGridField("Grid", DataGridField.GetRandom());
            return n;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.InnerHtml.AppendHtml(label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(table.BuildHtmlTag());
            return tb;
        }

        public override List<Field> GetDBFields()
        {
            return null;
        }
    }

    public class LabelField : Field, IHtmlElement
    {
        public string HtmlLabel { get; set; }
        public string For { get; set; }

        public string ToolTip { get; set; }
        public string PlaceHolder { get; set; }
        public bool Bold { get; set; }

        public LabelField(string HtmlLabel, bool Bold = false, string For = null):base(null, null, null, "label", null)
        {
            this.Bold = Bold;
            this.HtmlLabel = HtmlLabel;
            this.For = For;
        }

        public static Field GetRandom()
        {
            return new LabelField("LabelField");
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass(Bold ? "BoldLabelField" : "LabelField");
            if (this.For != null)
                tb.Attributes.Add("for", this.For);
            tb.InnerHtml.AppendHtml(HtmlLabel);
            return tb;
        }
    }

    public class TextAreaField : Field, IHtmlElement, IDBSaveable
    {
        public int Rows { get; set; }
        public string Placeholder { get; set; }
        public string Text { get; set; }

        public TextAreaField(string PanelID, string NameId, string Text, string Placeholder, int Rows)
            :base(PanelID, NameId, NameId, "textarea", null)
        {
            this.Text = Text;
            this.Placeholder = Placeholder;
            this.Rows = Rows;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("ControlOfLabelControlDuo");
            tb.Attributes.Add("rows", Rows.ToString());
            tb.Attributes.Add("placeholder", Placeholder);
            tb.InnerHtml.AppendHtml(WebUtility.HtmlEncode(Text));
            return tb;
        }

    }
    public class LabelTextAreaField : Field, IHtmlElement, ILabelBeforeControl
    {   //  <textarea rows="4" cols="50" placeholder="Describe yourself here...">Value</textarea>
        public LabelField label { get; set; }
        public TextAreaField textArea { get; set; }

        public LabelTextAreaField(string PanelID, string Id, string LabelText, string Text="", string Placeholder = "...", int Rows = 4)
            :base(null, Id.ToString(), Id.ToString(),"div", null)
        {
            
            label = new LabelField(LabelText, true);
            textArea = new TextAreaField(PanelID, Id, Text, Placeholder, Rows);
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(textArea.BuildHtmlTag());
            return tb;
        }

        public static Field GetRandom(int id)
        {
            LabelTextAreaField a = new LabelTextAreaField("random", id.ToString(), "TextArea label", null, "holder", 4);
            return a;
        }

        public override List<Field> GetDBFields()
        {
            return new List<Field> { textArea };
        }
    }

    public class TextBoxField : Field, IHtmlElement, IDBSaveable
    {
        public string Text { get; set; }
        public string PlaceHolder { get; set; }
        public string ToolTip { get; set; }
        public string OnClick { get; set; }
        public bool ReadOnly { get; set; }
        private string CSS { get; set; }

        //public TextBoxField(string ParentID, string NameId, string CSS, string Text="", string PlaceHolder = "", string ToolTip="")
        public TextBoxField(string HTMLFieldID, string DBFieldID, string CSS, string Text = "", string PlaceHolder = "", string ToolTip = "")
            //: base(ParentID, DBFieldID, "f" + DBFieldID, "input", null)
            : base(null, DBFieldID, HTMLFieldID, "input", null)
        {
            this.Text = Text;
            this.PlaceHolder = PlaceHolder;
            this.ToolTip = ToolTip;
            this.CSS = CSS;
            this.ReadOnly = false;
            this.ParentID = ParentID;
            OnClick = "";
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();

            if (CSS == "") 
                tb.AddCssClass("ControlOfLabelControlDuo");
            else
                tb.AddCssClass(CSS);

            if (ReadOnly)
                tb.Attributes.Add("readonly","");

            if (OnClick!="")
                tb.Attributes.Add("onclick", OnClick);

            tb.Attributes.Add("type", "text");
            if (ToolTip != "") 
                tb.Attributes.Add("title", ToolTip);
            if (PlaceHolder != "")
                tb.Attributes.Add("placeholder", PlaceHolder);

            tb.Attributes.Add("value", WebUtility.HtmlEncode(Text));
            return tb;
        }

        public override string CreateDBUpdateStringIfNecessary(StringValues NewValue, FieldValueUpdateInfo UpdateInfo)
        {
            if (NewValue != Text)
            {
                UpdateInfo.FieldID = HTMLFieldID;
                string ret = "UPDATE FieldsValues" + 
                    " SET StringValue = '" + NewValue + "'" +
                    " WHERE RetailerID = " + UpdateInfo.RetailerID +
                    " AND ProjectID = " + UpdateInfo.ProjectID +
                    " AND FieldID = " + UpdateInfo.FieldID;
                return ret + ";";
            }
            return null;
        }
        public override string GetResetDBUpdateString(FieldValueUpdateInfo UpdateInfo)
        {
            string NewValue = "";

            UpdateInfo.FieldID = HTMLFieldID;
            string ret = "UPDATE FieldsValues" +
                " SET StringValue = '" + NewValue + "'" +
                " WHERE RetailerID = " + UpdateInfo.RetailerID +
                " AND ProjectID = " + UpdateInfo.ProjectID +
                " AND FieldID = " + UpdateInfo.FieldID;
            return ret + ";";
        }

    }
    public class LabelTextBoxField : Field, IHtmlElement, ILabelBeforeControl
    {
        private LabelField label;
        private TextBoxField txtb;

        public LabelTextBoxField(string ParentID, string DBFieldID, string LabelText, string Text, string Placeholder = "...", string ToolTip = "") : base(null, null, null, "div", null)
        {
            Init(ParentID, DBFieldID, LabelText, Text, Placeholder, ToolTip);
        }
        public LabelTextBoxField(string ParentID, int DBFieldID, string LabelText, string Text, string Placeholder="...", string ToolTip="") : base(null, null, null, "div", null)
        {
            Init(ParentID, DBFieldID.ToString(), LabelText, Text, Placeholder, ToolTip);
        }
        private void Init(string ParentID, string DBFieldID, string LabelText, string Text, string Placeholder, string ToolTip)
        {
            label = new LabelField(LabelText, true);
            txtb = new TextBoxField(ParentID + "-f" + DBFieldID.ToString(), DBFieldID.ToString(), "ControlOfLabelControlDuo", Text, Placeholder, ToolTip);
        }

        public static Field GetRandom(int Id)
        {
            LabelTextBoxField a = new LabelTextBoxField(Id.ToString(),Id+":labelText","","placeholder","tooltip");
            return a;
        }

        //public override string GetValue()
        //{
        //    return txtb.GetValue();
        //}

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(txtb.BuildHtmlTag());
            return tb;
        }

        public override List<Field> GetDBFields()
        {
            return new List<Field> { txtb };
        }
    }

    public class LabelSelectableTextBoxField : Field, IHtmlElement
    {
        private LabelField label;
        private TextBoxField txtb;
        private HiddenField hiddenField;
        public string DataGridSP;

        public LabelSelectableTextBoxField(int Id, string LabelText, string Text, string dataGridSP):base(null, Id.ToString(), Id.ToString(),"div", null)
        {
            label = new LabelField(LabelText, true);
            txtb = new TextBoxField(Id.ToString(), "ControlOfLabelControlDuo", Text);

            //function OpenModal(dialogID,      tagIDOfReturnedID,  tagIDOfReturnedLabel)
                   //  OpenModal('myModal1',    '74100',            'SelectedIDOf74100')
            txtb.OnClick = "OpenModal(\'ErarniModal\',\'SelectedIDOf" + Id + "\', \'" + Id + "\',\'ModalDataGrid\',\'sp_Users\')";
            txtb.ReadOnly = true;
            hiddenField = new HiddenField("SelectedIDOf" + Id);

            DataGridSP = dataGridSP;
        }

        //public override string GetValue()
        //{
        //    return txtb.GetValue();
        //}

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(txtb.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(hiddenField.BuildHtmlTag());
            return tb;
        }

    }

    public class FileUploadField : Field, IHtmlElement
    {
        public string Name { get; set; }
        public string ToolTip { get; set; }
        public bool MultipleFile { get; set; }

        public FileUploadField(string ParentID, string NameId, bool MultipleFile = false, string ToolTip = "")
            :base(ParentID, NameId, NameId, "input", null)
        {
            this.Name = NameId;
            this.ToolTip = ToolTip;
            this.MultipleFile = MultipleFile;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("ControlOfLabelControlDuo");
            tb.Attributes.Add("name", this.Name);
            tb.Attributes.Add("type", "file");
            tb.Attributes.Add("title", ToolTip);
            if (MultipleFile)
                tb.Attributes.Add("multiple", "");
            return tb;
        }

        //public override string GetValue()
        //{
        //    return Name;
        //}
    }
    public class LabelFileUploadField : Field, IHtmlElement
    {
        private LabelField label;
        private FileUploadField fu;

        public LabelFileUploadField(string ParentID, string HtmlText, FileUploadField f):base(null,null, null, "div", null)
        {
            label = new LabelField(HtmlText,true);
            fu = f;
        }

        public static Field GetRandom(bool MultipleUpload=false)
        {
            LabelFileUploadField a = new LabelFileUploadField("random", "fu",new FileUploadField("random","fuu", MultipleUpload,"Tooltip..."));
            return a;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(fu.BuildHtmlTag());
            return tb;
        }

        //public override string GetValue()
        //{
        //    return "";
        //}
    }

    public class FileDownloadField : Field, IHtmlElement
    {
        public string Name { get; set; }
        public string ToolTip { get; set; }
        public string Path { get; set; }

        public FileDownloadField(string ParentID, string NameId, string Path, string ToolTip = ""): base(ParentID, NameId, NameId, "a", null)
        {
            this.Name = NameId;
            this.ToolTip = ToolTip;
            this.Path = Path;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("ControlOfLabelControlDuo");
            tb.Attributes.Add("href", this.Path);
            tb.Attributes.Add("download","");
            if (ToolTip=="")
                tb.Attributes.Add("title", this.ToolTip);

            string aa = System.IO.Path.GetFileName(this.Path);

            tb.InnerHtml.AppendHtml(aa);
            return tb;
        }


        //public override string GetValue()
        //{
        //    return Name;
        //}
    }
    public class LabelFileDownloadField : Field, IHtmlElement
    {
        private LabelField label;
        private FileDownloadField fu;

        public LabelFileDownloadField(string HtmlText, FileDownloadField f):base(null, null, null, "div", null)
        {
            label = new LabelField(HtmlText, true);
            fu = f;
        }

        public static Field GetRandom(bool MultipleUpload = false)
        {
            LabelFileDownloadField a = new LabelFileDownloadField("fu", new FileDownloadField("fuu", "/images/banner1.svg", "Tooltip..."));
            return a;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(fu.BuildHtmlTag());
            return tb;
        }

        //public override string GetValue()
        //{
        //    return fu.GetValue();
        //}
    }

    public class DatePickerField : Field, IHtmlElement
    {
        //  <input type="date" name="bday" value="2013-01-08"> disabled>
        public DateTime? Value { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool Enabled { get; set; }

        public DatePickerField(string ParentID, string Id, DateTime? Value = null, DateTime? MinDate = null, DateTime? MaxDate = null, bool Enabled = true):
            base(ParentID, Id, Id, "input", null)
        {
            this.Value = Value;
            this.MinDate = MinDate;
            this.MaxDate = MaxDate;
            this.Enabled = Enabled;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("ControlOfLabelControlDuo");
            tb.Attributes.Add("type", "date");

            if (this.Value != null)
                tb.Attributes.Add("value", this.Value.Value.ToString("yyyy-MM-dd"));
            if (this.MinDate != null)
                tb.Attributes.Add("min", this.MinDate.Value.ToString("yyyy-MM-dd"));
            if (this.MaxDate!= null)
                tb.Attributes.Add("max", this.MaxDate.Value.ToString("yyyy-MM-dd"));

            if (!Enabled)
                tb.Attributes.Add("disabled", "");
            return tb;
        }

        //public override string GetValue()
        //{
        //    return Value.Value.ToShortDateString();
        //}
    }
    public class LabelDatePickerField : Field, IHtmlElement
    {   //    <input type="date" name="bday">
        private LabelField Label;
        private DatePickerField DatePicker;

        public LabelDatePickerField(string ParentID, int Id, string HtmlLabel, DateTime? Value = null, DateTime? MinDate = null, DateTime? MaxDate = null)
            :base(null, Id.ToString(), Id.ToString(), "div", null)
        {
            this.Label = new LabelField(HtmlLabel, true);
            this.DatePicker = new DatePickerField(ParentID, Id.ToString(), Value, MinDate, MaxDate);
        }

        public static Field GetRandom(int Id, DateTime? DefaultValue = null)
        {
            DefaultValue = (DefaultValue == null ? DateTime.Now : DefaultValue);
            LabelDatePickerField a = new LabelDatePickerField("random", Id, "DP Label",DefaultValue );
            return a;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(DatePicker.BuildHtmlTag());
            return tb;
        }

        //public override string GetValue()
        //{
        //    return DatePicker.GetValue();
        //}
    }

    //public class DropDownOption : Field, IHtmlElement
    //{
    //    //public string Value { get; set; }
    //    public string Label { get; set; }
    //    public bool Enabled { get; set; }

    //    public DropDownOption(string ValueId, string Label, bool Enabled = true): base(ValueId,"option", Label)
    //    {
    //        //this.NameId ="DD" + ValueId.ToString();
    //        this.Label = Label;
    //        this.Enabled = Enabled;
    //    }

    //    public DropDownOption(string Name, string Label):base(Name,"option", Label)
    //    {
    //        this.Label = Label;
    //        this.Enabled = true;
    //    }

    //    public TagBuilder BuildHtmlTag()
    //    {   //  <option label="England" value="England"></option> 
    //        TagBuilder tb = base.BuildBaseHtmlTag();
    //        //tb.Attributes.Add("value", NameId);
    //        tb.Attributes.Add("value", "TODO");
    //        if (!Enabled)
    //            tb.Attributes.Add("disabled", "");
    //        tb.InnerHtml.AppendHtml(Label);
    //        return tb;
    //    }

    //    //public override string GetValue()
    //    //{
    //    //    return Label;
    //    //}
    //}
    //public class DropDownField : MultiOption, IHtmlElement
    //{
    //    public int Size { get; set; }
    //    private List<string> Classes { get; set; }
    //    public string jsOnInputFunction { get; set; }

    //    public DropDownField(string Id, int VisibleRows = 1):base(Id)
    //    {
    //        this.GCType = GroupControlType.DropDownListBoxes;
    //        this.Size = VisibleRows;
    //        Classes = new List<string>();
    //        Classes.Add("ControlOfLabelControlDuo");
    //        jsOnInputFunction = "";
    //    }

    //    public void Add(DropDownOption toBeAdded)
    //    {
    //        Options.Add(toBeAdded);
    //    }

    //    public void AddClass(string newClass)
    //    {
    //        Classes.Add(newClass);
    //    }

    //    public static DropDownField GetRandom(int id, int count)
    //    {
    //        DropDownField n = new DropDownField(id+":DD", count - 1);
    //        for (int i = 0; i < count; i++)
    //        {
    //            DropDownOption no = new DropDownOption(i.ToString(), "DD" + i.ToString(), i % 2 == 0);
    //            n.Add(no);
    //        }
    //        return n;
    //    }

    //    public TagBuilder BuildHtmlTag()
    //    {
    //        TagBuilder tbDropDown = base.BuildBaseHtmlTag();
    //        for (int i = 0; i < Classes.Count; i++)
    //            tbDropDown.AddCssClass(Classes[i]);

    //        if (jsOnInputFunction!="")
    //            tbDropDown.Attributes.Add("oninput", jsOnInputFunction);

    //        //tbDropDown.Attributes.Add("name", NameId);

    //        if (Size > 1)
    //            tbDropDown.Attributes.Add("size", Size.ToString());

    //        for (int i = 0; i < Options.Count; i++)
    //        {
    //            TagBuilder t = Options[i].BuildHtmlTag();
    //            tbDropDown.InnerHtml.AppendHtml(t);
    //        }
    //        return tbDropDown;
    //    }

    //    //public override string GetValue()
    //    //{
    //    //    return "DropDownField,GetValue: TODO";
    //    //}
    //}
    //public class LabelDropDownField : Field,IHtmlElement
    //{
    //    public LabelField Label { get; set; }
    //    public DropDownField Dropdown { get; set; }

    //    public LabelDropDownField(string Id, string label, int VisibleRows = 1):base(Id, "div", null)
    //    {
    //        Init(Id, label, VisibleRows);
    //    }
    //    public LabelDropDownField(int Id, string label, int VisibleRows = 1):base(Id.ToString(), "div", null)
    //    {
    //        Init(Id.ToString(),label,VisibleRows);
    //    }
    //    private void Init(string Id, string label, int VisibleRows)
    //    {
    //        //GCType = GroupControlType.DropDownListBoxes;
    //        //IntId = Id;
    //        Label = new LabelField(label, true);
    //        Dropdown = new DropDownField(Id, VisibleRows);
    //    }

    //    public static Field GetRandom(int id, int count)
    //    {
    //        LabelDropDownField n = new LabelDropDownField(id, id + ":DDLabel");
    //        n.Dropdown = DropDownField.GetRandom(id, count);
    //        return n;
    //    }

    //    public TagBuilder BuildHtmlTag()
    //    {
    //        TagBuilder tb = base.BuildBaseHtmlTag();
    //        tb.AddCssClass("LabelControlDuo");
                
    //        tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
    //        tb.InnerHtml.AppendHtml(Dropdown.BuildHtmlTag());
    //        return tb;
    //    }

    //    //public override string GetValue()
    //    //{
    //    //    return Dropdown.GetValue();
    //    //}
    //}

    public class CheckBoxField : Field, IHtmlElement
    {
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public CheckBoxField(string GroupName, string IndividualValueID, string HtmlLabel, bool Checked, bool Disabled) :
            base(IndividualValueID, GroupName, GroupName, "input", HtmlLabel)
        {
            this.Checked = Checked;
            this.Disabled = Disabled;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.Attributes.Add("type", "checkbox");

            if (Checked)
                tb.Attributes.Add("checked", "checked");
            if (Disabled)
                tb.Attributes.Add("disabled", "disabled");

            return tb;
        }

    }
    public class RadioButtonField : Field, IHtmlElement
    {
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public RadioButtonField(string GroupName, string IndividualValueID, string HtmlLabel, bool Checked, bool Disabled):
            base(IndividualValueID, GroupName, GroupName, "input", HtmlLabel)
        {
            this.Checked = Checked;
            this.Disabled = Disabled;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.Attributes.Add("type", "radio");

            if (Checked)
                tb.Attributes.Add("checked", "checked");
            if (Disabled)
                tb.Attributes.Add("disabled", "disabled");

            return tb;
        }
    }

    //public class CheckBoxField : Field, IHtmlElement
    //{
    //    public string Name { get; set; }
    //    public string Id { get; set; }
    //    public string Value { get; set; }
    //    public bool Checked { get; set; }
    //    public bool Disabled { get; set; }

    //    public CheckBoxField(string GroupName, string HtmlLabel, string IndividualValueId, bool Checked = false, bool Disabled = false):
    //        base(IndividualValueId,"div", HtmlLabel)
    //    {
    //        this.Name = GroupName;
    //        this.Id = IndividualValueId;
    //        this.Value = IndividualValueId;
    //        this.Checked = Checked;
    //        this.Disabled = Disabled;
    //    }
    //    public CheckBoxField(string IndividualValueId, string HtmlLabel, bool Checked, WebTagAttributes wta):
    //        base(IndividualValueId,"div", HtmlLabel)
    //    {
    //        this.Name = wta.CheckBoxGroupName;
    //        this.Id = IndividualValueId;
    //        this.Value = IndividualValueId;
    //        this.Checked = Checked;
    //        this.Disabled = wta.ReadOnly;
    //    }

    //    public TagBuilder BuildHtmlTag()
    //    {
    //        TagBuilder tbLabelCheckBox = base.BuildBaseHtmlTag();

    //        TagBuilder tbCheckBox = new TagBuilder("input");
    //        tbCheckBox.Attributes.Add("type", "checkbox");
    //        tbCheckBox.Attributes.Add("name", Name);
    //        tbCheckBox.Attributes.Add("id", Id);
    //        tbCheckBox.Attributes.Add("value", Value);
    //        //tbCheckBox.Attributes.Add("value", Label);
    //        if (Checked)
    //            tbCheckBox.Attributes.Add("checked", "checked");
    //        if (Disabled)
    //            tbCheckBox.Attributes.Add("disabled", "disabled");

    //        tbLabelCheckBox.InnerHtml.AppendHtml(tbCheckBox);
    //        return tbLabelCheckBox;
    //    }

    //    //public override string GetValue()
    //    //{
    //    //    return Checked ? "1" : "-1";
    //    //}


    //}

    //public class LabelCheckBoxField : Field, IHtmlElement
    //{
    //    public LabelField Label { get; set; }
    //    public string Name { get; set; }
    //    public string Id { get; set; }
    //    public string Value { get; set; }
    //    public bool Checked { get; set; }
    //    public bool Disabled { get; set; }

    //    public LabelCheckBoxField(string ParentID, string GroupName, string HtmlLabel, string IndividualValueId, bool Checked = false, bool Disabled = false)
    //        :base(ParentID, IndividualValueId, GroupName,"div")
    //    {
    //        this.Name = GroupName;
    //        this.Id = IndividualValueId;
    //        this.Value = IndividualValueId;
    //        this.Label = new LabelField(HtmlLabel, false, IndividualValueId);
    //        this.Checked = Checked;
    //        this.Disabled = Disabled;
    //    }

    //    public TagBuilder BuildHtmlTag()
    //    {
    //        TagBuilder tbLabelCheckBox = base.BuildBaseHtmlTag();

    //            TagBuilder tbCheckBox = new TagBuilder("input");
    //            tbCheckBox.Attributes.Add("type", "checkbox");
    //            tbCheckBox.Attributes.Add("name", Name);
    //            tbCheckBox.Attributes.Add("id", Id);
    //            tbCheckBox.Attributes.Add("value", Value);
    //            if (Checked)
    //                tbCheckBox.Attributes.Add("checked", "checked");
    //            if (Disabled)
    //                tbCheckBox.Attributes.Add("disabled", "disabled");

    //            TagBuilder tbLabel = Label.BuildHtmlTag();

    //        tbLabelCheckBox.InnerHtml.AppendHtml(tbCheckBox);
    //        tbLabelCheckBox.InnerHtml.AppendHtml(Label.BuildHtmlTag());
    //        return tbLabelCheckBox;
    //    }

    //    public static Field GetRandom()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //public class LabelRadioButtonField : Field, IHtmlElement
    //{
    //    public LabelField Label { get; set; }
    //    public string Name { get; set; }
    //    public string Id { get; set; }
    //    public string Value { get; set; }
    //    public bool Checked { get; set; }
    //    public bool Disabled { get; set; }

    //    public LabelRadioButtonField(string ParentID, string Name, string Label, string IndividualValueId, bool Checked = false, bool Disabled = false):
    //        base(ParentID, IndividualValueId, Name, "div")
    //    {
    //        this.Name = Name;
    //        this.Id = IndividualValueId;
    //        this.Value = IndividualValueId;
    //        this.Label = new LabelField(Label, false, IndividualValueId);
    //        this.Checked = Checked;
    //        this.Disabled = Disabled;
    //    }

    //    public TagBuilder BuildHtmlTag()
    //    {
    //        TagBuilder tbLabelCheckBox = base.BuildBaseHtmlTag();

    //        TagBuilder tbCheckBox = new TagBuilder("input");
    //        tbCheckBox.Attributes.Add("type", "radio");
    //        tbCheckBox.Attributes.Add("name", Name);
    //        tbCheckBox.Attributes.Add("id", Id);
    //        tbCheckBox.Attributes.Add("value", Value);
    //        //tbCheckBox.Attributes.Add("value", Label);
    //        if (Checked)
    //            tbCheckBox.Attributes.Add("checked", "checked");
    //        if (Disabled)
    //            tbCheckBox.Attributes.Add("disabled", "disabled");

    //        TagBuilder tbLabel = Label.BuildHtmlTag();

    //        tbLabelCheckBox.InnerHtml.AppendHtml(tbCheckBox);
    //        tbLabelCheckBox.InnerHtml.AppendHtml(Label.BuildHtmlTag());
    //        return tbLabelCheckBox;
    //    }

    //    public static Field GetRandom()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public enum GroupControlType
    {
        None,
        RadioButtons,
        CheckBoxes,
        DropDownListBoxes
    };


    public class MultiSelectionItem : Field, IHtmlElement
    {
        public string Value { get; set; }
        public bool SelectedOrChecked { get; set; }
        public bool Disabled { get; set; }
        private Field ItemMain { get; set; }
        private Field ItemLabel { get; set; }

        public GroupControlType gct { get; set; }

        public MultiSelectionItem(GroupControlType ItemType, string PredecessorHtmlID, string GroupID, string ValueID, string VisibleText, bool SelectedOrChecked = false, bool Disabled = false) :
            base(null, null, null, "div", VisibleText)
        {
            Value = ValueID;
            gct = ItemType;
            this.SelectedOrChecked = SelectedOrChecked;
            this.Disabled = Disabled;
            string IDAndFor = PredecessorHtmlID + "-o" + ValueID;

            if (gct == GroupControlType.CheckBoxes || gct == GroupControlType.RadioButtons)
            {
                //Field MainField = new Field(GroupID + "-" + ValueID, GroupID, "input", null);
                Field MainField = new Field(null, ValueID, IDAndFor, "input", null);

                if (gct == GroupControlType.CheckBoxes)
                {
                    //MainField.NameAttribute = ValueID;
                    MainField.NameAttribute = IDAndFor;
                    MainField.ParentDBID = GroupID;

                    MainField.TypeAttribute = "checkbox";
                }
                else
                {
                    //MainField.NameAttribute = GroupID;
                    MainField.NameAttribute = PredecessorHtmlID;

                    MainField.TypeAttribute = "radio";
                    MainField.ValueAttribute = ValueID;
                }
                
                ItemMain = MainField;
                //ItemLabel = new LabelField(VisibleText, false, GroupID + "-" + ValueID);
                ItemLabel = new LabelField(VisibleText, false, IDAndFor);
                base.VisibleText = null;

            }
            else if (gct == GroupControlType.DropDownListBoxes)
            {
                //ItemMain = new Field(PredecessorHtmlID, ValueID, GroupID + "-" + ValueID, "option", VisibleText);
                ItemMain = new Field(PredecessorHtmlID, ValueID, PredecessorHtmlID + "-o" + ValueID, "option", VisibleText);
                ItemMain.NameAttribute = null;
            }
            else
                throw new Exception("MS: Unknown control in MultiSelectionControl");
        }
       
        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();

            if (gct == GroupControlType.CheckBoxes || gct == GroupControlType.RadioButtons)
            {
                TagBuilder tbItemMain = ItemMain.BuildBaseHtmlTag();

                if (this.SelectedOrChecked)
                    tbItemMain.Attributes.Add("checked", "");   //  TODO: Overit jestli ten 2. parametr muze byt prazdny.
                if (this.Disabled)
                    tbItemMain.Attributes.Add("disabled", "");   //  TODO: Overit jestli ten 2. parametr muze byt prazdny.

                tb.InnerHtml.AppendHtml(tbItemMain);
                tb.InnerHtml.AppendHtml(((LabelField)ItemLabel).BuildHtmlTag());
            }
            else if (gct == GroupControlType.DropDownListBoxes)
            {
                tb.InnerHtml.AppendHtml(ItemMain.BuildBaseHtmlTag());
            }
            return tb;
        }

        public override List<Field> GetDBFields()
        {
            return new List<Field> { ItemMain };
        }

    }

    public class MultiSelectionControl : Field, IHtmlElement
    {
        private List<MultiSelectionItem> items { get; set; }
        private List<string> SelectedItems { get; set; }
        public GroupControlType gct { get; set; }
        public string GroupID { get; set; }
        private string CssClass { get; set; }
        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        //public MultiSelectionControl(GroupControlType FieldType, string ParentID, string id, string GroupID) : 
        public MultiSelectionControl(GroupControlType FieldType, string ParentID, string GroupID) :
            //base(ParentID, id, ParentID + "-g" + id, null, null)
            base(ParentID, GroupID, ParentID + "-g" + GroupID, null, null)
        {
            this.GroupID = GroupID;
            base.NameAttribute = null;
            items = new List<MultiSelectionItem>();
            SelectedItems = new List<string>();
            gct = FieldType;

            CssClass = "ControlOfLabelControlDuo";
            if (gct == GroupControlType.DropDownListBoxes)
            {
                base.HtmlTag = "select";
                base.NameAttribute = ParentID + "-g" + GroupID;
                //base.ParentID = ParentID;       //  Aby se u DDLB tisknuto jeho ParentID
            }
            else if (gct == GroupControlType.CheckBoxes || gct == GroupControlType.RadioButtons)
            {   //  NameAttribute and ParentID = NULL. To proto aby se u DIVu netiskli. ID musi zustat kvuli "AddRelevantItems". A navic v tom DIVu nevadi.
                base.HtmlTag = "div";
            }
            else
                throw new Exception("MS: Unknown control in MultiSelectionControl");
        }

        public bool AddItem(MultiSelectionItem item)
        {
            if (ExistsInExistingItems(item.Value))
                return false;
            else
            {
                items.Add(item);
                if (item.SelectedOrChecked && !ExistsInSelectedValuesArrays(item.HTMLFieldID))
                    SelectedItems.Add(item.HTMLFieldID);
                return true;
            }
        }

        /// <summary>
        /// Defines the list of selected/checked items. These items don't even have to have been added to the control yet.
        /// </summary>
        /// <param name="Selected">String of comma separeated values</param>
        public void SetSelectedItems(string Selected)
        {
            if (Selected == "")
            {
                SelectedItems = new List<string>();
                return;
            }
            else
            {
                string[] a = Selected.Split(',');
                SelectedItems = new List<string>(a);
            }
        }

        public void AddRelevantItems(List<TempMultiSelectItem> AllMultiSelectItem)
        {
            for (int i = 0; i < AllMultiSelectItem.Count; i++)
            {
                //if (AllMultiSelectItem[i].ParentFieldID == base.HTMLFieldID)
                if (AllMultiSelectItem[i].ParentFieldID == base.DBFieldID)
                {
                    bool bCheckedOrSelected = (ExistsInSelectedValuesArrays(AllMultiSelectItem[i].MultiSelectItemID.ToString()) ? true : false);
                    //items.Add(new MultiSelectionItem(gct, this.ParentID + "f" + this.HTMLFieldID + "-", GroupID, AllMultiSelectItem[i].MultiSelectItemID, AllMultiSelectItem[i].StringValue, bCheckedOrSelected));
                    items.Add(new MultiSelectionItem(gct, this.ParentID + "-g" + GroupID, GroupID, AllMultiSelectItem[i].MultiSelectItemID, AllMultiSelectItem[i].StringValue, bCheckedOrSelected));
                }
            }
        }

        private bool ExistsInSelectedValuesArrays(string id)
        {
            for (int i = 0; i < SelectedItems.Count; i++)
                if (SelectedItems[i] == id)       //  Cannot compare with items[i].ID !!
                    return true;
            return false;
        }
        private bool ExistsInExistingItems(string id)
        {
            for (int i = 0; i < items.Count; i++)
                if (items[i].Value == id)       
                    return true;
            return false;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass(CssClass);

            if (tb != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    tb.InnerHtml.AppendHtml(items[i].BuildHtmlTag());
                    //tb = items[i].BuildHtmlTag(tb);
                }   
            }
            return tb;
        }

        public override List<Field> GetDBFields()
        {
            List<Field> l = new List<Field>();

            for (int i = 0; i < items.Count; i++)
            {
                List<Field> inner = items[i].GetDBFields();
                for (int r = 0; r < inner.Count; r++)
                {
                    l.Add(inner[r]);
                }
            }
            return l;
        }
    }


    public class RadioButtonFields : MultiSelectionControl, IHtmlElement
    {
        //public RadioButtonFields(string ParentID, string NameID) : base(GroupControlType.RadioButtons, ParentID, NameID, NameID) { }
        public RadioButtonFields(string ParentID, string NameID) : base(GroupControlType.RadioButtons, ParentID, NameID) { }

        public void Add(string ItemID, string VisibleText, bool SelectedOrChecked = false, bool Disabled = false)
        {
            AddItem(new MultiSelectionItem(GroupControlType.RadioButtons, this.ParentID, this.HTMLFieldID, ItemID, VisibleText, SelectedOrChecked, Disabled ));
        }

        public new TagBuilder BuildHtmlTag()
        {
            return base.BuildHtmlTag();
        }
    }
    public class LabelRadioButtonFields : Field, IHtmlElement, ILabelBeforeControl
    {
        public LabelField Label { get; set; }
        public RadioButtonFields RadioButtons { get; set; }

        public LabelRadioButtonFields(string ParentID, string NameID, string VisibleText) : base(null, null, null, "div", null)
        {
            this.Label = new LabelField(VisibleText, true);
            RadioButtons = new RadioButtonFields(ParentID, NameID);
        }

        public static Field GetRandom(string id, int count)
        {
            return new LabelRadioButtonFields("random", id, "Label-" + id.ToString());
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");

            tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(RadioButtons.BuildHtmlTag());

            return tb;
        }

        public override List<Field> GetDBFields()
        {
            return RadioButtons.GetDBFields();
        }
    }

    public class CheckBoxFields : MultiSelectionControl, IHtmlElement
    {
        //public CheckBoxFields(string ParentID, string NameID) : base(GroupControlType.CheckBoxes, ParentID, NameID, NameID) {}
        public CheckBoxFields(string ParentID, string NameID) : base(GroupControlType.CheckBoxes, ParentID, NameID) { }

        public void Add(string ItemID, string VisibleText, bool SelectedOrChecked = false, bool Disabled = false)
        {
            AddItem(new MultiSelectionItem(GroupControlType.CheckBoxes, this.ParentID, this.HTMLFieldID, ItemID, VisibleText, SelectedOrChecked, Disabled));
        }

        public new TagBuilder BuildHtmlTag()
        {   // Zadny base.BuildBaseHtmlTag();
            return base.BuildHtmlTag();
        }
    }
    public class LabelCheckBoxFields : Field, IHtmlElement
    {
        public LabelField Label { get; set; }
        public CheckBoxFields CheckBoxes{ get; set; }

        public LabelCheckBoxFields(string ParentID, string NameID, string VisibleValue) : base(null, null, null, "div", null)
        {
            this.Label = new LabelField(VisibleValue, true);
            CheckBoxes = new CheckBoxFields(ParentID, NameID);
        }

        public static Field GetRandom(string id, int count)
        {
            return new LabelCheckBoxFields("random", id, "Label-" + id.ToString());
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");

            tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(CheckBoxes.BuildHtmlTag());

            return tb;
        }

        public override List<Field> GetDBFields()
        {
            return CheckBoxes.GetDBFields();
        }
    }

    public class DropDownListBox : MultiSelectionControl, IHtmlElement
    {
        public int VisibleRows { get; set; }
        public List<string> Classes { get; set; }
        public string jsOnInputFunction { get; set; }

        public DropDownListBox(string ParentID, string NameID, int VisibleRows=1)
            //: base(GroupControlType.DropDownListBoxes, ParentID, NameID, NameID)
            : base(GroupControlType.DropDownListBoxes, ParentID, NameID)
        {
            Classes = new List<string>();
            this.VisibleRows = VisibleRows;
            Classes.Add("ControlOfLabelControlDuo");
            jsOnInputFunction = "";
        }

        public void Add(string ItemID, string VisibleText, bool SelectedOrChecked = false, bool Disabled = false)
        {
            AddItem(new MultiSelectionItem(GroupControlType.DropDownListBoxes, this.ParentID, this.HTMLFieldID, ItemID, VisibleText, SelectedOrChecked, Disabled ));
        }

        public new TagBuilder BuildHtmlTag()
        {
            TagBuilder tbDropDown = base.BuildHtmlTag();
            for (int i = 0; i < Classes.Count; i++)
                tbDropDown.AddCssClass(Classes[i]);

            if (jsOnInputFunction != "")
                tbDropDown.Attributes.Add("oninput", jsOnInputFunction);

            if (VisibleRows > 1)
                tbDropDown.Attributes.Add("size", VisibleRows.ToString());
            
            return tbDropDown;
        }
    }
    public class LabelDropDownListBox : Field, IHtmlElement
    {
        public LabelField Label { get; set; }
        public DropDownListBox DropDown { get; set; }

        public LabelDropDownListBox(string ParentID, string NameID, string VisibleValue) : base(null, null, null, "div", null)
        {
            this.Label = new LabelField(VisibleValue, true);
            DropDown = new DropDownListBox(ParentID, NameID);
        }

        public static Field GetRandom(string id, int count)
        {
            return new LabelDropDownListBox("random",id, "Label-" + id.ToString());
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");

            tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(DropDown.BuildHtmlTag());

            return tb;
        }

        public override List<Field> GetDBFields()
        {
            return DropDown.GetDBFields();
        }
    }


    



}