using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DbModels.Validators
{
    public class ArchetypeNameFormatAttribute : ValidationAttribute
    {
        string _validFormatRegex;

        public ArchetypeNameFormatAttribute(string validFormatRegex = "")
        {
            _validFormatRegex = validFormatRegex;
        }

        public override bool IsValid(object value)
        {            
            return Regex.IsMatch(value.ToString(), _validFormatRegex); ;
        }

    }
}
