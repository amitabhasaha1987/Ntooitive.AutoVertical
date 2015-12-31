using Configuration;
using PagedList;
using Repository.Interfaces;
using Repository.Interfaces.Admin.Dealer;
using Repository.Interfaces.Mail;
using Repository.Models;
using Repository.Models.Admin.Auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class HomeController : Controller
    {
        readonly IAutoVertical _autoVerticalService;
        //readonly IMailBase _mailService;
        public HomeController(IAutoVertical autoVerticalService)
        {
            _autoVerticalService = autoVerticalService;
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            IndexModelView indexModelView = new IndexModelView();
            indexModelView.MakeList = _autoVerticalService.GetMakeRecords().ToList();
            indexModelView.VehicleTypeList = _autoVerticalService.GetVehicleTypeRecords().ToList();
            indexModelView.ModelList = _autoVerticalService.GetModelRecords().ToList();
            indexModelView.UserList = _autoVerticalService.GetUserRecords().ToList();

            return View(indexModelView);
        }

        [HttpGet]
        public JsonResult TypeList(string ID)
        {
            try
            {
                return Json(_autoVerticalService.GetVehicleTypeRecords(ID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult ModelList(string ID, string MakeId)
        {
            try
            {
                return Json(_autoVerticalService.GetModelRecords(ID, MakeId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult ModelListByMake(string MakeId)
        {
            try
            {
                return Json(_autoVerticalService.GetModelRecords(MakeId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                throw ex;
            }
        }

        public JsonResult GetAutoCompleteResult(string term)
        {
            var lst = _autoVerticalService.GetAutoCompleteDetails(term).Select(m => m.SearchResult.ToList()).FirstOrDefault();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CarListing(string txtSearch, string DealerName, string Name, string Type, string ModelName, string Location, string ZipCode, string ExteriorColor, string InteriorColor, string MinPrice, string MaxPrice, string Mileage, string FromYear, string ToYear, string SortBy, IndexModelView indexModelView, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(DealerName))
            {
                List<CarListing> carList = new List<CarListing>();
                ListingViewModel listingViewModel = new Models.ListingViewModel();
                int startIndex = (page - 1) * pageSize;
                int limit = pageSize;
                AdvanceSearch advSearch = new AdvanceSearch();

                if (!string.IsNullOrEmpty(txtSearch))
                {
                    Name = txtSearch.Split(' ')[0];
                    ModelName = txtSearch.Split(' ')[1];
                }

                #region Make
                if (!string.IsNullOrEmpty(Name))
                {
                    List<Make> lstMake = new List<Make>();
                    if (Name.Contains(','))
                    {
                        foreach (var item in Name.Split(','))
                        {
                            Make m = new Make();
                            m.MakersName = item;
                            lstMake.Add(m);
                        }
                    }
                    else
                    {
                        Make m = new Make();
                        m.MakersName = Name;
                        lstMake.Add(m);
                    }
                    advSearch.Makes = lstMake;
                    // advSearch.Make = Name;
                }
                #endregion


                #region Type
                if (!string.IsNullOrEmpty(Type))
                {
                    advSearch.VehicleType = Type;
                }
                #endregion

                #region Model
                if (!string.IsNullOrEmpty(ModelName))
                {
                    List<Model> modelLst = new List<Model>();
                    if (ModelName.Contains(','))
                    {
                        foreach (var item in ModelName.Split(','))
                        {
                            Model m = new Model();
                            m.ModelsName = item;
                            modelLst.Add(m);
                        }
                    }
                    else
                    {
                        Model m = new Model();
                        m.ModelsName = ModelName;
                        modelLst.Add(m);
                    }
                    advSearch.ModelList = modelLst;
                }
                #endregion

                #region Location
                advSearch.Location = Location;
                #endregion

                #region ZipCode
                advSearch.ZipCode = ZipCode;
                #endregion

                #region ExteriorColor
                if (!string.IsNullOrEmpty(ExteriorColor))
                {
                    advSearch.ExSelectedColor = ExteriorColor;
                }
                #endregion

                #region InteriorColor
                if (!string.IsNullOrEmpty(InteriorColor))
                {
                    advSearch.InSelectedColor = InteriorColor;
                }
                #endregion

                #region Price
                advSearch.MinPrice = string.IsNullOrEmpty(MinPrice) ? "0" : MinPrice;
                advSearch.MaxPrice = string.IsNullOrEmpty(MaxPrice) ? "0" : MaxPrice;
                #endregion

                #region Mileage
                advSearch.Mileage = string.IsNullOrEmpty(Mileage) ? "0" : Mileage;
                #endregion

                #region Year
                advSearch.FromYear = FromYear;
                advSearch.ToYear = ToYear;
                #endregion

                #region Sort
                advSearch.SortingOrder = string.IsNullOrEmpty(SortBy) ? "0" : SortBy;
                #endregion

                #region GenerateList
                listingViewModel.CarListing = _autoVerticalService.GetVehicleList(advSearch, startIndex, limit).ToList();
                carList = listingViewModel.CarListing.ToList();
                listingViewModel.RecordCount = _autoVerticalService.GetVehicleRecordCount(advSearch);
                #endregion

                #region YearList
                List<CarYears> lstCarYears = _autoVerticalService.GetCarYear();
                advSearch.YearList = lstCarYears.FirstOrDefault().Years.OrderByDescending(x => x).ToList();
                #endregion

                ViewBag.Type = Type;
                ViewBag.Name = Name;

                #region Paging
                StaticPagedList<CarListing> staticPagedList = new StaticPagedList<CarListing>(carList, page, pageSize, listingViewModel.RecordCount);
                listingViewModel.CarPageListings = staticPagedList;
                #endregion

                #region SelectedMake With List
                advSearch.Makes = _autoVerticalService.GetMakeRecords().ToList();
                if (advSearch.Makes != null && !string.IsNullOrEmpty(Name))
                {
                    advSearch.Makes = advSearch.Makes.Where(m => m != null).ToList();
                    foreach (var item in advSearch.Makes)
                    {
                        if (item != null && !string.IsNullOrEmpty(item.MakersName))
                        {
                            if (!Name.Contains(','))
                            {
                                if (item.MakersName == Name)
                                {
                                    item.IsSelected = true;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < Name.Split(',').Count(); i++)
                                {
                                    if (item.MakersName == Name.Split(',')[i])
                                    {
                                        item.IsSelected = true;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Selected Model With List
                List<Model> lstModel = new List<Model>();
                if (indexModelView.advanceSearch != null && !string.IsNullOrEmpty(indexModelView.advanceSearch.Make))
                {
                    lstModel = _autoVerticalService.GetModelRecords(Type, indexModelView.advanceSearch.Make).ToList();
                    lstModel = returnModel(lstModel, ModelName);
                    Dictionary<string, List<Model>> dic = new Dictionary<string, List<Model>>();
                    dic.Add(indexModelView.advanceSearch.Make, lstModel);
                    advSearch.Models = dic;
                }
                else
                {
                    //lstModel = _autoVerticalService.GetModelRecords(Type == "0" ? null : Type, Name).ToList();
                    Type = "0";
                    lstModel = _autoVerticalService.GetModelRecords(Type == "0" ? null : Type, Name).ToList();
                    if (!string.IsNullOrEmpty(Name))
                    {
                        if (!Name.Contains(','))
                        {
                            lstModel = returnModel(lstModel, ModelName);
                            Dictionary<string, List<Model>> dic = new Dictionary<string, List<Model>>();
                            dic.Add(Name, lstModel);
                            advSearch.Models = dic;
                        }
                        else
                        {
                            Dictionary<string, List<Model>> dic = new Dictionary<string, List<Model>>();
                            for (int i = 0; i < Name.Split(',').Count(); i++)
                            {
                                List<Model> lstModel1 = _autoVerticalService.GetModelRecords(Type == "0" ? null : Type, Name.Split(',')[i]).ToList();
                                dic.Add(Name.Split(',')[i], returnModel(lstModel1, ModelName));
                            }
                            advSearch.Models = dic;
                        }
                    }
                }
                #endregion

                #region Generate Color List
                if (!string.IsNullOrEmpty(Type) && Type != "0")
                {
                    advSearch.ExteriorColor = _autoVerticalService.GetExteriorColor(null).FirstOrDefault() != null ? _autoVerticalService.GetExteriorColor(null).FirstOrDefault().Colors : new List<string>();
                    advSearch.ExSelectedColor = ExteriorColor;
                    advSearch.InteriorColor = _autoVerticalService.GetInteriorColor(null).FirstOrDefault() != null ? _autoVerticalService.GetInteriorColor(null).FirstOrDefault().Colors : new List<string>();
                    advSearch.InSelectedColor = InteriorColor;
                }
                else if (indexModelView.advanceSearch != null && !string.IsNullOrEmpty(indexModelView.advanceSearch.Make))
                {
                    advSearch.ExteriorColor = _autoVerticalService.GetExteriorColor(null).FirstOrDefault().Colors;
                    advSearch.ExSelectedColor = ExteriorColor;
                    advSearch.InteriorColor = _autoVerticalService.GetInteriorColor(null).FirstOrDefault().Colors;
                    advSearch.InSelectedColor = InteriorColor;
                }
                else
                {
                    advSearch.ExteriorColor = _autoVerticalService.GetExteriorColor(null).FirstOrDefault().Colors;
                    advSearch.ExSelectedColor = ExteriorColor;
                    if (!string.IsNullOrEmpty(Name))
                    {
                        if (_autoVerticalService.GetInteriorColor(Name).FirstOrDefault() != null)
                        {
                            advSearch.InteriorColor = _autoVerticalService.GetInteriorColor(Name).FirstOrDefault().Colors;

                        }
                        else
                        {
                            advSearch.InteriorColor = new List<string>();

                        }

                    }
                    else
                    {
                        if (_autoVerticalService.GetInteriorColor(null) != null)
                        {
                            advSearch.InteriorColor = _autoVerticalService.GetInteriorColor(null).FirstOrDefault().Colors;

                        }
                        else
                        {
                            advSearch.InteriorColor = new List<string>();

                        }
                        advSearch.InSelectedColor = InteriorColor;
                    }
                }
                #endregion


                listingViewModel.advSearch = advSearch;

                return View(listingViewModel);
            }
            else
            {
                List<CarListing> carList = new List<CarListing>();
                ListingViewModel listingViewModel = new Models.ListingViewModel();
                int startIndex = (page - 1) * pageSize;
                int limit = pageSize;
                AdvanceSearch advSearch = new AdvanceSearch();

                if (!string.IsNullOrEmpty(txtSearch))
                {
                    Name = txtSearch.Split(' ')[0];
                    ModelName = txtSearch.Split(' ')[1];
                }

                #region Dealer
                if (!string.IsNullOrEmpty(DealerName))
                {
                    List<UserSrch> lstDealer = new List<UserSrch>();
                    if (DealerName.Contains(','))
                    {
                        foreach (var item in DealerName.Split(','))
                        {
                            UserSrch d = new UserSrch();
                            d.DealerName = item;
                            lstDealer.Add(d);
                        }
                    }
                    else
                    {
                        UserSrch d = new UserSrch();
                        d.DealerName = DealerName;
                        lstDealer.Add(d);
                    }
                    advSearch.Dealers = lstDealer;
                    if (lstDealer.Count == 1)
                    {
                        advSearch.Dealer = DealerName;
                    }
                }
                #endregion


                #region Make
                if (!string.IsNullOrEmpty(Name))
                {
                    List<Make> lstMake = new List<Make>();
                    if (Name.Contains(','))
                    {
                        foreach (var item in Name.Split(','))
                        {
                            Make m = new Make();
                            m.MakersName = item;
                            lstMake.Add(m);
                        }
                    }
                    else
                    {
                        Make m = new Make();
                        m.MakersName = Name;
                        lstMake.Add(m);
                    }
                    advSearch.Makes = lstMake;
                }
                #endregion

                #region Type
                if (!string.IsNullOrEmpty(Type))
                {
                    advSearch.VehicleType = Type;
                }
                #endregion

                #region Model
                if (!string.IsNullOrEmpty(ModelName))
                {
                    List<Model> modelLst = new List<Model>();
                    if (ModelName.Contains(','))
                    {
                        foreach (var item in ModelName.Split(','))
                        {
                            Model m = new Model();
                            m.ModelsName = item;
                            modelLst.Add(m);
                        }
                    }
                    else
                    {
                        Model m = new Model();
                        m.ModelsName = ModelName;
                        modelLst.Add(m);
                    }
                    advSearch.ModelList = modelLst;
                }
                #endregion

                #region Location
                advSearch.Location = Location;
                #endregion

                #region ZipCode
                advSearch.ZipCode = ZipCode;
                #endregion

                #region ExteriorColor
                if (!string.IsNullOrEmpty(ExteriorColor))
                {
                    advSearch.ExSelectedColor = ExteriorColor;
                }
                #endregion

                #region InteriorColor
                if (!string.IsNullOrEmpty(InteriorColor))
                {
                    advSearch.InSelectedColor = InteriorColor;
                }
                #endregion

                #region Price
                advSearch.MinPrice = string.IsNullOrEmpty(MinPrice) ? "0" : MinPrice;
                advSearch.MaxPrice = string.IsNullOrEmpty(MaxPrice) ? "0" : MaxPrice;
                #endregion

                #region Mileage
                advSearch.Mileage = string.IsNullOrEmpty(Mileage) ? "0" : Mileage;
                #endregion

                #region Year
                advSearch.FromYear = FromYear;
                advSearch.ToYear = ToYear;
                #endregion

                #region Sort
                advSearch.SortingOrder = string.IsNullOrEmpty(SortBy) ? "0" : SortBy;
                #endregion

                #region GenerateList
                listingViewModel.CarListing = _autoVerticalService.GetVehicleListForDealer(advSearch, startIndex, limit).ToList();
                carList = listingViewModel.CarListing.ToList();
                listingViewModel.RecordCount = _autoVerticalService.GetVehicleRecordCountForDealer(advSearch);

                //listingViewModel.ProfileImage=carList[0]
                #endregion

                #region YearList
                List<CarYears> lstCarYears = _autoVerticalService.GetCarYear();
                advSearch.YearList = lstCarYears.FirstOrDefault().Years.OrderByDescending(x => x).ToList();
                #endregion

                ViewBag.Type = Type;
                ViewBag.Name = Name;

                #region Paging
                StaticPagedList<CarListing> staticPagedList = new StaticPagedList<CarListing>(carList, page, pageSize, listingViewModel.RecordCount);
                listingViewModel.CarPageListings = staticPagedList;
                #endregion

                #region SelectedMake With List
                advSearch.Makes = _autoVerticalService.GetMakeRecords().ToList();
                if (advSearch.Makes != null && !string.IsNullOrEmpty(Name))
                {
                    advSearch.Makes = advSearch.Makes.Where(m => m != null).ToList();
                    foreach (var item in advSearch.Makes)
                    {
                        if (item != null && !string.IsNullOrEmpty(item.MakersName))
                        {
                            if (!Name.Contains(','))
                            {
                                if (item.MakersName == Name)
                                {
                                    item.IsSelected = true;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < Name.Split(',').Count(); i++)
                                {
                                    if (item.MakersName == Name.Split(',')[i])
                                    {
                                        item.IsSelected = true;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Selected Model With List
                List<Model> lstModel = new List<Model>();
                if (indexModelView.advanceSearch != null && !string.IsNullOrEmpty(indexModelView.advanceSearch.Make))
                {
                    lstModel = _autoVerticalService.GetModelRecords(Type, indexModelView.advanceSearch.Make).ToList();
                    lstModel = returnModel(lstModel, ModelName);
                    Dictionary<string, List<Model>> dic = new Dictionary<string, List<Model>>();
                    dic.Add(indexModelView.advanceSearch.Make, lstModel);
                    advSearch.Models = dic;
                }
                else
                {
                    lstModel = _autoVerticalService.GetModelRecords(Type == "0" ? null : Type, Name).ToList();
                    if (!string.IsNullOrEmpty(Name))
                    {
                        if (!Name.Contains(','))
                        {
                            lstModel = returnModel(lstModel, ModelName);
                            Dictionary<string, List<Model>> dic = new Dictionary<string, List<Model>>();
                            dic.Add(Name, lstModel);
                            advSearch.Models = dic;
                        }
                        else
                        {
                            Dictionary<string, List<Model>> dic = new Dictionary<string, List<Model>>();
                            for (int i = 0; i < Name.Split(',').Count(); i++)
                            {
                                List<Model> lstModel1 = _autoVerticalService.GetModelRecords(Type == "0" ? null : Type, Name.Split(',')[i]).ToList();
                                dic.Add(Name.Split(',')[i], returnModel(lstModel1, ModelName));
                            }
                            advSearch.Models = dic;
                        }
                    }
                }
                #endregion

                #region Generate Color List
                if (!string.IsNullOrEmpty(Type) && Type != "0")
                {
                    advSearch.ExteriorColor = _autoVerticalService.GetExteriorColor(null).FirstOrDefault() != null ? _autoVerticalService.GetExteriorColor(null).FirstOrDefault().Colors : new List<string>();
                    advSearch.ExSelectedColor = ExteriorColor;
                    advSearch.InteriorColor = _autoVerticalService.GetInteriorColor(null).FirstOrDefault() != null ? _autoVerticalService.GetInteriorColor(null).FirstOrDefault().Colors : new List<string>();
                    advSearch.InSelectedColor = InteriorColor;
                }
                else if (indexModelView.advanceSearch != null && !string.IsNullOrEmpty(indexModelView.advanceSearch.Make))
                {
                    advSearch.ExteriorColor = _autoVerticalService.GetExteriorColor(null).FirstOrDefault().Colors;
                    advSearch.ExSelectedColor = ExteriorColor;
                    advSearch.InteriorColor = _autoVerticalService.GetInteriorColor(null).FirstOrDefault().Colors;
                    advSearch.InSelectedColor = InteriorColor;
                }
                else
                {
                    advSearch.ExteriorColor = _autoVerticalService.GetExteriorColor(null).FirstOrDefault().Colors;
                    advSearch.ExSelectedColor = ExteriorColor;
                    if (!string.IsNullOrEmpty(Name))
                    {
                        if (_autoVerticalService.GetInteriorColor(Name).FirstOrDefault() != null)
                        {
                            advSearch.InteriorColor = _autoVerticalService.GetInteriorColor(Name).FirstOrDefault().Colors;

                        }
                        else
                        {
                            advSearch.InteriorColor = new List<string>();

                        }

                    }
                    else
                    {
                        if (_autoVerticalService.GetInteriorColor(null) != null)
                        {
                            advSearch.InteriorColor = _autoVerticalService.GetInteriorColor(null).FirstOrDefault().Colors;

                        }
                        else
                        {
                            advSearch.InteriorColor = new List<string>();

                        }
                        advSearch.InSelectedColor = InteriorColor;
                    }
                }
                #endregion

                listingViewModel.advSearch = advSearch;
                var dealer = NinjectConfig.Get<IDealer>(); ;
                listingViewModel.ProfileImage = dealer.GetDealerDetailsByName(DealerName).ProfileImage;
                return View(listingViewModel);
            }
        }

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //public ActionResult DealerListing(string txtSearch, string Name, string DealerName, string Type, string ModelName, string Location, string ZipCode, string ExteriorColor, string InteriorColor, string MinPrice, string MaxPrice, string Mileage, string FromYear, string ToYear, string SortBy, IndexModelView indexModelView, int page = 1, int pageSize = 10)
        //{

        //}

        public ActionResult CarDetails(string Id)
        {
            Auto auto = _autoVerticalService.GetAutoDetailsById(Id);
            //----------------------------------------Api-----------------------------------
            var marketval = NinjectConfig.Get<IFetchMarketValueFromAPIByVin>();
            var ownrshipcost = NinjectConfig.Get<IFetchOwnershipCostFromAPIByVin>();

            MarketValue objMarketVal = marketval.GetMarketValueByVin(auto.Vin);
            OwnershipCost objOwnershipCost = ownrshipcost.GetOwnershipCostByVin(auto.Vin);

            auto.AutoMarketValue = objMarketVal;
            auto.AutoOwnershipCost = objOwnershipCost;

            var dealer = NinjectConfig.Get<IDealer>(); ;
            //ViewBag.ShowMarketVal = dealer.GetDealerDetailsByName(auto.DealerName).ex;
            //ViewBag.ShowOwnershipCost = dealer.GetDealerDetailsByName(auto.DealerName).ShowOwnershipCost;

            //var temp = autoapi.GetAutoDetailsFromAPIByVin("1D7RB1CT8AS203937");
            //auto.Category = temp.Category;
            //auto.SubModel = temp.SubModel;

            //------------------------------------------------------------------------------
            if (auto.GeoLocation == null)
            {
                var latLongService = NinjectConfig.Get<IFetchLatLong>();
                var fetchedCoordinates =
                              latLongService.GetLatitudeAndLongitude("");
                GeoPoint gp = new GeoPoint(0, 0);
                auto.GeoLocation = gp;
            }
            return View(auto);
        }

        private List<Model> returnModel(List<Model> lstModel, string ModelName)
        {
            foreach (var item in lstModel)
            {
                if (item != null && !string.IsNullOrEmpty(item.ModelsName))
                {
                    if (!string.IsNullOrEmpty(ModelName))
                    {
                        if (!ModelName.Contains(','))
                        {
                            if (item.ModelsName == ModelName)
                            {
                                item.IsSelected = true;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ModelName.Split(',').Count(); i++)
                            {
                                if (item.ModelsName == ModelName.Split(',')[i])
                                {
                                    item.IsSelected = true;
                                }
                            }
                        }
                    }
                }
            }
            return lstModel;
        }

        public JsonResult SendMail(string Name, string Email, string ToMail, string Phone, string Zip, string Message, string Subject)
        {
            //ToMail = "abhratanu@indusnet.co.in";
            //UserContactAndFindDetails userContactAndFindDetails = new UserContactAndFindDetails();
            //userContactAndFindDetails.UserName = Name;
            //userContactAndFindDetails.Email = Email;
            //userContactAndFindDetails.PhoneNo = Phone;
            //userContactAndFindDetails.MlsNumber = MlsNumber;

            //_userContactDetails.InsertUpdateDetailsAgainstUser(userContactAndFindDetails, Command, SubCommand, SelectedRow);

            //#if DEBUG
            //            BrokerMail = "ankit.sarkar@indusnet.co.in,edwindantas@gmail.com,vikas@ntooitive.com";
            //#endif
            if (string.IsNullOrEmpty(ToMail))
            {
                ToMail = "vikas@ntooitive.com";

#if DEBUG
                ToMail = "vikas@ntooitive.com";
#endif
            }
            var _mailService = NinjectConfig.Get<IMailBase>();

            Message = Message + " <br /><br /> Phone : " + Phone;

            var isSuccess = _mailService.SendMail(new[] { ToMail }, Subject, Message,
                new[] { Email });
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
    }
}
