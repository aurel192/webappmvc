using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbConnectionClassLib.ResponseClasses
{
    public class DropDownTreeItem
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string text { get; set; }
        public bool hasChild { get; set; } = false;
        public bool expanded { get; set; } = false;
    }
}
