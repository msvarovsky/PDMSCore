using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using PDMSCore.BusinessObjects;
using PDMSCore.DataManipulation;
using PDMSCore.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace TestingConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, StringValues> d = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            FormCollection a = new FormCollection(d);
            GeneralSessionInfo gsi = new GeneralSessionInfo(1, 1, "en");

            Labels l = new Labels();
            l.LoadLabelsFromDB();





            Menu navigation = new Menu();
            navigation.LoadNavigation(new GeneralSessionInfo(1, 1, "en"));
            navigation.HtmlText();

            
            Project pr = new Project(1);
            pr.SavePage(gsi, 1, a); // asi bych ProjectID a PageID mel dodat ze stranky jako hidden fields.


            //pr.LoadProjectFromDB(new GeneralSessionInfo(1,1,"en"),1,1);

            Panel p = new Panel(1,"ahoj", 2);

            //p.Load();


        }
    }
}
