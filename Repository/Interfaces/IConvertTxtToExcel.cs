using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IConvertTxtToExcel
    {
        void ConvertTxtToExcel(string TextPath, string ExcelPath);
    }
}
