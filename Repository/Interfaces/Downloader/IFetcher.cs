using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.Downloader
{
    public interface IFetcher
    {
        bool SetUrl(string fileName);
        string GetUrl();





    }
}
