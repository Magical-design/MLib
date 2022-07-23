using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MLib
{
    public class NumValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int _out;
            if (!int.TryParse(value.ToString(),out _out))
                return new System.Windows.Controls.ValidationResult(false, "请输入整数!");
            return ValidationResult.ValidResult;
        }
    }
    public class IpValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!Regex.IsMatch(value.ToString(), @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                return new System.Windows.Controls.ValidationResult(false, "请输入正确的IP地址!");
            return ValidationResult.ValidResult;
        }
    }
}
