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
            Project pr = new Project();

            pr.LoadProjectFromDB(new GeneralSessionInfo(1,1,"en"),1,1);

            Panel p = new Panel(1,"ahoj", 2);

            //p.Load();


        }
    }
}
