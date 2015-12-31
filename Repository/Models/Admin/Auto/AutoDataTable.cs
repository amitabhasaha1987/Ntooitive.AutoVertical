using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.Admin.Auto
{
    public class AutoDataTable
    {
        public bool isVinSearchable { get; set; }
        public bool isMakeSearchable { get; set; }
        public bool isModelSearchable { get; set; }
        public bool isPriceSearchable { get; set; }
        public bool isConditionSearchable { get; set; }
        public bool isMileageSearchable { get; set; }
        public bool isStockNoSearchable { get; set; }
        public bool isTransmissionSearchable { get; set; }
        public bool isInteriorColorSearchable { get; set; }
        public bool isExteriorColorSearchable { get; set; }
        public bool isLocationSearchable { get; set; }

        public bool isVinSortable { get; set; }
        public bool isMakeSortable { get; set; }
        public bool isModelSortable { get; set; }
        public bool isPriceSortable { get; set; }
        public bool isConditionSortable { get; set; }
        public bool isMileageSortable { get; set; }
        public bool isStockNoSortable { get; set; }
        public bool isTransmissionSortable { get; set; }
        public bool isInteriorColorSortable { get; set; }
        public bool isExteriorColorSortable { get; set; }
        public bool isLocationSortable { get; set; }


        public int sortColumnIndex { get; set; }
        public string sortDirection { get; set; }
    }
}
