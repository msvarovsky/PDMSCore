﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static PDMSCore.DataManipulation.WebStuffHelper;

namespace PDMSCore.DataManipulation
{
    public enum RowType
    {
        Header,
        Data
    }
    public enum ColumnType
    {
        Unknown,
        Text,
        CheckBox
    }

    public class TableColumn
    {
        public string Label { get; set; }
        public bool ReadOnly { get; set; }
        public int MinColumnWidtg { get; set; }
        public bool Visible { get; set; } = true;
        public ColumnType Type { get; set; } = ColumnType.Unknown;


        public TableColumn(string Label, bool ReadOnly = false)
        {
            this.Label = Label;
            this.ReadOnly = ReadOnly;
        }

    }

    public class DataGridField : Field
    {
        public string ID { get; set; }
        public int nVisibleRows { get; set; }
        public string FocusControlID { get; set; }
        private DataTable SourceData { get; set; }
        private bool[] StayAfterFiltering { get; set; }
        public int DbTableUniqueIDColumnNumber { get; set; } = -1;
        //public ModalDialog AddRowDialog { get; set; }
        public List<TableColumn> Columns;
        //private List<TableRow> Data;
        public string ParentControllerAndAction { get; set; }
        public string HTMLHeaderID { get { return ID + "-h"; } }
        public string HTMLBodyID { get { return ID + "-b"; } }
        //public int RowCount { get { return Data.Count; } }
        public int RowCount { get { return SourceData.Rows.Count; } }
        //public List<Field> Menu { get; set; }
        public DataGridFieldMenus Menu { get; set; }

        public DataGridField(string ID, string ParentControllerAndAction) : base("TODO", "GridTable", "table", null)
        {
            Init(ID, ParentControllerAndAction);
        }
        public DataGridField(string ID, string ParentControllerAndAction, DataTable dt) : base("TODO", "GridTable", "table", null)
        {
            Init(ID, ParentControllerAndAction);
            //SetHeader(dt);
            SourceData = dt;
            //SetData(dt);
            GetColumnInfo();
        }
        private void Init(string ID, string ParentCtrlAndAction)
        {
            //this.ColumnReadOnly = new bool[ColumnReadOnly.Length];
            //for (int i = 0; i < ColumnReadOnly.Length; i++)
            //    this.ColumnReadOnly[i] = ColumnReadOnly[i];

            ParentControllerAndAction = ParentCtrlAndAction;
            this.ID = ID;
            //HeaderLabels = null;
            Columns = new List<TableColumn>();
            //Data = new List<TableRow>();
            nVisibleRows = 5;
            Menu = new DataGridFieldMenus(ID, ParentCtrlAndAction);


        }



        private void GetColumnInfo()
        {
            Columns = new List<TableColumn>();
            for (int i = 0; i < SourceData.Columns.Count; i++)
                Columns.Add(new TableColumn(SourceData.Columns[i].Caption));
        }

        private string[] GetColumnToStringArray(DataTable dt)
        {
            string[] ret = new string[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                ret[i] = dt.Columns[i].Caption;
            return ret;
        }

        public DataRow GetRow(int row)
        {
            //return Data[row];
            return SourceData.Rows[row];
        }

        public Field GetRandom()
        {
            return null;
        }

        public string Get(int row, int column)
        {
            return SourceData.Rows[row].ItemArray[column].ToString();   //  TODO: ToString je spatne. Mel bych take myslet na Checkbox,... .
        }

        private Field CreateFieldFromSourceData(int r, int c)
        {
            string CellID = ID + "-r" + r + "c" + c;
            Field f = new Field("", "", "", "");
            if (Columns[c].ReadOnly)
                if (Columns[c].Type == ColumnType.CheckBox)
                    f = new CheckBoxField("", CellID, "", SourceData.Rows[r].ItemArray[c].ToString().Trim() == "TRUE" ? true : false, false);
                else
                    f = new LabelField(SourceData.Rows[r].ItemArray[c].ToString().Trim());
            else
            {
                switch (Columns[c].Type)
                {
                    case ColumnType.Unknown:
                    case ColumnType.Text:
                        f = new TextBoxField(CellID, "dbfieldid", "DGCellTB", SourceData.Rows[r].ItemArray[c].ToString().Trim());
                        break;
                    case ColumnType.CheckBox:
                        f = new CheckBoxField("", CellID, "", SourceData.Rows[r].ItemArray[c].ToString().Trim() == "TRUE" ? true : false, false);
                        break;
                    default:
                        break;
                }
            }
            string DBFieldID = SourceData.Rows[r].ItemArray[DbTableUniqueIDColumnNumber].ToString();
            f.DBFieldID = DBFieldID;

            return f;
        }

        public List<DBTableUpdateTrio> GetDifferences(Dictionary<string, string> CliendDG)
        {
            List<DBTableUpdateTrio> ret = new List<DBTableUpdateTrio>();
            int DBFieldID = -1;
            string NewClientValue = "";

            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < SourceData.Rows[r].ItemArray.Length; c++)
                {
                    if (!Columns[c].ReadOnly)
                    {
                        Field f = CreateFieldFromSourceData(r, c);
                        if (CliendDG.TryGetValue(f.HTMLFieldID, out NewClientValue))
                        {
                            if (f.GetValue() == NewClientValue.ToString())
                                continue;
                            else
                            {
                                KeyValuePair<string, string> UpdatingValue = new KeyValuePair<string, string>(Columns[c].Label, NewClientValue);
                                if (Int32.TryParse(f.DBFieldID, out DBFieldID))
                                {
                                    DBTableUpdateTrio u = new DBTableUpdateTrio(DBFieldID, UpdatingValue);
                                    ret.Add(u);
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public void SetData()
        {
            for (int r = 0; r < SourceData.Rows.Count; r++)
            {
                TableRow tr = SetDataLabelsRow(r, SourceData.Rows[r]);
                //AddDataRow(tr, r);
                AddDataRow(SourceData.Rows[r], r);
            }
        }

        private TableRow SetDataLabelsRow(int r, DataRow dr)
        {
            TableRow tr = new TableRow();
            Field f;
            for (int c = 0; c < dr.ItemArray.Length; c++)
            {
                if (Columns[c].ReadOnly)
                    f = new LabelField(dr.ItemArray[c].ToString().Trim());
                else
                    f = new TextBoxField(ID + "-r" + r + "c" + c, "dbfieldid", "DGCellTB", dr.ItemArray[c].ToString().Trim());
                tr.AddColumnCell(f);
            }
            return tr;
        }

        public static DataGridField GetTestData(int ID)
        {
            DataGridField d = new DataGridField("test", null);
            d.ID = ID.ToString();
            d.SetHeaderLabels("Jmeno", "Prijmeni", "Aktivni");


            //TableRow tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Martin-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("Svarovsky"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr, 1);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Martin-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("SpatnePrijmeni"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Cecile-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("Svarovska"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Jitka-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("Svarovska"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Astrid-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("Svarovska"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Lubos-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("Svarovsky"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Dominique-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("Champagne"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Nadine-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("Champagne"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Thomas-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("Champagne"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);

            //tr = new TableRow();
            //tr.AddColumnCell(new LabelField("Noemie-" + DateTime.Now.Second));
            //tr.AddColumnCell(new LabelField("JeNeSaisPas"));
            //tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            //d.AddDataRow(tr);


            return d;
        }

        public void SetHeader(DataTable dt)
        {
            string[] HeaderLabels = GetColumnToStringArray(dt);
            SetHeaderLabels(HeaderLabels);
        }
        public void SetHeaderLabels(params string[] HeaderLabels)
        {
            for (int c = 0; c < HeaderLabels.Length; c++)
            {
                TableColumn tc = new TableColumn(HeaderLabels[c]);
                Columns.Add(tc);
            }
            FocusControlID = "filter-" + Columns[0].Label;
            return;
        }

        //public bool AddDataRow(TableRow tr, int? id = null)
        public bool AddDataRow(DataRow dr, int? id = null)
        {
            if (Columns == null)
                throw new Exception("Table header labels are not defined.");

            //tr.ID = (id == null) ? RowCount+1 : (int)id;

            SourceData.Rows.Add(dr);
            return true;
        }

        private string[] ToLowerCase(string[] f)
        {
            string[] ret = new string[f.Length];

            for (int i = 0; i < f.Length; i++)
            {
                if (f[i] == null)
                    continue;
                ret[i] = f[i].ToLower();
            }

            return ret;
        }

        //public void ApplyFilters(string[] filters, FilteringType ft )
        //{
        //    if (filters.Length == 0)
        //        return;
        //    filters = ToLowerCase(filters);

        //    for (int r = 0; r < Data.Count; r++)
        //    {
        //        if (!Data[r].DoesRowComplyWithFilters(filters, ft))
        //        {
        //            Data.RemoveAt(r);   //  TODO: Toto je mozna velice casove narocne. Take bych nehodici se polozky oznacit jako deactive a potom je nepouzivat.
        //            r--;
        //        }
        //    }
        //}
        public void ApplyFilters(string[] filters, FilteringType ft)
        {
            StayAfterFiltering = new bool[SourceData.Rows.Count];

            if (filters.Length == 0)
                return;
            filters = ToLowerCase(filters);

            for (int r = 0; r < SourceData.Rows.Count; r++)
            {
                if (!DoesRowComplyWithFilters(SourceData.Rows[r], filters, ft))
                {
                    StayAfterFiltering[r] = false;
                    //SourceData.Rows.RemoveAt(r);    //  TODO: Toto je mozna velice casove narocne. Take bych nehodici se polozky oznacit jako deactive a potom je nepouzivat.
                    //r--;
                }
                else
                    StayAfterFiltering[r] = true;
            }
        }
        public bool DoesRowComplyWithFilters(DataRow dr, string[] filter, FilteringType ft)
        {
            if (ft == FilteringType.StartWith)
            {
                for (int c = 0; c < dr.ItemArray.Length; c++)
                {
                    if (filter[c] == null || filter[c] == "")
                        continue;
                    else
                    {
                        switch (Columns[c].Type)
                        {
                            case ColumnType.Unknown:
                            case ColumnType.Text:
                                if (dr.ItemArray[c].ToString().ToLower().IndexOf(filter[c]) != 0)
                                    return false;
                                break;
                            case ColumnType.CheckBox:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return true;
        }

        //public TagBuilder HtmlTextHeaderRow()
        //{
        //    TableRow tr = Data[0];

        //    TagBuilder tbTr = new TagBuilder("tr");

        //    TagBuilder tbID = new TagBuilder("th");
        //    tbID.Attributes.Add("style", "display:none");
        //    tbTr.InnerHtml.AppendHtml(tbID);

        //    for (int c = 0; c < tr.Cells.Count; c++)
        //    {
        //        TagBuilder tbTh = new TagBuilder("th");
        //        TagBuilder tbDiv = new TagBuilder("div");
        //        tbDiv.AddCssClass("HeadCell");
        //        TagBuilder tbHl = new TagBuilder("HeaderLabel");

        //        string colID;
        //        if (c > Columns.Count)
        //            colID = "{Not defined}";
        //        else
        //            colID = Columns[c].Label;

        //        tbHl.InnerHtml.AppendHtml(colID);

        //        if (!Columns[c].Visible)
        //        {
        //            tbTh.Attributes.Add("style", "display:none");
        //        }

        //        //  Pouze kdyz se sloupec zobrazuje, tak potrebuju vsechno toto. Jinak ne.
        //        //  UPDATE: Tak nakonec tam toto take necham, i kdyz ty sloupce budou schovane. Filtrovani pouziva "filter-..." atribut a bez toho tagu ho nenajde.
        //        TagBuilder tbHs = new TagBuilder("HeaderSearch");
        //        if (tr.Cells[c].GetType() == typeof(CheckBoxField))
        //        {
        //            DropDownListBox ddf = new DropDownListBox("todo", "filter-ddf-1", 1);
        //            ddf.jsOnInputFunction = "OnDataGridFilterChange()";
        //            ddf.Add("", "(All)");
        //            ddf.Add("1", "Yes");
        //            ddf.Add("-1", "No");
        //            ddf.Classes.Add("filter");
        //            tbHs.InnerHtml.AppendHtml(ddf.BuildHtmlTag());
        //        }
        //        else
        //        {
        //            TagBuilder tbInput = new TagBuilder("input");
        //            tbInput.Attributes.Add("type", "text");
        //            tbInput.Attributes.Add("placeholder", "...");
        //            tbInput.Attributes.Add("id", "filter-" + colID);
        //            tbInput.Attributes.Add("oninput", "OnDataGridFilterChange(\'filter-" + colID + "\')");
        //            tbInput.AddCssClass("filter");
        //            tbHs.InnerHtml.AppendHtml(tbInput);
        //        }
        //        tbDiv.InnerHtml.AppendHtml(tbHs);
        //        ////////

        //        tbDiv.InnerHtml.AppendHtml(tbHl);
        //        tbTh.InnerHtml.AppendHtml(tbDiv);
        //        tbTr.InnerHtml.AppendHtml(tbTh);
        //    }
        //    return tbTr;
        //}
        public TagBuilder HtmlTextHeaderRow()
        {
            //TableRow tr = Data[0];

            TagBuilder tbTr = new TagBuilder("tr");
            TagBuilder tbID = new TagBuilder("th");
            tbID.Attributes.Add("style", "display:none");
            tbTr.InnerHtml.AppendHtml(tbID);

            for (int c = 0; c < Columns.Count; c++)
            {
                TagBuilder tbTh = new TagBuilder("th");
                TagBuilder tbDiv = new TagBuilder("div");
                tbDiv.AddCssClass("HeadCell");
                TagBuilder tbHl = new TagBuilder("HeaderLabel");
                tbHl.InnerHtml.AppendHtml(Columns[c].Label);

                if (!Columns[c].Visible)
                {
                    tbTh.Attributes.Add("style", "display:none");
                }

                //  Pouze kdyz se sloupec zobrazuje, tak potrebuju vsechno toto. Jinak ne.
                //  UPDATE: Tak nakonec tam toto take necham, i kdyz ty sloupce budou schovane. Filtrovani pouziva "filter-..." atribut a bez toho tagu ho nenajde.
                TagBuilder tbHs = new TagBuilder("HeaderSearch");
                if (Columns[c].Type == ColumnType.CheckBox)
                {
                    DropDownListBox ddf = new DropDownListBox("todo", "filter-ddf-1", 1);
                    ddf.jsOnInputFunction = "OnDataGridFilterChange()";
                    ddf.Add("", "(All)");
                    ddf.Add("1", "Yes");
                    ddf.Add("-1", "No");
                    ddf.Classes.Add("filter");
                    tbHs.InnerHtml.AppendHtml(ddf.BuildHtmlTag());
                }
                else
                {
                    TagBuilder tbInput = new TagBuilder("input");
                    tbInput.Attributes.Add("type", "text");
                    tbInput.Attributes.Add("placeholder", "...");
                    tbInput.Attributes.Add("id", "filter-" + Columns[c].Label);
                    tbInput.Attributes.Add("oninput", "OnDataGridFilterChange(\'filter-" + Columns[c].Label + "\')");
                    tbInput.AddCssClass("filter");
                    tbHs.InnerHtml.AppendHtml(tbInput);
                }
                tbDiv.InnerHtml.AppendHtml(tbHs);
                ////////

                tbDiv.InnerHtml.AppendHtml(tbHl);
                tbTh.InnerHtml.AppendHtml(tbDiv);
                tbTr.InnerHtml.AppendHtml(tbTh);
            }
            return tbTr;
        }

        //public TagBuilder HtmlTextTableBody(object filters=null)
        //{
        //    TagBuilder tbTableBody = new TagBuilder("tbody");
        //    //tbTableBody.Attributes.Add("id", "dg-" + this.HTMLFieldID + "-b");
        //    tbTableBody.Attributes.Add("id", this.HTMLBodyID);

        //    for (int i = 0; i < Data.Count; i++)
        //        tbTableBody.InnerHtml.AppendHtml(Data[i].HtmlText(Columns));

        //    return tbTableBody;
        //}
        public TagBuilder HtmlTextTableBody(object filters = null)
        {
            TagBuilder tbTableBody = new TagBuilder("tbody");
            tbTableBody.Attributes.Add("id", this.HTMLBodyID);


            if (StayAfterFiltering == null)
            {
                for (int r = 0; r < SourceData.Rows.Count; r++)
                    tbTableBody.InnerHtml.AppendHtml(HtmlTextOfTableBodyRow(SourceData.Rows[r], r));
            }
            else
            {
                for (int r = 0; r < SourceData.Rows.Count; r++)
                {
                    if (StayAfterFiltering[r])
                        tbTableBody.InnerHtml.AppendHtml(HtmlTextOfTableBodyRow(SourceData.Rows[r], r));
                }
            }

            return tbTableBody;
        }
        private TagBuilder HtmlTextOfTableBodyRow(DataRow dr, int r)
        {
            TagBuilder tbTr = new TagBuilder("tr");
            TagBuilder tbID = new TagBuilder("td");
            tbID.Attributes.Add("style", "display:none");
            tbID.InnerHtml.AppendHtml(ID.ToString());

            tbTr.InnerHtml.AppendHtml(tbID);

            for (int c = 0; c < dr.ItemArray.Length; c++)
            {
                TagBuilder tbTd = new TagBuilder("td");
                TagBuilder tbDiv = new TagBuilder("div");
                tbDiv.AddCssClass("Cell");

                Field f = CreateFieldFromSourceData(r, c);

                /*string CellID = ID + "-r" + r + "c" + c;

                Field f = new Field("", "", "", "");
                if (Columns[c].ReadOnly)
                    if (Columns[c].Type == ColumnType.CheckBox)
                        f = new CheckBoxField("", CellID, "", dr.ItemArray[c].ToString().Trim() == "TRUE" ? true : false, false);
                    else
                        f = new LabelField(dr.ItemArray[c].ToString().Trim());
                else
                {
                    switch (Columns[c].Type)
                    {
                        case ColumnType.Unknown:
                        case ColumnType.Text:
                            f = new TextBoxField(CellID, "dbfieldid", "DGCellTB", dr.ItemArray[c].ToString().Trim());
                            break;
                        case ColumnType.CheckBox:
                            f = new CheckBoxField("", CellID, "", dr.ItemArray[c].ToString().Trim() == "TRUE" ? true : false, false);
                            break;
                        default:
                            break;
                    }
                }*/

                tbDiv.InnerHtml.AppendHtml(((IHtmlElement)f).BuildHtmlTag());
                //tbDiv.InnerHtml.AppendHtml(((IHtmlElement)Cells[c]).BuildHtmlTag());

                tbTd.InnerHtml.AppendHtml(tbDiv);

                if (!Columns[c].Visible)    //  Other then the below, there's nothing to do in order to minimize the amounth of data transfered.
                    tbTd.Attributes.Add("style", "display:none");

                tbTr.InnerHtml.AppendHtml(tbTd);
            }
            return tbTr;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.Attributes.Add("id", "GridTable");
            tb.AddCssClass("GridTable");

            tb.InnerHtml.AppendHtml(HtmlTextHeaderRow());

            TagBuilder tbDiv = new TagBuilder("div");
            tbDiv.Attributes.Add("id", "GridTableContent");

            tbDiv.InnerHtml.AppendHtml(HtmlTextTableBody());
            //for (int i = 0; i < Data.Count; i++)
            //    tbDiv.InnerHtml.AppendHtml(Data[i].HtmlText());

            tb.InnerHtml.AppendHtml(tbDiv);

            return tb;
        }

        public TagBuilder HtmlTextMenu()
        {
            return null;
        }

        //public string GetPresentableStringFromID(string id)
        //{
        //    int iid;
        //    if (Int32.TryParse(id, out iid))
        //    {
        //        TableRow tr2 = Data[iid - 1];
        //        return tr2.Cells[0].GetValue() + " " + tr2.Cells[1].GetValue();
        //    }
        //    return null;
        //}
    }



    public abstract class DataGridFieldMenuItem
    {
        abstract public TagBuilder BuildHtmlMenuButton(string ID);
        abstract public TagBuilder BuildHtmlMenuDialog(string ID, string ParentControllerAndAction);
        abstract public ModalDialog GetModalDialogObject();
        abstract public void UpdateID(string id);
        virtual public string WebName { get; set; }
        virtual public string ID { get; set; }
    }

    public class DataGridFieldAddMenuItem: DataGridFieldMenuItem
    {
        public ModalDialog AddRowDialog { get; set; }

        public DataGridFieldAddMenuItem(string DialogTitle, string DialogDescription)
        {
            Init();
            AddRowDialog = new ModalDialog(ID + "-" + WebName, "no need", DialogTitle, DialogDescription);
        }

        public DataGridFieldAddMenuItem()
        {
            Init();
            AddRowDialog = new ModalDialog(ID + "-" + WebName);
        }

        private void Init()
        {
            WebName = "AddMenu";
        }

        public override TagBuilder BuildHtmlMenuButton(string ID)
        {
            // <button type = "button" onclick = "AddRowModal(100)" >< i class="fa fa-plus"></i> Add</button>
            //< button type = "button" title = "New label" >< img src = "~/images/Plus2-24x24.png" alt = "Save" /></ button >
            

            TagBuilder tbButton = new TagBuilder("button");
            tbButton.Attributes.Add("type", "button");
            tbButton.Attributes.Add("onclick", "AddRowModal(100)");

            TagBuilder tbImg = new TagBuilder("img");
            tbImg.Attributes.Add("src", "/images/Plus2-24x24.png");
            tbImg.Attributes.Add("alt", "Add");
            tbButton.InnerHtml.AppendHtml(tbImg);

            //TagBuilder tbI = new TagBuilder("i");
            //tbI.AddCssClass("fa fa-plus");
            //tbButton.InnerHtml.AppendHtml(tbI);

            return tbButton;
        }

        public override TagBuilder BuildHtmlMenuDialog(string ID, string ParentControllerAndAction)
        {
            AddRowDialog = new ModalDialog(ID + "-AddRowModal", "", "Titulek");
            AddRowDialog.ParentControllerAndAction = ParentControllerAndAction;
            AddRowDialog.OnOk = "ReloadDataGridAndScrollToBottom()";
            return AddRowDialog.BuildDialogContent();
        }

        public override ModalDialog GetModalDialogObject()
        {
            return AddRowDialog;
        }

        public override void UpdateID(string id)
        {
            ID = id;
            AddRowDialog.ID = id + "-" + WebName;
        }
    }

    public class DataGridFieldSaveMenuItem : DataGridFieldMenuItem
    {
        public DataGridFieldSaveMenuItem()
        {
            WebName = "SaveMenu";
        }

        public override TagBuilder BuildHtmlMenuButton(string ID)
        {
            //< button type = "button" title = "New label" >< img src = "~/images/Plus2-24x24.png" alt = "Save" /></ button >

            TagBuilder tb = new TagBuilder("span");

            TagBuilder tbButtonSave = new TagBuilder("button");
            tbButtonSave.Attributes.Add("id", ID + "-SaveMenuBtn");
            tbButtonSave.Attributes.Add("title","Save");


            TagBuilder tbImgSave = new TagBuilder("img");
            tbImgSave.Attributes.Add("src", "/images/Save-24x24.png");
            tbImgSave.Attributes.Add("alt", "Save");
            tbButtonSave.InnerHtml.AppendHtml(tbImgSave);


            //<button type = "button" id="@Model.ID-SavedMenuBtn" style="width:100px; background-color: limegreen; border-color: limegreen; display: none;"><i class="fa fa-save"></i> Saved</button>
            TagBuilder tbButtonSaved = new TagBuilder("button");
            tbButtonSaved.Attributes.Add("type", "button");
            tbButtonSaved.Attributes.Add("id", ID + "-SavedMenuBtn");
            tbButtonSaved.Attributes.Add("style", "display: none;");

            TagBuilder tbImgSaved = new TagBuilder("img");
            tbImgSaved.Attributes.Add("src", "/images/Saved-24x24.png");
            tbImgSaved.Attributes.Add("alt", "Save");
            tbButtonSaved.InnerHtml.AppendHtml(tbImgSaved);


            tb.InnerHtml.AppendHtml(tbButtonSave);
            tb.InnerHtml.AppendHtml(tbButtonSaved);

            return tb;
        }

        public override TagBuilder BuildHtmlMenuDialog(string ID, string ParentControllerAndAction)
        {
            return null;
        }

        public override ModalDialog GetModalDialogObject()
        {
            return null;
        }

        public override void UpdateID(string id)
        {
            ID = id;
        }
    }


    public class DataGridFieldMenus
    {
        public string DataGridID { get; set; }
        public string DataGridParentControllerAndAction { get; set; }

        private List<DataGridFieldMenuItem> items;
        
        public DataGridFieldMenus(string ID, string ParentControllerAndAction)
        {
            DataGridID = ID;
            DataGridParentControllerAndAction = ParentControllerAndAction;
            items = new List<DataGridFieldMenuItem>();
        }

        public void AddMenu(DataGridFieldMenuItem item)
        {
            item.UpdateID(DataGridID);
            items.Add(item);
        }

        public ModalDialog GetModalDialog(string LookingFor)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].WebName.ToLower() == LookingFor.ToLower())
                    return items[i].GetModalDialogObject();
            }
            return null;
        }

        public bool Exists(string LookingFor)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].WebName == LookingFor)
                    return true;
            }
            return false;
        }

        public TagBuilder BuildHtmlMenuButtons()
        {
            TagBuilder tb = new TagBuilder("span");

            for (int i = 0; i < items.Count; i++)
            {
                //tb.InnerHtml.AppendHtml(items[i].BuildHtmlMenuButtons(DataGridID, DataGridParentControllerAndAction));
                tb.InnerHtml.AppendHtml(items[i].BuildHtmlMenuButton(DataGridID));
            }

            return tb;
        }
        public TagBuilder BuildHtmlMenuDialogs()
        {
            TagBuilder tb = new TagBuilder("div");
            for (int i = 0; i < items.Count; i++)
            {
                tb.InnerHtml.AppendHtml(items[i].BuildHtmlMenuDialog(DataGridID, DataGridParentControllerAndAction));
            }

            return tb;
        }
    }

    public enum FilteringType
    {
        StartWith,
        Contain,
        ContainWithAsterisks
    }

    public class TableRow: IHtmlTag
    {
        public List<Field> Cells { get; set; }
        public int ID { get; set; }

        public TableRow()
        {
            Cells = new List<Field>();
        }

        public bool DoesRowComplyWithFilters(string[] filter, FilteringType ft)
        {
            if (ft == FilteringType.StartWith)
            {
                for (int c = 0; c < Cells.Count; c++)
                {

                    if (filter[c] == null || filter[c] == "")
                        continue;
                    else
                    {
                        if (Cells[c].GetValue().ToLower().IndexOf(filter[c]) != 0)
                            return false;
                    }
                }
            }
            return true;
        }

        public void AddColumnCell(Field f)
        {
            Cells.Add(f);
        }

        public TagBuilder HtmlText()
        {
            return HtmlText(null);
        }
        public TagBuilder HtmlText(List<TableColumn> Columns)
        {
            TagBuilder tbTr = new TagBuilder("tr");

            TagBuilder tbID = new TagBuilder("td");
            tbID.Attributes.Add("style", "display:none");
            tbID.InnerHtml.AppendHtml(ID.ToString());

            tbTr.InnerHtml.AppendHtml(tbID);

            for (int c = 0; c < Cells.Count; c++)
            {
                TagBuilder tbTd = new TagBuilder("td");
                TagBuilder tbDiv = new TagBuilder("div");
                tbDiv.AddCssClass("Cell");

                tbDiv.InnerHtml.AppendHtml(((IHtmlElement)Cells[c]).BuildHtmlTag());
                tbTd.InnerHtml.AppendHtml(tbDiv);


                if (!Columns[c].Visible)    //  Other then the below, there's nothing to do in order to minimize the amounth of data transfered.
                    tbTd.Attributes.Add("style", "display:none");

                tbTr.InnerHtml.AppendHtml(tbTd);
            }
            return tbTr;
        }

        public TableRow MakeCopy()
        {
            TableRow n = new TableRow();
            n.ID = this.ID;
            n.Cells = new List<Field>();

            for (int i = 0; i < this.Cells.Count; i++)
                n.Cells.Add(this.Cells[i]);

            return n;
        }
    }
    

    //public class TableColumns
    //{
    //    public List<OneColumnInfo> column;
    //    //ColumnType type, Boolean ro = false)

    //    public TableColumns()
    //    {
    //        column = new List<OneColumnInfo>();
    //    }

    //    public void Add(ColumnType type, Boolean readOnly)
    //    {
    //        column.Add(new OneColumnInfo(type, readOnly));

    //    }
    //}
    //public struct OneColumnInfo
    //{
    //    public ColumnType Type { get; private set; }
    //    public Boolean ReadOnly { get; private set; }

    //    public OneColumnInfo(ColumnType type, Boolean readOnly)
    //    {
    //        Type = type;
    //        ReadOnly = readOnly;
    //    }
    //}

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    

    public class TableCell : IHtmlTag
    {
        RowType type;
        public string HtmlLabel { get; set; }
        private Field CellField { get; set; }

        public TableCell(RowType Type, Field f, string HeaderText = null)
        {
            HtmlLabel = HeaderText;
            this.type = Type;
            this.CellField = f;
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");

            if (type == RowType.Header)
            {
                tb.AddCssClass("HeadCell");

                TagBuilder tbHeaderLabel = new TagBuilder("HeaderLabel");
                tbHeaderLabel.InnerHtml.AppendHtml(HtmlLabel);

                TagBuilder tbHeaderSearch = new TagBuilder("HeaderSearch");
                TextBoxField t = new TextBoxField("TODO-NameId", "TableTextBoxHeader AutoComplete", "", "...");
                tbHeaderSearch.AddCssClass("HeaderSrchCl");
                tbHeaderSearch.InnerHtml.AppendHtml(t.BuildHtmlTag());

                tb.InnerHtml.AppendHtml(tbHeaderLabel);
                tb.InnerHtml.AppendHtml(tbHeaderSearch);
            }
            else if (type == RowType.Data)
            {
                tb.AddCssClass("Cell");
                tb.InnerHtml.AppendHtml(CellField.BuildBaseHtmlTag());
            }
            else
                throw new NotImplementedException();

            return tb;
        }
    }
    
}
