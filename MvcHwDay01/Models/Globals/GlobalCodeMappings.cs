using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcHwDay01.Models
{
    public static class GlobalCodeMappings
    {
        public static IDictionary<int, string> BillTypes = new Dictionary<int, string>() {
            { -1, "請選擇 ..." },
            { 1, "支出"},
            { 2, "收入"},
        };
    }
}