using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IReader
    {
        List<Repository.Models.Admin.Auto.Auto> ReadFile(string filePath);
        List<Repository.Models.Admin.Auto.Auto> ReadFile(byte[] byteArray);        
    }
}
