using PDMSCore.Models;
using System.Collections.Generic;

namespace PDMSCore.DataManipulation
{
    public class Panel
    {
        public int id { get; set; }
        public string Size { get; set; }
        public string Label { get; set; }
        public List<Field> Content { get; set; }
    }
}