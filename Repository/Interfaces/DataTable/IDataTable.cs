using Repository.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.DataTable
{
    public interface IDataTable<T, S>
    {
        List<T> GetDataSet(string userEmail, JQueryDataTableParamModel dataTableParamModel, S serachCriteria, out long filteredCount, string type = "");
        long GetTotalCount(string userEmail, string type = "");
    }
}
