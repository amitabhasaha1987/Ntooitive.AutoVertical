using MongoDB.Driver;
using MongoDbRepository.Repository;
using Repository.Interfaces;
using Repository.Models;
using Repository.Models.Map;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbRepository.Implementation.APIFetch
{
    public class GetLatLongFromBing : IFetchLatLong
    {

        public string GetApiLink(string address)
        {
            throw new NotImplementedException();
        }

        public global::Repository.Models.Location GetLatitudeAndLongitude(string address)
        {
            try
            {
                Location objCor = new Location();
                var apiKey = ConfigurationManager.AppSettings["Bing:APIKey"];
                var baseUrl = ConfigurationManager.AppSettings["Bing:BaseUrl"];

                var client = new RestClient(baseUrl);

                #region LAT-LONG


                if (address.Contains("#"))
                {
                    address = address.Replace("#", "");
                }
                if (address.Contains("&"))
                {
                    address = address.Replace("&", "");
                }
                var request = new RestRequest("REST/v1/Locations?q=" + address + "&key=" + apiKey, Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };
                var response = client.Execute<BingAddressDetails>(request);

                if (response.Data.statusCode == 200)
                {
                    var location = response.Data.resourceSets.FirstOrDefault();
                    if (location != null)
                    {
                        var resorce = location.resources.FirstOrDefault();
                        if (resorce != null)
                        {
                            //var latLong = new Location
                            //{
                            //    lat = resorce.point.coordinates[0],
                            //    lng = resorce.point.coordinates[1],
                            //};
                            objCor.lat = resorce.point.coordinates[0];
                            objCor.lng = resorce.point.coordinates[1];
                        }
                        else
                        {
                            //var latLong = new LatitudeLongitude
                            //{
                            //    lat = 0,
                            //    lng = 0,
                            //};
                            //objCor.Coordinate = new double[] { latLong.lat, latLong.lng };
                            objCor.lat = 0;
                            objCor.lng = 0;
                        }
                    }
                }
                #endregion

                //List<NearByLocation> objLstLoc = new List<NearByLocation>();
                // var EntityCodeList = ConfigurationManager.AppSettings["Bing:EntityCodeList"];

                //foreach (var type in EntityCodeList.Split(','))
                //{
                //    string EntityName = Enum.GetName(typeof(EntiyiType), Convert.ToInt32(type));
                //    objLstLoc = getLocationByType(address, Convert.ToInt32(type));
                //    if (objLstLoc != null)
                //    {
                //        InsertEntityType(address, EntityName, objLstLoc);
                //    }
                //}

                return objCor;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "GetLatitudeAndLongitude");
                throw;
            }

        }


    }
}
