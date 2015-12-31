using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class Dealer : BaseEntity
    {
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerAddress { get; set; }
        public string DealerCity { get; set; }
        public string DealerState { get; set; }
        public string DealerZip { get; set; }
        public string DealerPhone { get; set; }
        public string DealerEmail { get; set; }
        public string Role { get; set; }

        public string ChatScript { get; set; }
    }
}
