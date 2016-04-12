using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcHwDay01.Models
{
    /// <summary>
    /// 帳單歷史
    /// </summary>
    /// <remarks>
    /// 每個使用者有多筆帳單資料
    /// </remarks>
    public static class BillingHistory
    {
        public static Dictionary<string, IEnumerable<BillingItemViewModel>> Data { get; set; }
    }
}