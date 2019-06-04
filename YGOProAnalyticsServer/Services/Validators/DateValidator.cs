using System;
using System.Text.RegularExpressions;
using YGOProAnalyticsServer.Services.Validators.Interfaces;

namespace YGOProAnalyticsServer.Services.Validators
{
    /// <summary>
    /// Provide validation methods for date.
    /// </summary>
    public class DateValidator : IDateValidator
    {
        /// <inheritdoc />
        public virtual bool IsValidFormat(string dateAsString, string format)
        {
            switch (format)
            {
                case DateFormat.yyyy_MM_dd:
                    {
                        return _is_yyyy_mm_dd_Format(dateAsString);
                    }
                default:
                    {
                        throw new Exception($"{format.ToString()} format is not supported.");
                    }
            }
        }

        private bool _is_yyyy_mm_dd_Format(string dateAsString)
        {
            return Regex.IsMatch(
                dateAsString,
                @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))");
        } 
    }

    /// <summary>
    /// Provide date formats.
    /// </summary>
    public static class DateFormat
    {
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public const string yyyy_MM_dd = "yyyy-MM-dd";
    }
}
