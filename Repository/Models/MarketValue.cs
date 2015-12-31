using MongoDB.Bson;
using Repository.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class Prices
    {
        public int average { get; set; }
        public int below { get; set; }
        public int above { get; set; }
    }

    public class MarketValue
    {
        public string vin { get; set; }
        public bool success { get; set; }
        public string vehicle { get; set; }
        public int count { get; set; }
        public int mean { get; set; }
        public int stdev { get; set; }
        public int certainty { get; set; }
        public List<string> period { get; set; }
        public Prices prices { get; set; }
    }
}
