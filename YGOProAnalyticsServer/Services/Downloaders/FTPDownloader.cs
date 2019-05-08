using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Downloaders
{
    /// <summary>
    /// Use it to download data from FTP
    /// </summary>
    /// <seealso cref="YGOProAnalyticsServer.Services.Downloaders.Interfaces.IFTPDownloader" />
    public class FTPDownloader : IFTPDownloader
    {
        private IAdminConfig _adminConfig;
        /// <summary>
        /// Initializes a new instance of the <see cref="FTPDownloader"/> class.
        /// </summary>
        /// <param name="adminConfig">The admin configuration.</param>
        public FTPDownloader(IAdminConfig adminConfig)
        {
            _adminConfig = adminConfig;
        }

        /// <inheritdoc />
        public async Task<string> DownloadDuelLogFromFTP(string EndPointFTP)
        {

            Uri _ftpUri = new Uri(EndPointFTP);
            string _folderPath = Path.Combine(_adminConfig.DataFolderLocation, "DuelLogZipFiles");
            string _filePath = Path.Combine(_folderPath, Path.GetFileName(_ftpUri.LocalPath));
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            if (_ftpUri.Scheme != Uri.UriSchemeFtp)
            {
                throw new UriFormatException("Format must be FTP.");
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

        /// <inheritdoc />
        public async Task<string> DownloadDeckFromFTP(string URLToDeckList)
        {

            Uri _ftpUri = new Uri(URLToDeckList);
            string _folderPath = Path.Combine(_adminConfig.DataFolderLocation, "DecksZipFiles");
            string _filePath = Path.Combine(_folderPath, Path.GetFileName(_ftpUri.LocalPath));
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            if (_ftpUri.Scheme != Uri.UriSchemeFtp)
            {
                throw new UriFormatException("Format must be FTP.");
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
        /// <inheritdoc />
        public List<string> DownloadListOfFilesFromFTP(string EndPointFTP)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(EndPointFTP);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(_adminConfig.FTPUser, _adminConfig.FTPPassword);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string names = reader.ReadToEnd();

            reader.Close();
            response.Close();
            var result = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            result.Sort();
            return result;
        }

    }
}
