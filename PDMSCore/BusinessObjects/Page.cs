using PDMSCore.DataManipulation;
using System.Data;

namespace PDMSCore.BusinessObjects
{
    public class Page
    {
        public Menu SideMenu { get; set; }
        public Panels Panels { get; set; }

        public Page()
        {
            SideMenu = new Menu();
            Panels = new Panels();
        }


        public void ProcessPageInfo(DataTable dt)
        {

            foreach (DataRow existingRow in dt.Rows)
            {
                //Console.WriteLine(existingRow.ItemArray[0].ToString() + " - " + existingRow.ItemArray[1].ToString());
            }
        }
    }
}
