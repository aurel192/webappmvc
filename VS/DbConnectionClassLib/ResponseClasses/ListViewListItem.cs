using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnectionClassLib.ResponseClasses
{
    public class ListViewListItem
    {
        public string id { get; set; }
        public string text { get; set; }
        public List<ListViewListItem> child { get; set; }
        public int instumentType { get; set; }
    }
}
