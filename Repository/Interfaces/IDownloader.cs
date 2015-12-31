using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IDownloader
    {
        string DownloadFileForClassifiedFeed(string remoteFtpPath, string localDestinationPath);
        string DownloadFile(string remoteFtpPath, string localDestinationPath);
    }
}
