using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;
using Repository.Models.Map;

namespace Repository.Interfaces
{
    public interface IFetchLatLong
    {
        string GetApiLink(string address);
        Location GetLatitudeAndLongitude(string address);
    }
}
