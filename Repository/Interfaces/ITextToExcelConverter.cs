using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITextToExcelConverter
    {
        void ConvertTxtToExcel(string textPath, string excelPath);
    }
}
