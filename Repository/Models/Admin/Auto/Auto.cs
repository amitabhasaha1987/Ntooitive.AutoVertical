using MongoDB.Bson.Serialization.Attributes;
using Repository.Interfaces;
using Repository.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.Admin.Auto
{
    public class Auto : BaseEntity
    {
        private readonly IFetchLatLong _fetchlatlong;
        public Auto()
        {

        }

        public Auto(IFetchLatLong fetchlatlong)
        {
            _fetchlatlong = fetchlatlong;
        }
        public string DealerId { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int Year { get; set; }
        public string Vin { get; set; }
        public string Category { get; set; }
        public double Mileage { get; set; }
        public double Price { get; set; }
        public string Condition { get; set; }
        public string InteriorColor { get; set; }
        public string ExteriorColor { get; set; }
        public string Description { get; set; }
        public string VehicleStyle { get; set; }
        public string VehicleSize { get; set; }
        public string DealershipPhone { get; set; }
        public string DealershipCity { get; set; }
        public string DealershipState { get; set; }
        public string DealershipZip { get; set; }
        public List<string> PhotosUrl { get; set; }
        public string StockNumber { get; set; }
        public string Transmission { get; set; }
        public string FranchiseID { get; set; }
        public string DealerName { get; set; }
        public string DealerAddress { get; set; }
        public string DealerCity { get; set; }
        public string DealerState { get; set; }
        public string DealerZip { get; set; }
        public string DealerPhone { get; set; }
        public string DealerEmail { get; set; }
        public string LeadEmailAddress { get; set; }
        public string CCEmailAddress { get; set; }
        public string LeadType { get; set; }
        public string ChatScript { get; set; }
        public string ProfileImage { get; set; }
        public GeoPoint GeoLocation { get; set; }

        public MarketValue AutoMarketValue { get; set; }
        public OwnershipCost AutoOwnershipCost { get; set; }

        //public bool ShowMarketVal { get; set; }
        //public bool ShowOwnershipCost { get; set; }
       
        [BsonIgnore]
        private string _searchstring;
        public string SearchString
        {
            get
            {
                return this.Make + " " + this.Model;

            }
            set
            {
                _searchstring = value;
            }
        }





        [BsonIgnore]
        private string[] _searcharray;



        public string[] SearchArray
        {
            get
            {
                var searchList = new List<string> { };
                if (!string.IsNullOrEmpty(this.Make))
                {
                    searchList.AddRange(this.Make.ToLowerInvariant().Split(' '));

                }
                if (!string.IsNullOrEmpty(this.Model))
                {
                    searchList.AddRange(this.Model.ToLowerInvariant().Split(' '));

                }
                if (!string.IsNullOrEmpty(this.Trim))
                {
                    searchList.AddRange(this.Trim.ToLowerInvariant().Split(' '));

                }

                return searchList.ToArray();
            }
            set
            {
                _searcharray = value;
            }
        }

        public ManageAutoViewModel ExtProperties { get; set; }

        public string ListedBy { get; set; }

        #region "Added for ClassifiedFeed"

        public string AdId { get; set; }
        public string Bodystyle { get; set; }
        public string Enginetext { get; set; }
        public string Fueltype { get; set; }
        public string Numdoors { get; set; }
        public string Postdate { get; set; }
        public string Expiredate { get; set; }
        public string Drivetype { get; set; }
        public string Style { get; set; }
        public string UpsellFeaturedAd { get; set; }
        public string UpsellSpotlightAd { get; set; }
        public bool IsClassified { get; set; }
        #endregion

    }

    public class GeoPoint
    {
        public GeoPoint(double lat, double lang)
        {
            Coordinates = new List<double>();
            Coordinates.Add(lang);
            Coordinates.Add(lat);
            this.Type = "Point";
        }
        [BsonElement("type")]
        public string Type { get; set; }//Point
        [BsonElement("coordinates")]
        public List<double> Coordinates { get; set; }
    }
}
