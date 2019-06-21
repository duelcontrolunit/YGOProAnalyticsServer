using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Validators.Interfaces
{
    /// <summary>
    /// Provide validation for <see cref="ArchetypeBrowserQueryParams"/>
    /// </summary>
    public interface IArchetypeBrowserQueryParamsValidator
    {
        /// <summary>
        /// Returns true if <see cref="ArchetypeBrowserQueryParams"/> are valid.
        /// </summary>
        /// <param name="queryParams">The query parameters.</param>
        bool IsValid(ArchetypeBrowserQueryParams queryParams);
    }
}