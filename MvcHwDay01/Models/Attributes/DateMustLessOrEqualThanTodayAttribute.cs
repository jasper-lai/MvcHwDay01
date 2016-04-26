using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcHwDay01.Models.Attributes
{
    public class DateMustLessOrEqualThanTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = true;
            var inDate = Convert.ToDateTime(value);
            if (inDate > DateTime.Today)
            {
                result = false;
            }
            return result;
        }

        public override string FormatErrorMessage(string name)
        {
            return name + "的內容值, 必須小於或等於今日";
        }
    }
}