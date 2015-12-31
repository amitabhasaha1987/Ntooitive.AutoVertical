using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class AdvanceSearch
    {
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string Mileage { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
        public string UsedNew { get; set; }
        public string Make { get; set; }
        public string DealerSrch { get; set; }
        public string Model { get; set; }
        public string Location { get; set; }
        public string VehicleType { get; set; }
        public string ZipCode { get; set; }
        public List<Make> Makes { get; set; }
        public List<Model> ModelList { get; set; }
        public Dictionary<string, List<Model>> Models { get; set; }
        public List<string> ExteriorColor { get; set; }
        public string ExSelectedColor { get; set; }
        public List<string> InteriorColor { get; set; }
        public string InSelectedColor { get; set; }
        public List<Int32> YearList { get; set; }
        public string SortingOrder { get; set; }

        public string Dealer { get; set; }

        public List<UserSrch> Dealers { get; set; }
    }
}
