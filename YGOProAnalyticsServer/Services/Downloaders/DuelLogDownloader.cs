using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Downloaders
{
    public class DuelLogDownloader : IDuelLogDownloader
    {
        private IAdminConfig _adminConfig;

        public DuelLogDownloader(IAdminConfig adminConfig)
        {
            _adminConfig = adminConfig;
        }

        /// <summary>
        /// Downloads the duel log from FTP.
        /// </summary>
        /// <param name="EndPointFTP">The end point FTP link.</param>
        /// <returns>
        /// Path to the downloaded DuelLog file
        /// </returns>
        /// <exception cref="UriFormatException">Thrown when Uri format is not FTP</exception>
        public async Task<string> DownloadDuelLogFromFTP(string EndPointFTP)
        {

            Uri _ftpUri = new Uri(EndPointFTP);
            string _folderPath = @"DuelLogDatas";
            string _filePath = Path.Combine(_folderPath, Path.GetFileName(_ftpUri.LocalPath));
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            if (_ftpUri.Scheme != Uri.UriSchemeFtp)
            {
                throw new UriFormatException("Format must be FTP");
            }
            using (var request = new WebClient())
            {
                request.Credentials = new NetworkCredential(_adminConfig.FTPUser, _adminConfig.FTPPassword);
                await request.DownloadFileTaskAsync(_ftpUri, _filePath + ".tmp");
            }
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            if (File.Exists(_filePath + ".tmp"))
            {
                File.Move(_filePath + ".tmp", _filePath);
                return _filePath;
            }
            else
            {
                throw new FileNotFoundException("Downloaded file couldn't be found.");
            }
        }
    }
}
