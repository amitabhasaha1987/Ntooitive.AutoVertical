using Repository.Models;
using Repository.Models.Admin.Auto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITSVReader
    {
        List<Auto> ReadFile(byte[] byteArray);
    }
}
