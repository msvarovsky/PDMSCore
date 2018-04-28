﻿using Microsoft.AspNetCore.Mvc.Rendering;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;

namespace PDMSCore.BusinessObjects
{
    public enum RowType
    {
        Header,
        Data
    }
    public enum ColumnType
    {
        Label,
        Text,
        CheckBox
    }

    public class DataGridField2 : Field
    {
        private TableColumns Columns;
        //private List<TableColumn2> Columns;

        public DataGridField2()
        {
            //Columns = new TableColumns();
        }

        public void DefineColumns(TableColumns tc)
        {
            Columns = tc;
        }

        public bool AddDataRow()
        {
            if (Columns == null)
                return false;



            return true;
        }

        public override TagBuilder HtmlText()
        {
            throw new NotImplementedException();
        }
    }

    public class TableColumns
    {
        public List<OneColumnInfo> column;
        //ColumnType type, Boolean ro = false)

        public TableColumns()
        {
            column = new List<OneColumnInfo>();
        }

        public void Add(ColumnType type, Boolean readOnly)
        {
            column.Add(new OneColumnInfo(type, readOnly));

        }
    }
    public struct OneColumnInfo
    {
        public ColumnType Type { get; private set; }
        public Boolean ReadOnly { get; private set; }

        public OneColumnInfo(ColumnType type, Boolean readOnly)
        {
            Type = type;
            ReadOnly = readOnly;
        }
    }

    /////////////////////////////////////////////////////////

    public class DataGridField : Field
    {
        private TableRow HeaderRow;
        private List<TableRow> DataRow;

        public DataGridField()
        {
            DataRow = new List<TableRow>();
        }

        public void SetHeaderRow(TableRow NewRow)
        {
            HeaderRow = NewRow;
        }
        public void AddDataRow(TableRow NewRow)
        {
            DataRow.Add(NewRow);
        }

        public static DataGridField GetRandom(string type="")
        {
            DataGridField n = new DataGridField();
            if (type == "")
            {
                int ColumnCount = 3;
                n.SetHeaderRow(TableRow.GetRandomHeaderRow(ColumnCount));
                for (int i = 0; i < 5; i++)
                    n.AddDataRow(TableRow.GetRandomDataRow(ColumnCount));
            }
            else if (type == "users")
            {
                int ColumnCount = 3;
                n.SetHeaderRow(TableRow.GetRandomHeaderRow(ColumnCount));
                for (int i = 0; i < 5; i++)
                    n.AddDataRow(TableRow.GetRandomDataRow(ColumnCount));

            }

            return n;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("table");
            tb.Attributes.Add("id", "GridTable");
            tb.AddCssClass("GridTable");
            tb.InnerHtml.AppendHtml(HeaderRow.HtmlText());

            for (int i = 0; i < DataRow.Count; i++)
                tb.InnerHtml.AppendHtml(DataRow[i].HtmlText());

            return tb;
        }
    }

    public class TableRow : IHtmlTag
    {
        private List<TableColumn> Columns;

        public TableRow()
        {
            Columns = new List<TableColumn>();
        }

        public void AddColumn(TableColumn NewColumn)
        {
            Columns.Add(NewColumn);
        }

        public static TableRow GetRandomHeaderRow(int count)
        {
            TableRow n = new TableRow();
            for (int i = 0; i < count; i++)
            {
                TableColumn t = new TableColumn(RowType.Header, new TableCell(RowType.Header, new LabelField("NameID-" + i), "NameID-" + i));
                n.AddColumn(t);
            }
            return n;
        }
        public static TableRow GetRandomDataRow(int count)
        {
            TableRow n = new TableRow();
            for (int i = 0; i < count; i++)
            {
                TableColumn t = new TableColumn(RowType.Data, new TableCell(RowType.Data, new TextBoxField("NameID-" + i, "TableTextBoxData", "NameID-" + i), "NameID-" + i));
                n.AddColumn(t);
            }
            return n;
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("tr");
            for (int i = 0; i < Columns.Count; i++)
                tb.InnerHtml.AppendHtml(Columns[i].HtmlText());
            return tb;
        }
    }

    public class TableColumn : IHtmlTag
    {
        public TableCell Cell;
        private RowType TableRowType;

        public TableColumn(RowType type, TableCell Cell = null)
        {
            TableRowType = type;
            this.Cell = Cell;
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tb;
            if (TableRowType == RowType.Header)
                tb = new TagBuilder("th");
            else if (TableRowType == RowType.Data)
                tb = new TagBuilder("td");
            else
                return null;


            if (Cell != null)
                tb.InnerHtml.AppendHtml(this.Cell.HtmlText());
            else
                tb.InnerHtml.AppendHtml("Not defined.");
            return tb;
        }
    }

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
                tbHeaderSearch.InnerHtml.AppendHtml(t.HtmlText());

                tb.InnerHtml.AppendHtml(tbHeaderLabel);
                tb.InnerHtml.AppendHtml(tbHeaderSearch);
            }
            else if (type == RowType.Data)
            {
                tb.AddCssClass("Cell");
                tb.InnerHtml.AppendHtml(CellField.HtmlText());
            }
            else
                throw new NotImplementedException();

            return tb;
        }
    }
    
}
