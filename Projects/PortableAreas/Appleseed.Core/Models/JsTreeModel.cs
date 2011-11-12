using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appleseed.Core.Models
{
    public class JsTreeModel
    {
        public string data;
        public JsTreeAttribute attr;
        public JsTreeModel[] children;
    }

    public class JsTreeAttribute
    {
        public string id;
        public bool selected;
    }
}