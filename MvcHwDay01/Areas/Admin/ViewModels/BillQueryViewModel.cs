using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcHwDay01.Models;

namespace MvcHwDay01.Areas.Admin.ViewModels
{
    public class BillQueryViewModel
    {
        /// <summary>
        /// 類別
        /// </summary>
        /// <remarks>
        /// 0.支出, 1.收入
        /// </remarks>
        [Display(Name = "類別")]
        public int? BillType { get; set; }

        /// <summary>
        /// 查詢日期區間(起)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [Display(Name = "查詢日期區間(起)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 查詢日期區間(迄)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [Display(Name = "查詢日期區間(迄)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 查詢的結果
        /// </summary>
        /// <remarks>
        /// 這樣應該可以作到: 將送出查詢條件與查詢結果放在同一個頁面
        /// </remarks>
        public IEnumerable<BillingItemViewModel> QueryResult { get; set; }

    }
}