using Microsoft.AspNetCore.Mvc.Rendering;
using PDMSCore.BusinessObjects;
using PDMSCore.DataManipulation;
using System;

namespace TestingConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Menu m = new Menu();
            //m.GetRandomMenu();
            //m.HtmlText();

            DataGridField2 d = new DataGridField2();

            TableRow2 tr = new TableRow2();
            tr.AddColumnCell(new TextBoxField("", "", "Jmeno"));
            tr.AddColumnCell(new TextBoxField("", "", "Prijmeni"));
            tr.AddColumnCell(new CheckBoxField("", "", true, new WebTagAttributes(true, "")));

            d.SetHeaderLabels("Jmeno", "Prijmeni", "Aktivni");
            d.AddDataRow(tr);

            TagBuilder tb = d.HtmlText();


        }
    }
}
