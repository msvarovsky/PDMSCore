using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.DataManipulation
{
    public class AsyncPanel
    {
        public int id { get; set; }
        public string Size { get; set; }
        public string Label { get; set; }
        public List<Field> Content { get; set; }
        public PanelMenu menu { get; set; }

        public AsyncPanel()
        {
          
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("a");
            tb.Attributes.Add("href", "TraLaLa");
            
            return tb;
        }
    }
}
