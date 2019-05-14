namespace YGOProAnalyticsServer
{
    /// <summary>
    /// It provide options which should be set up by server Admin.
    /// </summary>
    public interface IAdminConfig
    {
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
    }
}