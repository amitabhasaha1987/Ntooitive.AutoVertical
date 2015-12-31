using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class UserSrch 
    {
        public string DealerId { get; set; }
        public string DealerName { get; set; }       
        public int Count { get; set; }
        public bool IsSelected { get; set; }
    }
}
