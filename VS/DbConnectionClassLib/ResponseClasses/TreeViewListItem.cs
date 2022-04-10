using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnectionClassLib.ResponseClasses
{
    public class TreeViewListItem
    {
        public string id { get; set; }
        public string text { get; set; }
        public bool expanded { get; set; } = false;
        public bool selected { get; set; } = false;
        public List<TreeViewListItem> child { get; set; }
        public int instumentType { get; set; }
    }
}
