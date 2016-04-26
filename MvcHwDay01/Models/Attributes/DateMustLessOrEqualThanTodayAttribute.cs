using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcHwDay01.Models.Attributes
{
    public class DateMustLessOrEqualThanTodayAttribute : ValidationAttribute, IClientValidatable
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

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "billdate",
            };
            yield return rule;
        }

    }
}