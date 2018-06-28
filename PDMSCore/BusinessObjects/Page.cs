using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.BusinessObjects
{
    public class Page
    {
        public Menu SideMenu { get; set; }


        public Page()
        {
            SideMenu = new Menu();
        }
    }
}
