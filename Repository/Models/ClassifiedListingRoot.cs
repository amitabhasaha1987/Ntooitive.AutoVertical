
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Repository.Models
{
    [XmlRoot(ElementName = "Images")]
    public class Images
    {
        [XmlElement(ElementName = "Image")]
        public List<string> Image { get; set; }
    }

    [XmlRoot(ElementName = "Upsell")]
    public class Upsell
    {
        [XmlElement(ElementName = "UpsellFeaturedAd")]
        public string UpsellFeaturedAd { get; set; }
        [XmlElement(ElementName = "UpsellSpotlightAd")]
        public string UpsellSpotlightAd { get; set; }
    }

    [XmlRoot(ElementName = "auto")]
    public class AutoClassifiedFeed
    {
        [XmlElement(ElementName = "AdId")]
        public string AdId { get; set; }
        [XmlElement(ElementName = "bodystyle")]
        public string Bodystyle { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "enginetext")]
        public string Enginetext { get; set; }
        [XmlElement(ElementName = "exteriorcolor")]
        public string Exteriorcolor { get; set; }
        [XmlElement(ElementName = "fueltype")]
        public string Fueltype { get; set; }
        [XmlElement(ElementName = "city")]
        public string City { get; set; }
        [XmlElement(ElementName = "state")]
        public string State { get; set; }
        [XmlElement(ElementName = "zip")]
        public string Zip { get; set; }
        [XmlElement(ElementName = "interiorcolor")]
        public string Interiorcolor { get; set; }
        [XmlElement(ElementName = "year")]
        public string Year { get; set; }
        [XmlElement(ElementName = "make")]
        public string Make { get; set; }
        [XmlElement(ElementName = "model")]
        public string Model { get; set; }
        [XmlElement(ElementName = "mileage")]
        public string Mileage { get; set; }
        [XmlElement(ElementName = "numdoors")]
        public string Numdoors { get; set; }
        [XmlElement(ElementName = "postdate")]
        public string Postdate { get; set; }
        [XmlElement(ElementName = "expiredate")]
        public string Expiredate { get; set; }
        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
        [XmlElement(ElementName = "transmissiontype")]
        public string Transmissiontype { get; set; }
        [XmlElement(ElementName = "drivetype")]
        public string Drivetype { get; set; }
        [XmlElement(ElementName = "style")]
        public string Style { get; set; }
        [XmlElement(ElementName = "used")]
        public string Used { get; set; }
        [XmlElement(ElementName = "vin")]
        public string Vin { get; set; }
        [XmlElement(ElementName = "Images")]
        public Images Images { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "phone")]
        public string Phone { get; set; }
        [XmlElement(ElementName = "alternatephone")]
        public string Alternatephone { get; set; }
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "Upsell")]
        public Upsell Upsell { get; set; }
    }

    [XmlRoot(ElementName = "autos")]
    public class Autos
    {
        [XmlElement(ElementName = "auto")]
        public List<AutoClassifiedFeed> AutoClassifiedFeed { get; set; }
    }

    [XmlRoot(ElementName = "xml")]
    public class ClassifiedListingRoot
    {
        [XmlElement(ElementName = "autos")]
        public Autos Autos { get; set; }
    }

}
