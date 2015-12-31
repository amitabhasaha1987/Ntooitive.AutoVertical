using Repository.Interfaces;
using Repository.Models;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbRepository.Implementation.APIFetch
{
    public class GetOwnershipCostFromAPIByVin : IFetchOwnershipCostFromAPIByVin
    {
        public OwnershipCost GetOwnershipCostByVin(string vin)
        {
            //MarketValue objMarketValue = new MarketValue();

            //Auto objAuto = new Auto();

            //http://specifications.vinaudit.com/getspecifications.php?vin=KMHWF35H84A921550&key=VA_DEMO_KEY&format=json

            var key = "VA_DEMO_KEY";
            var client = new RestClient("http://ownershipcost.vinaudit.com/getownershipcost.php");
            var format = "json";



            var request = new RestRequest("?vin=" + vin + "&key=" + key + "&format=" + format + "", Method.POST)
            {
                RequestFormat = DataFormat.Json

            };


            var response = client.Execute<OwnershipCost>(request);
            JsonDeserializer deserial = new JsonDeserializer();
            try
            {
                OwnershipCost objOwnershipCost = deserial.Deserialize<OwnershipCost>(response);
                return objOwnershipCost;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
               
    }
}
