using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Validators.Interfaces
{
    /// <summary>
    /// Provide validation for <see cref="BanlistBrowserQueryParams"/>.
    /// </summary>
    public interface IBanlistBrowserQueryParamsValidator
    {
        /// <summary>
        /// Returns true if <see cref="BanlistBrowserQueryParams"/> are valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        bool IsValid(BanlistBrowserQueryParams queryParams);
    }
}