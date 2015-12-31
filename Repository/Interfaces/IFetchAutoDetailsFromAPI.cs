using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;
using Repository.Models.Admin.Auto;

namespace Repository.Interfaces
{
    public interface IFetchAutoDetailsFromAPI
    {
        Auto GetAutoDetailsFromAPIByVin(string vin);
              

    }
}
