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
    public class GetMarketValueFromAPIByVin : IFetchMarketValueFromAPIByVin
    {
        public MarketValue GetMarketValueByVin(string vin)
        {
            //MarketValue objMarketValue = new MarketValue();

            //Auto objAuto = new Auto();

            //http://specifications.vinaudit.com/getspecifications.php?vin=KMHWF35H84A921550&key=VA_DEMO_KEY&format=json

            var key = "VA_DEMO_KEY";
            var client = new RestClient("https://marketvalue.vinaudit.com/getmarketvalue.php");
            var format = "json";



            var request = new RestRequest("?vin=" + vin + "&key=" + key + "&format=" + format + "", Method.POST)
            {
                RequestFormat = DataFormat.Json

            };


            var response = client.Execute<MarketValue>(request);
            JsonDeserializer deserial = new JsonDeserializer();
            try
            {
                MarketValue objMarketValue = deserial.Deserialize<MarketValue>(response);
                return objMarketValue;
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
            
    }
}
