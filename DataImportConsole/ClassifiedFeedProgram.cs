using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Configuration;
using Repository.Interfaces;
using Repository.Models;
using System.Runtime.Serialization.Formatters.Binary;
using Repository.Models.Admin.Dealer;
using Repository.Interfaces.Admin.Dealer;
using AutoMapper;
using Repository.Models.Map;
using System.Collections;

namespace DataImportConsole
{
    class ClassifiedFeedProgram
    {
         void Main(string[] args)
        {
            RunClassifiedFeedProcess();

        }
        public static void RunClassifiedFeedProcess()
        {
            if (!Initialize())
            {
                Console.WriteLine("Error!!! Unable to initialize.");
                Console.ReadKey(true);
                return;
            }
            #region "Added for ClassifiedFeed"

            #region "Download & Process The File"

            var folderPathClassifiedFeed = ConfigurationManager.AppSettings["path"];
            var remoteFtpPathClassifiedFeed = ConfigurationManager.AppSettings["FTPpathClassified"];

            var downloader = NinjectConfig.Get<IDownloader>();
            string fileNameClassifiedFeed = downloader.DownloadFileForClassifiedFeed(remoteFtpPathClassifiedFeed, folderPathClassifiedFeed);
            //string fileNameClassifiedFeed = "motorslinersdut12092015_2301.xml";
            string localDestinationPath = folderPathClassifiedFeed + fileNameClassifiedFeed;

            var newlistingDeserializerClassifiedFeed = new XmlSerializer(typeof(ClassifiedListingRoot));
            var newlistingreaderClassifiedFeed = new StreamReader(localDestinationPath);
            var objClassifiedFeed = newlistingDeserializerClassifiedFeed.Deserialize(newlistingreaderClassifiedFeed);
            var classifiedFeed = (ClassifiedListingRoot)objClassifiedFeed;

            newlistingreaderClassifiedFeed.Close();

            var offcClassifiedFeed = Mapper.Map<List<Repository.Models.Admin.Auto.Auto>>(classifiedFeed.Autos.AutoClassifiedFeed);



            #endregion


            #region "ProcessFeed Mongo Insert"
            var autoVerticalClassifiedFeed = NinjectConfig.Get<IAutoVertical>();
            Hashtable hashGeo = new Hashtable();
            var coordinates = NinjectConfig.Get<ICoordinates>();
            var geoLoc = NinjectConfig.Get<IFetchLatLong>();
            foreach (var newAuto in offcClassifiedFeed)
            {
                string DealerAddress = !string.IsNullOrEmpty(newAuto.DealerAddress) ? newAuto.DealerAddress + "," : string.Empty;
                string DealerCity = !string.IsNullOrEmpty(newAuto.DealerCity) ? newAuto.DealerCity + "," : string.Empty;
                string DealershipZip = !string.IsNullOrEmpty(newAuto.DealerZip) ? newAuto.DealerZip : string.Empty;
                string address = DealerAddress + DealerCity + DealershipZip;
                if (!String.IsNullOrEmpty(address))
                {
                    address = (address.LastIndexOf(',') == address.Length - 1) ? address.Remove(address.Length - 1, 1) : address;

                    Location geoLocation = new Location();
                    bool isExistinlocal = true;
                    List<double> lst = new List<double>();

                    #region Get Lat Lng
                    if (hashGeo.ContainsKey(address))
                    {
                        geoLocation = (Location)hashGeo[address];
                    }
                    else
                    {
                        Coordinates cor = coordinates.GetCoordinates(address);
                        if (cor != null)
                        {
                            geoLocation.lng = cor.Coordinate[0];
                            geoLocation.lat = cor.Coordinate[1];
                        }
                        else
                        {
                            isExistinlocal = false;
                            geoLocation = geoLoc.GetLatitudeAndLongitude(address);
                        }
                    }
                    #endregion

                    if (isExistinlocal == false)
                    {
                        hashGeo.Add(address, geoLocation);
                    }

                    if (geoLocation != null)
                    {
                        lst.Add(geoLocation.lng);
                        lst.Add(geoLocation.lat);
                        Repository.Models.Admin.Auto.GeoPoint gp = new Repository.Models.Admin.Auto.GeoPoint(geoLocation.lng, geoLocation.lat);
                        gp.Coordinates = lst;
                        newAuto.GeoLocation = gp;
                    }
                }
            }

            autoVerticalClassifiedFeed.ProcessFeed(offcClassifiedFeed);
            #region Coordinates

            List<Coordinates> lstCor = new List<Coordinates>();
            foreach (DictionaryEntry item in hashGeo)
            {
                Coordinates c = new Coordinates();
                c.Address = (string)item.Key;
                Location loc = (Location)item.Value;
                c.Coordinate = new double[] { loc.lng, loc.lat };
                lstCor.Add(c);
            }
            if (lstCor.Count > 0)
            {
                var j = coordinates.InsertBulkCoordinates(lstCor);
            }
            #endregion
            #endregion


            #endregion
        }

        private static bool Initialize()
        {
            NinjectConfig.StartScheduler();
            AutoMapperConfiguration.Configure();
            return true;
        }
    }
}
