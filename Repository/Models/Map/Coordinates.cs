using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.Map
{
    public class Coordinates : BaseEntity
    {
        public string Address { get; set; }
        public double[] Coordinate { get; set; }
    }
}
