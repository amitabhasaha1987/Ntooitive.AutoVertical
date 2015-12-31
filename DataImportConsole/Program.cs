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
using System.Collections;
using Repository.Models.Map;
using Repository.Models.Admin.Auto;
using System.Web.Script.Serialization;
using Repository.Interfaces.Downloader;

namespace DataImportConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Utility.UtilityClass.WriteLog("-------------------------RunProcess Start--------------------------------" + DateTime.Now);
                RunProcess1();
                //ClassifiedFeedProgram.RunClassifiedFeedProcess();

            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "-------------------------RunProcess--------------------------------");
                throw;
            }

            Utility.UtilityClass.WriteLog("-------------------------RunProcess End--------------------------------" + DateTime.Now);

        }

        private static void RunProcess1()
        {
            if (!Initialize())
            {
                Console.WriteLine("Error!!! Unable to initialize.");
                Console.ReadKey(true);
                return;
            }

            //Step 1 : Download the latest feed

            var folderPath = ConfigurationManager.AppSettings["path"];
            var remoteFtpPath = ConfigurationManager.AppSettings["FTPpath"];
            string fileName = "";
            Utility.UtilityClass.WriteLog("Download started");
            try
            {
                var downloader = NinjectConfig.Get<IDownloader>();
                fileName = downloader.DownloadFile(remoteFtpPath, folderPath);
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "DownloadFile");
                throw;
            }
            Utility.UtilityClass.WriteLog("Download completed");

            //----------------------------------------------------------

            if (!string.IsNullOrEmpty(fileName))
            {
                //Step 2 : Get Last Updated File Path From Database

                var fetchService = NinjectConfig.Get<IFetcher>();
                var previousUrl = fetchService.GetUrl();

                //-------------------------------------------------------

                //Step 3 : Save Latest File Path To Database
                var UnzipFilePath = @folderPath + fileName;

                var setCurrentUrl = fetchService.SetUrl(UnzipFilePath);

                //-------------------------------------------------------

                //Step 4: Unzip the latest file and then read

                byte[] data = File.ReadAllBytes(UnzipFilePath);

                var unzipper = NinjectConfig.Get<IUnzipper>();
                fileName = fileName.Split('.')[0];
                var unzippedData = unzipper.Decompress(data);

                Utility.UtilityClass.WriteLog("Unzip completed");
                var reader = NinjectConfig.Get<IReader>();
                var dataTable = reader.ReadFile(unzippedData);
                Utility.UtilityClass.WriteLog("file read completed");

                byte[] oldData = File.ReadAllBytes(previousUrl);

                var oldUnzippedData = unzipper.Decompress(oldData);

                Utility.UtilityClass.WriteLog("Unzip completed");
                var oldDataTable = reader.ReadFile(oldUnzippedData);
                Utility.UtilityClass.WriteLog("file read completed");

                var filteredData = dataTable.Where(x => !oldDataTable.Any(y => y.Vin == x.Vin));

                var coordinates = NinjectConfig.Get<ICoordinates>();
                var geoLoc = NinjectConfig.Get<IFetchLatLong>();
                var autoapi = NinjectConfig.Get<IFetchAutoDetailsFromAPI>();
                Hashtable hashGeoinDB = new Hashtable();
                Hashtable hashGeoinlocal = new Hashtable();

                foreach (var newAuto in filteredData)
                {
                    //Step 5 : Get Vin Details from NewAuto
 
                    var temp = autoapi.GetAutoDetailsFromAPIByVin(newAuto.Vin);
                    if (temp != null)
                    {
                        newAuto.Category = temp.Category;
                        newAuto.Trim = temp.Trim;
                        newAuto.VehicleStyle = temp.VehicleStyle;
                        newAuto.VehicleSize = temp.VehicleSize;
                    }
                    else
                    {
                        newAuto.Category = "N.A.";
                        newAuto.Trim = "N.A.";
                        newAuto.VehicleStyle = "N.A.";
                        newAuto.VehicleSize = "N.A.";
                    }

                    //Step 6 : Get GeoCode Details from NewAuto 

                    string DealerAddress = !string.IsNullOrEmpty(newAuto.DealerAddress) ? newAuto.DealerAddress + "," : string.Empty;
                    string DealerCity = !string.IsNullOrEmpty(newAuto.DealerCity) ? newAuto.DealerCity + "," : string.Empty;
                    string DealershipZip = !string.IsNullOrEmpty(newAuto.DealerZip) ? newAuto.DealerZip : string.Empty;
                    string address = DealerAddress + DealerCity + DealershipZip;
                    address = (address.LastIndexOf(',') == address.Length - 1) ? address.Remove(address.Length - 1, 1) : address;
                    Location geoLocation = new Location();

                    bool isExistinlocal = false;
                    bool isExistinDB = false;
                    
                    List<double> lst = new List<double>();

                    #region Get Lat Lng
                    if (hashGeoinlocal.ContainsKey(address))
                    {
                        geoLocation = (Location)hashGeoinlocal[address];
                    }
                    else if (hashGeoinDB.ContainsKey(address))
                    {
                        geoLocation = (Location)hashGeoinDB[address];
                    }
                    else
                    {
                        Coordinates cor = coordinates.GetCoordinates(address);
                        if (cor != null)
                        {
                            isExistinDB = true;
                            isExistinlocal = false;
                            geoLocation.lng = cor.Coordinate[0];
                            geoLocation.lat = cor.Coordinate[1];
                        }
                        else
                        {
                            isExistinDB = false;
                            isExistinlocal = true;
                            geoLocation = geoLoc.GetLatitudeAndLongitude(address);
                        }
                    }
                    #endregion

                    if (isExistinlocal == true)
                    {
                        hashGeoinlocal.Add(address, geoLocation);
                    }
                    if (isExistinDB == true)
                    {
                        hashGeoinDB.Add(address, geoLocation);
                    }

                    if (geoLocation != null)
                    {
                        lst.Add(geoLocation.lng);
                        lst.Add(geoLocation.lat);
                        GeoPoint gp = new GeoPoint(geoLocation.lng, geoLocation.lat);
                        gp.Coordinates = lst;
                        newAuto.GeoLocation = gp;
                    }
                }

                #region Coordinates
                List<Coordinates> lstCor = new List<Coordinates>();
                if (hashGeoinlocal.Count > 0)
                {
                    foreach (DictionaryEntry item in hashGeoinlocal)
                    {
                        Coordinates c = new Coordinates();
                        c.Address = (string)item.Key;
                        Location loc = (Location)item.Value;
                        c.Coordinate = new double[] { loc.lng, loc.lat };
                        lstCor.Add(c);
                    }
                    var result = coordinates.InsertBulkCoordinates(lstCor);

                }
                #endregion

                Utility.UtilityClass.WriteLog("API completed");
                //-------------------------------------------------------

                var autoVertical = NinjectConfig.Get<IAutoVertical>();
                //autoVertical.ProcessFeed(filteredData);
            }
        }

        private static void RunProcess()
        {
            if (!Initialize())
            {
                Console.WriteLine("Error!!! Unable to initialize.");
                Console.ReadKey(true);
                return;
            }


            //Step 2 : Download---------------------------------------------------------------------------------------------------------------------------

            var folderPath = ConfigurationManager.AppSettings["path"];
            var remoteFtpPath = ConfigurationManager.AppSettings["FTPpath"];
            string fileName = "";
            Utility.UtilityClass.WriteLog("Download started");
            try
            {
                var downloader = NinjectConfig.Get<IDownloader>();
                fileName = downloader.DownloadFile(remoteFtpPath, folderPath);
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "DownloadFile");
                throw;
            }
            Utility.UtilityClass.WriteLog("Download completed");
            //fileName = "sdutfeed_20151216.txt.gz";


            if (!string.IsNullOrEmpty(fileName))
            {
                //Step 3 : Unzip Files And Write save to db-----------------------------------------------------------------------------------------------------------------------------

                var UnzipFilePath = @folderPath + fileName;
                byte[] data = File.ReadAllBytes(UnzipFilePath);

                var unzipper = NinjectConfig.Get<IUnzipper>();
                fileName = fileName.Split('.')[0];
                var unzippedData = unzipper.Decompress(data);

                Utility.UtilityClass.WriteLog("Unzip completed");
                var reader = NinjectConfig.Get<IReader>();
                var dataTable = reader.ReadFile(unzippedData);
                Utility.UtilityClass.WriteLog("file read completed");

                var coordinates = NinjectConfig.Get<ICoordinates>();
                var geoLoc = NinjectConfig.Get<IFetchLatLong>();
                var autoapi = NinjectConfig.Get<IFetchAutoDetailsFromAPI>();

                //------------------------------------Api----------------------------------------

                Hashtable hashGeoinDB = new Hashtable();
                Hashtable hashGeoinlocal = new Hashtable();


                foreach (var newAuto in dataTable)
                {
                    var temp = autoapi.GetAutoDetailsFromAPIByVin(newAuto.Vin);
                    // Auto temp = null;
                    if (temp != null)
                    {
                        newAuto.Category = temp.Category;
                        newAuto.Trim = temp.Trim;
                        newAuto.VehicleStyle = temp.VehicleStyle;
                        newAuto.VehicleSize = temp.VehicleSize;
                    }
                    else
                    {
                        newAuto.Category = "N.A.";
                        newAuto.Trim = "N.A.";
                        newAuto.VehicleStyle = "N.A.";
                        newAuto.VehicleSize = "N.A.";
                    }
                    // Bing Api ---------------------------------------------------------------


                    string DealerAddress = !string.IsNullOrEmpty(newAuto.DealerAddress) ? newAuto.DealerAddress + "," : string.Empty;
                    string DealerCity = !string.IsNullOrEmpty(newAuto.DealerCity) ? newAuto.DealerCity + "," : string.Empty;
                    string DealershipZip = !string.IsNullOrEmpty(newAuto.DealerZip) ? newAuto.DealerZip : string.Empty;
                    string address = DealerAddress + DealerCity + DealershipZip;
                    address = (address.LastIndexOf(',') == address.Length - 1) ? address.Remove(address.Length - 1, 1) : address;
                    Location geoLocation = new Location();

                    bool isExistinlocal = false;
                    bool isExistinDB = false;



                    List<double> lst = new List<double>();

                    #region Get Lat Lng
                    if (hashGeoinlocal.ContainsKey(address))
                    {
                        geoLocation = (Location)hashGeoinlocal[address];
                    }
                    else if (hashGeoinDB.ContainsKey(address))
                    {
                        geoLocation = (Location)hashGeoinDB[address];
                    }
                    else
                    {
                        Coordinates cor = coordinates.GetCoordinates(address);
                        if (cor != null)
                        {
                            isExistinDB = true;
                            isExistinlocal = false;
                            geoLocation.lng = cor.Coordinate[0];
                            geoLocation.lat = cor.Coordinate[1];
                        }
                        else
                        {
                            isExistinDB = false;
                            isExistinlocal = true;
                            geoLocation = geoLoc.GetLatitudeAndLongitude(address);
                        }
                    }
                    #endregion

                    if (isExistinlocal == true)
                    {
                        hashGeoinlocal.Add(address, geoLocation);
                    }
                    if (isExistinDB == true)
                    {
                        hashGeoinDB.Add(address, geoLocation);
                    }

                    if (geoLocation != null)
                    {
                        lst.Add(geoLocation.lng);
                        lst.Add(geoLocation.lat);
                        GeoPoint gp = new GeoPoint(geoLocation.lng, geoLocation.lat);
                        gp.Coordinates = lst;
                        newAuto.GeoLocation = gp;
                    }
                }

                #region Coordinates
                List<Coordinates> lstCor = new List<Coordinates>();
                if (hashGeoinlocal.Count > 0)
                {
                    foreach (DictionaryEntry item in hashGeoinlocal)
                    {
                        Coordinates c = new Coordinates();
                        c.Address = (string)item.Key;
                        Location loc = (Location)item.Value;
                        c.Coordinate = new double[] { loc.lng, loc.lat };
                        lstCor.Add(c);
                    }
                    var result = coordinates.InsertBulkCoordinates(lstCor);

                }
                #endregion
                Utility.UtilityClass.WriteLog("API completed");
                //-------------------------------------------------------------------------------


                var autoVertical = NinjectConfig.Get<IAutoVertical>();

                //------------------------------------Process feed--------------------------------------------------------

                var oldListing = autoVertical.GetAllAuto();

                autoVertical.ProcessFeed(dataTable);

                Utility.UtilityClass.WriteLog("Process feed completed");
                var newListing = dataTable;

                var newVin = newListing.Select(x => x.Vin);
                var oldVin = oldListing.Where(y => y.IsUpdatedByPortal != true).Select(x => x.Vin);

                try
                {
                    if (oldVin != null)
                    {
                        var propertynotlisted = oldVin.Except(newVin);

                        foreach (var mlsid in propertynotlisted)
                        {
                            autoVertical.DeleteProperty(mlsid, true);

                        }
                        Utility.UtilityClass.WriteLog("delete completed");
                    }
                }
                catch (Exception ex)
                {
                    Utility.UtilityClass.WriteLogException(ex, "DeleteProperty");
                    throw;
                }

                //------------------------------------Process feed--------------------------------------------------------


                #region user
                List<User> lstUser = new List<User>();
                var autoUser = NinjectConfig.Get<IDealer>();
                foreach (var item in dataTable)
                {
                    if (autoUser.GetDealerDetailsByName(item.DealerName) == null)
                    {
                        if (lstUser.FirstOrDefault(n => n.DealerName == item.DealerName) == null)
                        {
                            User u = new User();
                            u.DealerId = Utility.UtilityClass.GetUniqueKey();
                            u.DealerName = item.DealerName;
                            u.DealerAddress = item.DealerAddress;
                            u.DealerCity = item.DealerCity;
                            u.DealerZip = item.DealerZip;
                            u.DealerState = item.DealerState;
                            u.DealerEmail = item.DealerEmail;
                            u.DealerPhone = item.DealerPhone;
                            lstUser.Add(u);
                        }
                    }

                }

                try
                {
                    List<User> dealuerList = lstUser.Distinct().ToList();

                    var i = autoUser.InsertBulkDealers(dealuerList);
                    Utility.UtilityClass.WriteLog("dealer insert completed");
                }
                catch (Exception ex)
                {
                    Utility.UtilityClass.WriteLogException(ex, "InsertBulkDealers");
                    throw;
                }

                #endregion


            }

            #region ClassifiedFeed
            // ClassifiedFeedProgram.RunClassifiedFeedProcess();
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
