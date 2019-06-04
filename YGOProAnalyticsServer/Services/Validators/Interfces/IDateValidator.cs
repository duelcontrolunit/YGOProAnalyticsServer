namespace YGOProAnalyticsServer.Services.Validators.Interfaces
{
    /// <summary>
    /// Provide validation methods for date.
    /// </summary>
    public interface IDateValidator
    {
        /// <summary>
        /// Check if given string contain valid date format.
        /// <see cref="DateFormat"/>
        /// </summary>
        /// <param name="dateAsString">The date as string.</param>
        /// <param name="format">The date format.</param>
        bool IsValidFormat(string dateAsString, string format);
    }
}