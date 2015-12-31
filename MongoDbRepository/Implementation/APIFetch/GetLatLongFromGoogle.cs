using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.Models;
using RestSharp;

namespace MongoDbRepository.Implementation.APIFetch
{
   public class GetLatLongFromGoogle : IFetchLatLong
    {
        public string GetApiLink(string address)
        {
          return  "http://maps.google.com/maps/api/geocode/xml?address="+address+"&sensor=false";
        }

        public Location GetLatitudeAndLongitude(string address)
        {
            FetDataFromAPI : 

            var client = new RestClient("https://maps.google.com/");

            var request = new RestRequest("maps/api/geocode/json?address=" + address + "&key=AIzaSyCbjnpA1wXxk5A4pOV-2BbJgMg2gIXDHyM", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };
            var response = client.Execute<AddressDetails>(request);

            if (response.Data != null)
            {
                if (response.Data.status == "OK")
                {
                    var location = response.Data.results.FirstOrDefault();
                    if (location != null)
                        return location.geometry.location;
                    else
                    {
                        return null;
                    }
                }
                else if (response.Data.status == "OVER_QUERY_LIMIT")
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public global::Repository.Models.Map.Coordinates GetCoordinates(string address)
        {
            throw new NotImplementedException();
        }
    }
}
