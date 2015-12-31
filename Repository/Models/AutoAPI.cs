using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Repository.Models
{
    //public class AutoAPI : BaseEntity
    //{
    //    public decode decode { get; set; }
    //}

    //public class decode
    //{
    //    public int api_version { get; set; }
    //    public int id { get; set; }
    //    public string vin { get; set; }
    //    public string year { get; set; }
    //    public string make { get; set; }
    //    public string series { get; set; }
    //    public List<status> status { get; set; }
    //    public List<trim> trim { get; set; }

    //}

    //public class status
    //{
    //    public string name { get; set; }
    //    public string message { get; set; }
    //}

    //public class trim
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public List<data> data { get; set; }
    //}

    //public class data
    //{
    //    public int id { get; set; }
    //    public string category { get; set; }
    //    public string value { get; set; }
    //    public string unit { get; set; }
    //    public int group_id { get; set; }
    //    public string group { get; set; }
    //}

    //-----------------------------------------------------------------------------------------------------------------------

    public class AutoAPI : BaseEntity
    {
        public string vin { get; set; }

        public Attributes Attributes { get; set; }

    }
    public class Attributes
    {
        public string VIN { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        [JsonProperty("Made In")]
        public string MadeIn { get; set; }

        [JsonProperty("Vehicle Style")]
        public string VehicleStyle { get; set; }
        [JsonProperty("Vehicle Type")]
        public string VehicleType { get; set; }

        [JsonProperty("Vehicle Size")]
        public string VehicleSize { get; set; }

        [JsonProperty("Vehicle Category")]
        public string VehicleCategory { get; set; }
        [JsonProperty("Doors")]
        public string Doors { get; set; }
        [JsonProperty("Fuel Type")]
        public string FuelType { get; set; }
        [JsonProperty("Fuel Type")]
        public string FuelCapacity { get; set; }
        [JsonProperty("City Mileage")]
        public string CityMileage { get; set; }

        [JsonProperty("Highway Mileage")]
        public string HighwayMileage { get; set; }
        [JsonProperty("Fuel Type")]
        public string Engine { get; set; }
        [JsonProperty("Engine Size")]
        public string EngineSize { get; set; }

        [JsonProperty("Engine Cylinders")]
        public string EngineCylinders { get; set; }

        [JsonProperty("Transmission Type")]
        public string TransmissionType { get; set; }

        [JsonProperty("Transmission Gears")]
        public string TransmissionGears { get; set; }
        [JsonProperty("Driven Wheels")]
        public string DrivenWheels { get; set; }

        [JsonProperty("Anti-Brake System")]
        public string AntiBrakeSystem { get; set; }

        [JsonProperty("Steering Type")]
        public string SteeringType { get; set; }
        [JsonProperty("Curb Weight")]
        public string CurbWeight { get; set; }

        [JsonProperty("Gross Weight")]
        public string GrossWeight { get; set; }

        [JsonProperty("Overall Height")]
        public string OverallHeight { get; set; }

        [JsonProperty("Overall Length")]
        public string OverallLength { get; set; }
        [JsonProperty("Overall Width")]
        public string OverallWidth { get; set; }

        [JsonProperty("Standard Seating")]
        public string StandardSeating { get; set; }

        [JsonProperty("Optional Seating")]
        public string OptionalSeating { get; set; }
        [JsonProperty("Invoice Price")]
        public string InvoicePrice { get; set; }
        [JsonProperty("Delivery Charges")]
        public string DeliveryCharges { get; set; }
        public string MSRP { get; set; }
    }

    public class RootObject
    {
        public string vin { get; set; }
        public Attributes attributes { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
    }

    public class Content
    {
        public object vin { get; set; }
    }
}
