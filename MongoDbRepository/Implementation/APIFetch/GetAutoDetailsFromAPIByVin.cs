using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.Models;
using RestSharp;
using System.Configuration;
using RestSharp.Deserializers;
using Repository.Models.Admin.Auto;
namespace MongoDbRepository.Implementation.APIFetch
{
    public class FetchAutoDetailsFromAPIByVin : IFetchAutoDetailsFromAPI
    {
        public Auto GetAutoDetailsFromAPIByVin(string vin)
        {
            //--------------------------------------------new-----------------------------------------------------------------------
            Auto objAuto = new Auto();

            //http://specifications.vinaudit.com/getspecifications.php?vin=KMHWF35H84A921550&key=VA_DEMO_KEY&format=json

            var key = "VA_DEMO_KEY";
            var client = new RestClient("http://specifications.vinaudit.com/getspecifications.php");
            var format = "json";



            var request = new RestRequest("?vin=" + vin + "&key=" + key + "&format=" + format + "", Method.POST)
            {
                RequestFormat = DataFormat.Json

            };


            var response = client.Execute<AutoAPI>(request);
            JsonDeserializer deserial = new JsonDeserializer();
            try
            {
                AutoAPI data = deserial.Deserialize<AutoAPI>(response);
                if (!string.IsNullOrEmpty(data.vin))
                {
                    if (string.IsNullOrEmpty(data.Attributes.Trim))
                    {
                        objAuto.Trim = "N.A.";
                    }
                    else
                    {
                        objAuto.Trim = data.Attributes.Trim;
                    }
                    if (string.IsNullOrEmpty(data.Attributes.VehicleCategory))
                    {
                        objAuto.Category = "N.A.";
                    }
                    else
                    {
                        objAuto.Category = data.Attributes.VehicleCategory;
                    }
                    if (string.IsNullOrEmpty(data.Attributes.VehicleStyle))
                    {
                        objAuto.VehicleStyle = "N.A.";
                    }
                    else
                    {
                        objAuto.VehicleStyle = data.Attributes.VehicleStyle;
                    }
                    if (string.IsNullOrEmpty(data.Attributes.VehicleSize))
                    {
                        objAuto.VehicleSize = "N.A.";
                    }
                    else
                    {
                        objAuto.VehicleSize = data.Attributes.VehicleSize;
                    }



                    return objAuto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }


            //var response1 = client.Execute<object>(request);




            //----------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------old-----------------------------------------------------------------------
            //Auto objAuto = new Auto();
            ////var auth_token = ConfigurationManager.AppSettings["auth_token"];
            //var auth_token = "RLBVPvj8riQmRuPzcT";
            //var client = new RestClient("http://vinvault.com/");
            //var request = new RestRequest("api/decodes?vin=" + vin + "&auth_token=" + auth_token + "", Method.POST)
            //{
            //    RequestFormat = DataFormat.Json
            //};


            //var response = client.Execute<List<AutoAPI>>(request);



            //if (response.Data[0].decode.status[0].name == "VALID" && response.Data[0].decode.status[1].name == "VALIDCHECK" && response.Data[0].decode.status[2].name == "VALIDCHARACTERS" && response.Data[0].decode.status[3].name == "MATCH")
            //{

            //    objAuto.SubModel = response.Data[0].decode.series;
            //    objAuto.Category = response.Data[0].decode.trim[0].name;

            //    return objAuto;
            //}
            //else if (response.Data.ToString() == "OVER_QUERY_LIMIT")
            //{
            //    return null;
            //}
            //else
            //{
            //    return null;
            //}

            /////api/decodes?vin=1D7RB1CT8AS203937&auth_token=RLBVPvj8riQmRuPzcT
            //// return objAuto;

            //---------------------------------------------------------------------------------------------------
        }
            
    }
}
