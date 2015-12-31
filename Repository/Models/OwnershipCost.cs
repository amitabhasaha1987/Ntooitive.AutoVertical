using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    
    public class OwnershipCost
    {
        public string vin { get; set; }
        public int mileage_start { get; set; }
        public int mileage_year { get; set; }
        public string vehicle { get; set; }
        public List<int> depreciation_cost { get; set; }
        public List<int> insurance_cost { get; set; }
        public List<int> fuel_cost { get; set; }
        public List<int> maintenance_cost { get; set; }
        public List<int> repairs_cost { get; set; }
        public List<int> fees_cost { get; set; }
        public List<int> total_cost { get; set; }
        public int total_cost_sum { get; set; }
    }

}
