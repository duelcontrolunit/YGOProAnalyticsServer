using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Downloaders
{
    public class FTPDownloader : IFTPDownloader
    {
        private IAdminConfig _adminConfig;

        public FTPDownloader(IAdminConfig adminConfig)
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
            string _folderPath = @"Data/DuelLogZipFiles";
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

        /// <summary>
        /// Downloads the decks file from FTP.
        /// </summary>
        /// <param name="EndPointFTP">The end point FTP link.</param>
        /// <returns>
        /// Path to the downloaded decks file
        /// </returns>
        /// <exception cref="UriFormatException">Thrown when Uri format is not FTP</exception>
        public async Task<string> DownloadDecksFromFTP(string EndPointFTP)
        {

            Uri _ftpUri = new Uri(EndPointFTP);
            string _folderPath = @"Data/DecksZipFiles";
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
