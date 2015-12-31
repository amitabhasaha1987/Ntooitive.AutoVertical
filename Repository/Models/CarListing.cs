using MongoDB.Bson;
using Repository.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class CarListing
    {
        public ObjectId Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double Mileage { get; set; }
        public double Price { get; set; }
        public string Condition { get; set; }
        public string InteriorColor { get; set; }
        public string ExteriorColor { get; set; }
        public string Description { get; set; }
        public string[] PhotosUrl { get; set; }
        public string StockNumber { get; set; }
        public string Transmission { get; set; }
        public string DealerAddress { get; set; }
        public string DealerCity { get; set; }
        public string DealerPhone { get; set; }

        public string DealerName  { get; set; }

        public ManageAutoViewModel ExtProperties { get; set; }
    }
}
