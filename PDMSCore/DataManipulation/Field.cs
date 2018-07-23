using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;

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

    public interface IHtmlElement
    {
        TagBuilder BuildHtmlTag();
    }

    public class Field
    {
        private string Name { get; set; }
        private string id{ get; set; }
        public string ID
        {
            get {
                return id;
            }
        }
        public string HtmlTag { get; set; }
        private string VisibleText { get; set; }

        public Field(string NameID, string HtmlTag, string FieldValue)
        {
            this.Name = NameID;
            this.id = NameID;
            this.HtmlTag = HtmlTag;
            this.VisibleText = FieldValue;
        }
        public Field(string ID, string Name, string HtmlTag, string VisibleText)
        {
            this.id = ID;
            this.Name = Name;
            this.HtmlTag = HtmlTag;
            this.VisibleText = VisibleText;
        }

        public TagBuilder BuildBaseHtmlTag()
        {
            TagBuilder tb = null;
            if (HtmlTag != null)
            {
                tb = new TagBuilder(HtmlTag);
                if (Name != null)
                    tb.Attributes.Add("name", Name);
                if (ID != null)
                    tb.Attributes.Add("id", id);
                if (this.VisibleText != null)
                    tb.InnerHtml.AppendHtml(VisibleText);
            }
            return tb;
        }

        public string GetValue()
        {
            return VisibleText;
        }

    }

    

    public class MultiOption : Field
    {
        public int OtherRef { get; set; }
        public string[] SelectedValues { get; set; }
        public List<Field> Options { get; set; }
        public GroupControlType GCType { get; set; }

        public MultiOption(string NameID) : base(NameID, "select", null)
        {
            Options = new List<Field>();
        }

        public void AddRelevantItems(List<TempMultiSelectItem> AllMultiSelectItem)
        {
            for (int i = 0; i < AllMultiSelectItem.Count; i++)
            {
                if (AllMultiSelectItem[i].ParentFieldID == base.ID)
                {
                    bool bChecked = (ExistsInSelectedValuesArrays(AllMultiSelectItem[i].MultiSelectItemID.ToString()) ? true : false);

                    if (GCType == GroupControlType.None)
                    {
                        throw new Exception("MS: Undefined GroupControlType in AddRelevantItems.");
                    }
                    else if (GCType == GroupControlType.DropDownListBoxes)
                    {   //DropDownOption
                        T aa = (T)Activator.CreateInstance(typeof(T), AllMultiSelectItem[i].MultiSelectItemID.ToString(), AllMultiSelectItem[i].StringValue);
                        Options.Add(aa);
                    }
                    else if (GCType == GroupControlType.RadioButtons || GCType == GroupControlType.CheckBoxes)
                    {
                        T aa = (T)Activator.CreateInstance(typeof(T), "input", AllMultiSelectItem[i].StringValue, AllMultiSelectItem[i].MultiSelectItemID.ToString(), bChecked, false);
                        Options.Add(aa);
                    }
                }
            }
        }

        private bool ExistsInSelectedValuesArrays(string h)
        {
            for (int i = 0; i < SelectedValues.Length; i++)
                if (SelectedValues[i] == h)
                    return true;
            return false;
        }
    }

    //public abstract class MultiOption<T>: Field
    //{
    //    public int OtherRef { get; set; }
    //    public string[] SelectedValues { get; set; }
    //    public List<T> Options { get; set; }
    //    public GroupControlType GCType { get; set; }

    //    public MultiOption(string NameID) :base(NameID,"select",null)
    //    {
    //        Options = new List<T>();
    //    }

    //    public void AddRelevantItems(List<TempMultiSelectItem> AllMultiSelectItem)
    //    {
    //        for (int i = 0; i < AllMultiSelectItem.Count; i++)
    //        {
    //            if (AllMultiSelectItem[i].ParentFieldID == base.ID)
    //            {
    //                bool bChecked = (ExistsInSelectedValuesArrays(AllMultiSelectItem[i].MultiSelectItemID.ToString()) ? true : false);

    //                if (GCType == GroupControlType.None)
    //                {
    //                    throw new Exception("MS: Undefined GroupControlType in AddRelevantItems.");
    //                }
    //                else if (GCType == GroupControlType.DropDownListBoxes)
    //                {   //DropDownOption
    //                    T aa = (T)Activator.CreateInstance(typeof(T), AllMultiSelectItem[i].MultiSelectItemID.ToString(), AllMultiSelectItem[i].StringValue);
    //                    Options.Add(aa);
    //                }
    //                else if (GCType == GroupControlType.RadioButtons || GCType == GroupControlType.CheckBoxes)
    //                {
    //                    T aa = (T)Activator.CreateInstance(typeof(T), "input", AllMultiSelectItem[i].StringValue, AllMultiSelectItem[i].MultiSelectItemID.ToString(), bChecked, false);
    //                    Options.Add(aa);
    //                }
    //            }
    //        }
    //    }

    //    private bool ExistsInSelectedValuesArrays(string h)
    //    {
    //        for (int i = 0; i < SelectedValues.Length; i++)
    //            if (SelectedValues[i] == h)
    //                return true;
    //        return false;
    //    }
    //}

    public class NewLine : Field, IHtmlElement
    {
        public NewLine():base(null,"/br",null)
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
        public HiddenField(string NameId):base(NameId,"input", null)
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

    public class LabelDataGridField : Field, IHtmlElement
    {
        public LabelField label { get; set; }
        public DataGridField table { get; set; }

        public LabelDataGridField(string LabelText, DataGridField table):base("", "div", null)
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
    }

    public class LabelField : Field, IHtmlElement
    {
        public string HtmlLabel { get; set; }
        public string For { get; set; }

        public string ToolTip { get; set; }
        public string PlaceHolder { get; set; }
        public bool Bold { get; set; }

        public LabelField(string HtmlLabel, bool Bold = false, string For = null):base(null, "label", null)
        {
            this.Bold = Bold;
            this.HtmlLabel = HtmlLabel;
            this.For = For;
        }

        //public override TagBuilder HtmlText()
        //{
        //    TagBuilder tb = new TagBuilder("label");
        //    tb.AddCssClass(Bold ? "BoldLabelField" : "LabelField");
        //    if (this.For != null)
        //        tb.Attributes.Add("for", this.For);
        //    tb.InnerHtml.AppendHtml(HtmlLabel);
        //    return tb;
        //}

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
    public class TextAreaField : Field, IHtmlElement
    {
        public int Rows { get; set; }
        public string Placeholder { get; set; }
        public string Text { get; set; }

        public TextAreaField(string NameId, string Text, string Placeholder, int Rows):base(NameId, "textarea", null)
        {
            this.Text = Text;
            this.Placeholder = Placeholder;
            this.Rows = Rows;
        }

        //public override TagBuilder HtmlText()
        //{
        //    TagBuilder tb = new TagBuilder("textarea");
        //    tb.AddCssClass("ControlOfLabelControlDuo");
        //    tb.Attributes.Add("name", NameId);
        //    tb.Attributes.Add("id", NameId);
        //    tb.Attributes.Add("rows", Rows.ToString());
        //    tb.Attributes.Add("placeholder", Placeholder);
        //    tb.InnerHtml.AppendHtml(WebUtility.HtmlEncode(Text));
        //    return tb;
        //}

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
    public class LabelTextAreaField : Field,IHtmlElement
    {   //  <textarea rows="4" cols="50" placeholder="Describe yourself here...">Value</textarea>
        public LabelField label { get; set; }
        public TextAreaField textArea { get; set; }


        public LabelTextAreaField(int Id, string LabelText, string Text="", string Placeholder = "...", int Rows = 4):base(Id.ToString(),"div", null)
        {
            
            label = new LabelField(LabelText, true);
            textArea = new TextAreaField(Id.ToString(), Text, Placeholder, Rows);
        }

        //public override TagBuilder HtmlText()
        //{
        //    TagBuilder tb = new TagBuilder("div");
        //    tb.AddCssClass("LabelControlDuo");
        //    tb.InnerHtml.AppendHtml(label.HtmlText());
        //    tb.InnerHtml.AppendHtml(textArea.HtmlText());
        //    return tb;
        //}
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
            LabelTextAreaField a = new LabelTextAreaField(id, "TextArea label", null, "holder", 4);
            return a;
        }

    }

    public class TextBoxField : Field, IHtmlElement
    {
        public string Text { get; set; }
        public string PlaceHolder { get; set; }
        public string ToolTip { get; set; }
        public string OnClick { get; set; }
        public bool ReadOnly { get; set; }
        private string CSS { get; set; }

        public TextBoxField(string NameId, string CSS, string Text="", string PlaceHolder = "", string ToolTip=""):base(NameId, "input", Text)
        {
            this.Text = Text;
            this.PlaceHolder = PlaceHolder;
            this.ToolTip = ToolTip;
            this.CSS = CSS;
            this.ReadOnly = false;
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

    }
    public class LabelTextBoxField : Field, IHtmlElement
    {
        private LabelField label;
        private TextBoxField txtb;

        public LabelTextBoxField(int Id, string LabelText, string Text, string Placeholder="...", string ToolTip=""):base(Id.ToString(),"div", null)
        {
            label = new LabelField(LabelText,true);
            txtb = new TextBoxField(Id.ToString(), "ControlOfLabelControlDuo", Text, Placeholder, ToolTip);
        }

        public static Field GetRandom(int Id)
        {
            LabelTextBoxField a = new LabelTextBoxField(Id,Id+":labelText","","placeholder","tooltip");
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
    }

    public class LabelSelectableTextBoxField : Field, IHtmlElement
    {
        private LabelField label;
        private TextBoxField txtb;
        private HiddenField hiddenField;
        public string DataGridSP;

        public LabelSelectableTextBoxField(int Id, string LabelText, string Text, string dataGridSP):base(Id.ToString(),"div", null)
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

        public FileUploadField(string NameId, bool MultipleFile = false, string ToolTip = ""):base(NameId,"input", null)
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

        public LabelFileUploadField(string HtmlText, FileUploadField f):base(null,"div", null)
        {
            label = new LabelField(HtmlText,true);
            fu = f;
        }

        public static Field GetRandom(bool MultipleUpload=false)
        {
            LabelFileUploadField a = new LabelFileUploadField("fu",new FileUploadField("fuu", MultipleUpload,"Tooltip..."));
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

        public FileDownloadField(string NameId, string Path, string ToolTip = ""): base(NameId,"a", null)
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

        public LabelFileDownloadField(string HtmlText, FileDownloadField f):base(null,"div", null)
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

        public DatePickerField(string Id, DateTime? Value = null, DateTime? MinDate = null, DateTime? MaxDate = null, bool Enabled = true):
            base(Id, "input", null)
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

        public LabelDatePickerField(int Id, string HtmlLabel, DateTime? Value = null, DateTime? MinDate = null, DateTime? MaxDate = null)
            :base(Id.ToString(), "div", null)
        {
            this.Label = new LabelField(HtmlLabel, true);
            this.DatePicker = new DatePickerField(Id.ToString(), Value, MinDate, MaxDate);
        }

        public static Field GetRandom(int Id, DateTime? DefaultValue = null)
        {
            DefaultValue = (DefaultValue == null ? DateTime.Now : DefaultValue);
            LabelDatePickerField a = new LabelDatePickerField(Id,"DP Label",DefaultValue );
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

    //public class DropDownOption : IHtmlTag
    public class DropDownOption : Field, IHtmlElement
    {
        //public string Value { get; set; }
        public string Label { get; set; }
        public bool Enabled { get; set; }

        public DropDownOption(string ValueId, string Label, bool Enabled = true): base(ValueId,"option", Label)
        {
            //this.NameId ="DD" + ValueId.ToString();
            this.Label = Label;
            this.Enabled = Enabled;
        }

        public DropDownOption(string Name, string Label):base(Name,"option", Label)
        {
            this.Label = Label;
            this.Enabled = true;
        }

        public TagBuilder BuildHtmlTag()
        {   //  <option label="England" value="England"></option> 
            TagBuilder tb = base.BuildBaseHtmlTag();
            //tb.Attributes.Add("value", NameId);
            tb.Attributes.Add("value", "TODO");
            if (!Enabled)
                tb.Attributes.Add("disabled", "");
            tb.InnerHtml.AppendHtml(Label);
            return tb;
        }

        //public override string GetValue()
        //{
        //    return Label;
        //}
    }
    public class DropDownField : MultiOption, IHtmlElement
    {
        //List<DropDownOption> Options { get; set; }
        public int Size { get; set; }
        private List<string> Classes { get; set; }
        public string jsOnInputFunction { get; set; }

        public DropDownField(string Id, int VisibleRows = 1):base(Id)
        {
            this.GCType = GroupControlType.DropDownListBoxes;
            this.Size = VisibleRows;
            Classes = new List<string>();
            Classes.Add("ControlOfLabelControlDuo");
            jsOnInputFunction = "";
        }

        public void Add(DropDownOption toBeAdded)
        {
            Options.Add(toBeAdded);
        }

        public void AddClass(string newClass)
        {
            Classes.Add(newClass);
        }

        public static DropDownField GetRandom(int id, int count)
        {
            DropDownField n = new DropDownField(id+":DD", count - 1);
            for (int i = 0; i < count; i++)
            {
                DropDownOption no = new DropDownOption(i.ToString(), "DD" + i.ToString(), i % 2 == 0);
                n.Add(no);
            }
            return n;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tbDropDown = base.BuildBaseHtmlTag();
            for (int i = 0; i < Classes.Count; i++)
                tbDropDown.AddCssClass(Classes[i]);

            if (jsOnInputFunction!="")
                tbDropDown.Attributes.Add("oninput", jsOnInputFunction);

            //tbDropDown.Attributes.Add("name", NameId);

            if (Size > 1)
                tbDropDown.Attributes.Add("size", Size.ToString());

            for (int i = 0; i < Options.Count; i++)
            {
                TagBuilder t = Options[i].BuildHtmlTag();
                tbDropDown.InnerHtml.AppendHtml(t);
            }
            return tbDropDown;
        }

        //public override string GetValue()
        //{
        //    return "DropDownField,GetValue: TODO";
        //}
    }
    public class LabelDropDownField : Field,IHtmlElement
    {
        public LabelField Label { get; set; }
        public DropDownField Dropdown { get; set; }

        public LabelDropDownField(string Id, string label, int VisibleRows = 1):base(Id, "div", null)
        {
            Init(Id, label, VisibleRows);
        }
        public LabelDropDownField(int Id, string label, int VisibleRows = 1):base(Id.ToString(), "div", null)
        {
            Init(Id.ToString(),label,VisibleRows);
        }
        private void Init(string Id, string label, int VisibleRows)
        {
            //GCType = GroupControlType.DropDownListBoxes;
            //IntId = Id;
            Label = new LabelField(label, true);
            Dropdown = new DropDownField(Id, VisibleRows);
        }

        public static Field GetRandom(int id, int count)
        {
            LabelDropDownField n = new LabelDropDownField(id, id + ":DDLabel");
            n.Dropdown = DropDownField.GetRandom(id, count);
            return n;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");
                
            tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(Dropdown.BuildHtmlTag());
            return tb;
        }

        //public override string GetValue()
        //{
        //    return Dropdown.GetValue();
        //}
    }

    public class CheckBoxField : Field, IHtmlElement
    {
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public CheckBoxField(string GroupName, string IndividualValueID, string HtmlLabel, bool Checked, bool Disabled) :
            base(IndividualValueID, GroupName, "input", HtmlLabel)
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
            base(IndividualValueID, GroupName, "input", HtmlLabel)
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

    public class LabelCheckBoxField : Field, IHtmlElement
    {
        public LabelField Label { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public LabelCheckBoxField(string GroupName, string HtmlLabel, string IndividualValueId, bool Checked = false, bool Disabled = false)
            :base(IndividualValueId,GroupName,"div")
        {
            this.Name = GroupName;
            this.Id = IndividualValueId;
            this.Value = IndividualValueId;
            this.Label = new LabelField(HtmlLabel, false, IndividualValueId);
            this.Checked = Checked;
            this.Disabled = Disabled;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tbLabelCheckBox = base.BuildBaseHtmlTag();

                TagBuilder tbCheckBox = new TagBuilder("input");
                tbCheckBox.Attributes.Add("type", "checkbox");
                tbCheckBox.Attributes.Add("name", Name);
                tbCheckBox.Attributes.Add("id", Id);
                tbCheckBox.Attributes.Add("value", Value);
                if (Checked)
                    tbCheckBox.Attributes.Add("checked", "checked");
                if (Disabled)
                    tbCheckBox.Attributes.Add("disabled", "disabled");

                TagBuilder tbLabel = Label.BuildHtmlTag();

            tbLabelCheckBox.InnerHtml.AppendHtml(tbCheckBox);
            tbLabelCheckBox.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            return tbLabelCheckBox;
        }

        public static Field GetRandom()
        {
            throw new NotImplementedException();
        }

        //public override string GetValue()
        //{
        //    return Value;
        //}
    }
    public class LabelRadioButtonField : Field, IHtmlElement
    {
        public LabelField Label { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public LabelRadioButtonField(string Name, string Label, string IndividualValueId, bool Checked = false, bool Disabled = false):base(IndividualValueId, Name, "div")
        {
            this.Name = Name;
            this.Id = IndividualValueId;
            this.Value = IndividualValueId;
            this.Label = new LabelField(Label, false, IndividualValueId);
            this.Checked = Checked;
            this.Disabled = Disabled;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tbLabelCheckBox = base.BuildBaseHtmlTag();

            TagBuilder tbCheckBox = new TagBuilder("input");
            tbCheckBox.Attributes.Add("type", "radio");
            tbCheckBox.Attributes.Add("name", Name);
            tbCheckBox.Attributes.Add("id", Id);
            tbCheckBox.Attributes.Add("value", Value);
            //tbCheckBox.Attributes.Add("value", Label);
            if (Checked)
                tbCheckBox.Attributes.Add("checked", "checked");
            if (Disabled)
                tbCheckBox.Attributes.Add("disabled", "disabled");

            TagBuilder tbLabel = Label.BuildHtmlTag();

            tbLabelCheckBox.InnerHtml.AppendHtml(tbCheckBox);
            tbLabelCheckBox.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            return tbLabelCheckBox;
        }

        public static Field GetRandom()
        {
            throw new NotImplementedException();
        }

        //public override string GetValue()
        //{
        //    return Value;
        //}
    }
    public enum GroupControlType
    {
        None,
        RadioButtons,
        CheckBoxes,
        DropDownListBoxes
    };

    public class ItemOfMultiItem : Field, IHtmlElement
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public bool SelectedOrChecked { get; set; }

        public GroupControlType gct { get; set; }

        public ItemOfMultiItem(GroupControlType ItemType, string GroupID, string ValueID, string VisibleText, bool SelectedOrChecked = false) :
            base(null, null, HtmlTag, VisibleText)
        {
            Value = ValueID;
            gct = ItemType;
            Name = GroupID;
            this.SelectedOrChecked = SelectedOrChecked;

            //  This should be happenning after the base(..) call. That's why I write the correct Tag here.
            //  Although it could be more efficient as the if(gct == ..) is called twice. Here and later in BuildHtmlTag.
            if (gct == GroupControlType.CheckBoxes || gct == GroupControlType.RadioButtons)
                base.HtmlTag = "input";
            else if (gct == GroupControlType.DropDownListBoxes)
                base.HtmlTag = "option";
            else
                throw new Exception("MS: Unknown control in MultiSelectionControl");
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("");

            tb.Attributes.Add("value", Value);

            if (gct == GroupControlType.CheckBoxes)
            {
                tb.Attributes.Add("type", "checkbox");
                tb.Attributes.Add("name", Name);
                tb.Attributes.Add("checked", "");   //  TODO: Overit jestli ten 2. parametr muze byt prazdny.
            }
            else if (gct == GroupControlType.RadioButtons)
            {
                tb.Attributes.Add("type", "radio");
                tb.Attributes.Add("name", Name);
                tb.Attributes.Add("checked", "");   //  TODO: Overit jestli ten 2. parametr muze byt prazdny.
            }
            else if (gct == GroupControlType.DropDownListBoxes)
            {

            }
            else
                throw new Exception("MS: Unknown control type in ItemOfMultiItem");
            return tb;
        }
    }

    public class MultiItems : IHtmlElement
    {
        public List<ItemOfMultiItem> items { get; set; }
        public GroupControlType gct { get; set; }
        public string GroupID { get; set; }
        public int Count {
            get {
                return items.Count;
            }
        }

        public MultiItems(GroupControlType FieldType, string GroupID)
        {
            items = new List<ItemOfMultiItem>();
            gct = FieldType;
            this.GroupID = GroupID;
        }

        public void Add(string ValueID, string VisibleText)
        {
            string tag = null;

            if (gct == GroupControlType.CheckBoxes || gct == GroupControlType.RadioButtons)
                tag = "input";
            else if (gct == GroupControlType.DropDownListBoxes)
                tag = "option";
            else
                throw new Exception("MS: Adding non-existing item to MultiItems.");

            items.Add(new ItemOfMultiItem(gct, GroupID, ValueID, VisibleText, tag));
        }

        public TagBuilder BuildHtmlTag(TagBuilder tb)
        {
            for (int i = 0; i < items.Count; i++)
            {
                tb.InnerHtml.AppendHtml(items[i].BuildHtmlTag());
            }
            return tb;
        }
        public TagBuilder BuildHtmlTag()
        {
            throw new Exception("MS: BuildHtmlTag() shouldn't be used.");
        }
    }

    public class RBCBControl : Field,  IHtmlElement
    {
        private MultiItems items{ get; set; }

        public RBCBControl(GroupControlType FieldType, string id, string GroupID) : base(id, "div", null)
        {
            items = new MultiItems(FieldType, GroupID);
        }

        public void AddRelevantItems(List<TempMultiSelectItem> AllMultiSelectItem)
        {
            for (int i = 0; i < AllMultiSelectItem.Count; i++)
            {
                if (AllMultiSelectItem[i].ParentFieldID == base.ID)
                {
                    //bool bChecked = (ExistsInSelectedValuesArrays(AllMultiSelectItem[i].MultiSelectItemID.ToString()) ? true : false);
                    items.Add(AllMultiSelectItem[i].MultiSelectItemID, AllMultiSelectItem[i].StringValue);
                }
            }
        }

        /*private bool ExistsInSelectedValuesArrays(string h)
        {
            for (int i = 0; i < items.Count; i++)
                if (items.items[i].ID == h)
                    return true;
            return false;
        }*/

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb = items.BuildHtmlTag(tb);
            return tb;
        }
    }

    public class LabelRBCBControl :Field, IHtmlElement
    {
        public LabelField Label { get; set; }
        public RBCBControl RBCBControl { get; set; }
        private GroupControlType GCType { get; set; }

        public LabelRBCBControl(GroupControlType FieldType, string id, string label) : base(id, "div", null)
        {
            this.Label = new LabelField(label, true);
            RBCBControl = new RBCBControl(FieldType,id,id);
        }

        public void AddRelevantItems(List<TempMultiSelectItem> AllMultiSelectItem)
        {
            RBCBControl.AddRelevantItems(AllMultiSelectItem);
        }

        public static Field GetRandom(string id, int count)
        {
            return null;

            //LabelRBCBControl<T> n = new LabelRBCBControl<T>(id, "control global label");
            //for (int i = 0; i < count; i++)
            //{
            //    string Name = typeof(T).ToString() + DateTime.Now.ToLongTimeString();
            //    //          string Name, string Label, string IndividualValueId, bool Checked = false, bool Disabled = false
            //    T aa = (T)Activator.CreateInstance(typeof(T), Name, typeof(T).ToString() + i.ToString(), typeof(T).ToString() + i.ToString(), false, false);
            //    RBCBControl.items.Options.Add(aa);
            //}
            //return n;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");

            tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(RBCBControl.BuildHtmlTag());
            
            return tb;
        }
    }

    public class MultiSelectionControl : Field, IHtmlElement
    {
        public List<ItemOfMultiItem> items { get; set; }
        public List<int> SelectedItems { get; set; }
        public GroupControlType gct { get; set; }
        public string GroupID { get; set; }
        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public MultiSelectionControl(GroupControlType FieldType, string id, string GroupID) : base(id, null, null)
        {
            this.GroupID = GroupID;
            items = new List<ItemOfMultiItem>();
            gct = FieldType;

            if (gct == GroupControlType.DropDownListBoxes)
                base.HtmlTag = "select";
            else if (gct == GroupControlType.CheckBoxes || gct == GroupControlType.CheckBoxes)
                base.HtmlTag = "select";
            else
                throw new Exception("MS: Unknown control in MultiSelectionControl");
        }

        public void AddRelevantItems(List<TempMultiSelectItem> AllMultiSelectItem)
        {
            for (int i = 0; i < AllMultiSelectItem.Count; i++)
            {
                if (AllMultiSelectItem[i].ParentFieldID == base.ID)
                {
                    bool bCheckedOrSelected = (ExistsInSelectedValuesArrays(AllMultiSelectItem[i].MultiSelectItemID.ToString()) ? true : false);
                    items.Add(new ItemOfMultiItem(gct, GroupID, AllMultiSelectItem[i].MultiSelectItemID, AllMultiSelectItem[i].StringValue, bCheckedOrSelected));
                }
            }
        }
        private bool ExistsInSelectedValuesArrays(string id)
        {
            for (int i = 0; i < items.Count; i++)
                if (items[i].Value == id)       //  Cannot compare with items[i].ID !!
                    return true;
            return false;
        }

        public TagBuilder BuildHtmlTag()    //  Umyslne ignoruji Base.BuildHtmlTag(...) (beztak do jeho konstruktora jako jeho TagString posilam NULL)
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            
            if (tb != null)
            {
                for (int i = 0; i < items.Count; i++)
                    tb.InnerHtml.AppendHtml(items[i].BuildHtmlTag());
            }
            return tb;
        }
    }
    public class LabelMultiSelectionControl : Field, IHtmlElement
    {
        public LabelField Label { get; set; }
        public MultiSelectionControl MultiSelectionControl { get; set; }
        private GroupControlType GCType { get; set; }

        public LabelMultiSelectionControl(GroupControlType FieldType, string id, string label) : base(id, "div", null)
        {
            this.Label = new LabelField(label, true);
            RBCBControl = new RBCBControl(FieldType, id, id);
        }

        public void AddRelevantItems(List<TempMultiSelectItem> AllMultiSelectItem)
        {
            RBCBControl.AddRelevantItems(AllMultiSelectItem);
        }

        public static Field GetRandom(string id, int count)
        {
            return null;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.AddCssClass("LabelControlDuo");

            tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
            tb.InnerHtml.AppendHtml(RBCBControl.BuildHtmlTag());

            return tb;
        }
    }

    //public class LabelRBCBControl<T> : MultiOption<T>, IHtmlElement
    //{
    //    public LabelField Label { get; set; }

    //    public LabelRBCBControl(string id, string label):base(id)
    //    {
    //        this.Label = new LabelField(label, true);
    //        Options = new List<T>();
    //        if (typeof(T) == typeof(LabelCheckBoxField))
    //            GCType = GroupControlType.CheckBoxes;
    //        else if (typeof(T) == typeof(LabelRadioButtonField))
    //            GCType = GroupControlType.RadioButtons;
    //        else
    //            throw new Exception("MS: Unknown MultiOption control in LabelRBCBControl.");
    //    }

    //    public static Field GetRandom(string id, int count)
    //    {
    //        LabelRBCBControl<T> n = new LabelRBCBControl<T>(id, "control global label");
    //        for (int i = 0; i < count; i++)
    //        {
    //            string Name = typeof(T).ToString() + DateTime.Now.ToLongTimeString();
    //            //          string Name, string Label, string IndividualValueId, bool Checked = false, bool Disabled = false
    //            T aa = (T)Activator.CreateInstance(typeof(T), Name, typeof(T).ToString() + i.ToString(), typeof(T).ToString() + i.ToString(), false, false);
    //            n.Options.Add(aa);
    //        }
    //        return n;
    //    }

    //    public TagBuilder BuildHtmlTag()
    //    {
    //        TagBuilder tb = base.BuildBaseHtmlTag();
    //        tb.AddCssClass("LabelControlDuo");

    //        TagBuilder tbControl = new TagBuilder("div");

    //        if (GCType == GroupControlType.CheckBoxes)
    //            tbControl.AddCssClass("GroupOfCheckBoxes");
    //        else if (GCType == GroupControlType.RadioButtons)
    //            tbControl.AddCssClass("GroupOfRadioButtons");
    //        else
    //            return new TagBuilder("error");

    //        //TagBuilder tbForm = new TagBuilder("form");
    //        //tbForm.Attributes.Add("action", "#");
    //        //tbForm.Attributes.Add("method", "get");

    //        for (int i = 0; i < Options.Count; i++)
    //        {
    //            TagBuilder t = (TagBuilder) Options[i].GetType().GetMethod("BuildHtmlTag").Invoke(Options[i], null);
    //            //tbForm.InnerHtml.AppendHtml(t);
    //            tbControl.InnerHtml.AppendHtml(t);
    //        }

    //        //tbControl.InnerHtml.AppendHtml(tbForm);

    //        tb.InnerHtml.AppendHtml(Label.BuildHtmlTag());
    //        tb.InnerHtml.AppendHtml(tbControl);
    //        return tb;
    //    }

    //    //public override string GetValue()
    //    //{
    //    //    return "";
    //    //}
    //}



}