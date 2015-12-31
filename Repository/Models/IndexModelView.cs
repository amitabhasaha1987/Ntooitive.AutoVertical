using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class IndexModelView
    {
        public List<Make> MakeList { get; set; }
        public List<VehicleType> VehicleTypeList { get; set; }
        public List<Model> ModelList { get; set; }

        public List<UserSrch> UserList { get; set; }
        public AdvanceSearch advanceSearch { get; set; }
    }
}
