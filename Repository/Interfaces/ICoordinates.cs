using Repository.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ICoordinates
    {
        Coordinates GetCoordinates(string address);
        bool InsertBulkCoordinates(List<Coordinates> coordinates);
    }
}
