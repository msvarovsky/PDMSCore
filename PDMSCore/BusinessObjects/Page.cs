using PDMSCore.DataManipulation;
using System.Data;

namespace PDMSCore.BusinessObjects
{
    public class Page
    {
        public Menu SideMenu { get; set; }
        public Panels Panels { get; set; }
        public string Title { get; set; }

        public Page()
        {
            SideMenu = new Menu();
            Panels = new Panels();
        }

        public void ProcessPageInfo(DataTable dt)
        {
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                Title = DBUtil.GetString(dt.Rows[r], 1);
            }
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
