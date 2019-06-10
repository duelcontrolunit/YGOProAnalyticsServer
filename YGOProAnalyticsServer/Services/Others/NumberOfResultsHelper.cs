using System;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    public class NumberOfResultsHelper : INumberOfResultsHelper
    {
        readonly IAdminConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOfResultsHelper"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">config</exception>
        public NumberOfResultsHelper(IAdminConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <inheritdoc />
        public int GetNumberOfResultsPerPage(int numberOfResultsRequestedByUser)
        {
            int numberOfResultsPerPage = _config.DefaultNumberOfResultsPerBrowserPage;
            if (numberOfResultsRequestedByUser != -1)
            {
                if (numberOfResultsRequestedByUser <= _config.MaxNumberOfResultsPerBrowserPage)
                {
                    numberOfResultsPerPage = numberOfResultsRequestedByUser;
                }
                else
                {
                    numberOfResultsPerPage = _config.MaxNumberOfResultsPerBrowserPage;
                }
            }

            return numberOfResultsPerPage;
        }
    }
}
