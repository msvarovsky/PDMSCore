using PDMSCore.DataManipulation;
using System.Data;

namespace PDMSCore.BusinessObjects
{
    public class Page
    {
        public Menu SideMenu { get; set; }
        public Panels Panels { get; set; }
        public string Title { get; set; }
        public string url { get; set; }     //  Zatim nevim, jestli bude toto k necemu vubec uzitecne.

        public Page()
        {
            SideMenu = new Menu();
            Panels = new Panels();
        }

        public void ProcessPageInfo(DataTable dt)
        {
            Title = DBUtil.GetString(dt.Rows[0], 1);
            url = DBUtil.GetString(dt.Rows[0], 2);
        }
        public void GenerateUnknownPageInfo()
        {
            Title = "Unknown";
            url = null;
        }


        public void ProcessPanelsInfo(DataTable dt)
        {
            Panels.ProcessPanelsInfo(dt);
        }

        public void ProcessFieldsInfo(DataTable dt)
        {
            Panels.ProcessFieldsInfo(dt);
        }

    }
}
