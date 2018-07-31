using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.DataManipulation
{
    public class PageMenu
    {
        public PageMenu()
        {

        }

        
        public TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("type", "button");
            tb.Attributes.Add("value", "Submit form");
            tb.Attributes.Add("onclick", "myFunction()");


            return tb;

        }

    }
}
