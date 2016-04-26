using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcHwDay01.Models.Attributes;

namespace MvcHwDay01.Models
{

    public class BillingItemViewModel
    {

        /// <summary>
        /// 唯一編號
        /// </summary>
        /// <remarks>
        /// 加入原因:
        /// 1. 新增(Create): 讓 ViewModel 在 Controller 準備好, 才丟到 Service 層
        /// 2. 修改(Edit) / 刪除(Delete): 需要 PKEY 作處理
        /// </remarks>
        [Display(Name = "唯一編號")]
        public Guid Id { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        /// <remarks>
        /// 0.支出, 1.收入
        /// </remarks>
        [Required]
        [Display(Name = "類別")]
        public int BillType { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [Required]
        [Display(Name = "日期")]
        [DataType(DataType.Date)]
        //[Range(typeof(DateTime),"2016/04/01", "2016/04/30", ErrorMessage = "{0} 欄位不可大於今日")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateMustLessOrEqualThanToday]
        public DateTime BillDate { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        //[RegularExpression(@"\d+", ErrorMessage = "{0} 欄位與指定的數字格式不符")]
        [Required]
        [Display(Name = "金額")]
        [Range(1, int.MaxValue, ErrorMessage ="{0} 欄位必須為正整數")]
        public int Amount { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "備註")]
        [StringLength(100, ErrorMessage = "{0} 欄位最多100個字元")]
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