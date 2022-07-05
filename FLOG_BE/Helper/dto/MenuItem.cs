using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Helper.dto
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Group { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
        public bool IsVisible { get; set; }
        public bool Divider { get; set; }
        public List<MenuItem> Items { get; set; }
    }
}
