using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcHwDay01.Models
{

    public class BillingItemViewModel
    {

        /// <summary>
        /// 類別
        /// </summary>
        /// <remarks>
        /// 1.支出, 2.收入
        /// </remarks>
        [Display(Name = "類別")]
        public int BillType { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [Display(Name = "日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BillDate { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Display(Name = "金額")]
        public decimal Aoumnt { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Display(Name = "備註")]
        public string Memo { get; set; }

        /// <summary>
        /// 類別名稱
        /// </summary>
        /// <remarks>
        /// 用在清單顯示之用
        /// </remarks>
        [Display(Name = "類別名稱")]
        public string BillTypeName { get; set; }

    }
}