namespace YGOProAnalyticsServer
{
    /// <summary>
    /// It provide options which should be set up by server Admin.
    /// </summary>
    public interface IAdminConfig
    {
        /// <summary>
        /// Gets the maximum number of results per browser page.
        /// </summary>
        /// <value>
        /// The maximum number of results per browser page.
        /// </value>
        int MaxNumberOfResultsPerBrowserPage { get; }

        /// <summary>
        /// Gets the name of default banlist.
        /// </summary>
        /// <value>
        /// The name of default banlist.
        /// </value>
        string DefaultBanlistName { get; }

        /// <summary>
        /// Gets the FTP User credential.
        /// </summary>
        /// <value>
        /// The FTP user.
        /// </value>
        string FTPUser { get; }
        /// <summary>
        /// Gets the FTP password credential.
        /// </summary>
        /// <value>
        /// The FTP password.
        /// </value>
        string FTPPassword { get; }

        /// <summary>
        /// Gets the list of rooms URL (ygopro server).
        /// </summary>
        /// <value>
        /// The of rooms URL (ygopro server).
        /// </value>
        string YgoProListOfRoomsUrl { get; }
      
        /// Gets the ydk folder location.
        /// </summary>
        /// <value>
        /// The ydk folder location.
        /// </value>
        string DataFolderLocation { get; }

        /// <summary>
        /// Gets the card API URL.
        /// </summary>
        /// <value>
        /// The card API URL.
        /// </value>
        string CardApiURL { get; }

        /// <summary>
        /// Gets the banlist API URL.
        /// </summary>
        /// <value>
        /// The banlist API URL.
        /// </value>
        string BanlistApiURL { get; }

        /// <summary>
        /// Gets the server data endpoint URL.
        /// </summary>
        /// <value>
        /// The server data endpoint URL.
        /// </value>
        string ServerDataEndpointURL { get; }

        /// <summary>
        /// Gets the default number of results per browser page.
        /// </summary>
        /// <value>
        /// The default number of results per browser page.
        /// </value>
        int DefaultNumberOfResultsPerBrowserPage { get; }

        /// <summary>
        /// Gets the banlist sliding cache expiration in hours.
        /// </summary>
        /// <value>
        /// The banlist sliding cache expiration in hours.
        /// </value>
        int BanlistSlidingCacheExpirationInHours { get; }

        /// <summary>
        /// Gets the banlist absolute cache expiration in hours.
        /// </summary>
        /// <value>
        /// The banlist absolute cache expiration in hours.
        /// </value>
        int BanlistAbsoluteCacheExpirationInHours { get; }

        /// <summary>
        /// Gets the archetype sliding cache expiration in hours.
        /// </summary>
        /// <value>
        /// The archetype sliding cache expiration in hours.
        /// </value>
        int ArchetypeSlidingCacheExpirationInHours { get; }

        /// <summary>
        /// Gets the archetype absolute cache expiration in hours.
        /// </summary>
        /// <value>
        /// The archetype absolute cache expiration in hours.
        /// </value>
        int ArchetypeAbsoluteCacheExpirationInHours { get; }
    }
}