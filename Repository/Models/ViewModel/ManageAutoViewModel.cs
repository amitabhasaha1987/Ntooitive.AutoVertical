using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.ViewModel
{
    public class ManageAutoViewModel
    {
        [BsonIgnore]
        public string UniqueId { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsClassified { get; set; }
        public bool IsSpotlight { get; set; }

        public bool ShowMarketVal { get; set; }
        public bool ShowOwnershipCost { get; set; }

       // public string AgentDescription { get; set; }
       // public bool IsDeleted { get; set; }
       // public List<DateTimeRange> DateTimeRanges { get; set; }

        [BsonIgnore]
        public List<string> PhotosUrl { get; set; }


        [BsonIgnore]
        public string Type { get; set; }
    }
}
