using AdminInterface.Models.Mail;
using Configuration;
using MongoDbRepository.Implementation;
using MongoDbRepository.Implementation.Dealer;
using Newtonsoft.Json;
using Repository.Interfaces;
using Repository.Interfaces.Admin.Auto;
using Repository.Interfaces.Admin.Dealer;
using Repository.Interfaces.Mail;
using Repository.Models.Admin.Auto;
using Repository.Models.Admin.Dealer;
using Repository.Models.DataTable;
using Repository.Models.ViewModel;
using Security.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utility;

namespace AdminInterface.Controllers
{
    [RoutePrefix("auto")]
    public class AutoController : BaseController
    {


        private readonly IAuto _auto;
        private IAutoVertical _autovertical;
        private readonly IDealer _dealer;
        public AutoController(IAuto auto)
        {
            _auto = auto;
            _autovertical = new AutoVertical();
            _dealer = new DealerHandler();
        }

        #region Listing
        // [CustomAuthorize(Roles = "Admin")]
        [Route("auto-listing")]
        public ActionResult AutoListing()
        {
            return View();
        }

        // [CustomAuthorize(Roles = "Admin")]
        [Route("auto-listing-ajax-handler")]
        public ActionResult UserAjaxHandler(JQueryDataTableParamModel param)
        {
            long filterCount = 0;
            var listHubPropertysearchCriteria = new AutoDataTable()
            {
                isVinSearchable = Convert.ToBoolean(Request["bSearchable_0"]),
                isMakeSearchable = Convert.ToBoolean(Request["bSearchable_1"]),
                isModelSearchable = Convert.ToBoolean(Request["bSearchable_2"]),
                isPriceSearchable = Convert.ToBoolean(Request["bSearchable_3"]),
                isConditionSearchable = Convert.ToBoolean(Request["bSearchable_4"]),
                isMileageSearchable = Convert.ToBoolean(Request["bSearchable_5"]),
                isStockNoSearchable = Convert.ToBoolean(Request["bSearchable_6"]),
                isTransmissionSearchable = Convert.ToBoolean(Request["bSearchable_7"]),
                isInteriorColorSearchable = Convert.ToBoolean(Request["bSearchable_8"]),
                isExteriorColorSearchable = Convert.ToBoolean(Request["bSearchable_9"]),
                isLocationSearchable = Convert.ToBoolean(Request["bSearchable_10"]),


                isVinSortable = Convert.ToBoolean(Request["bSortable_0"]),
                isMakeSortable = Convert.ToBoolean(Request["bSortable_1"]),
                isModelSortable = Convert.ToBoolean(Request["bSortable_2"]),
                isPriceSortable = Convert.ToBoolean(Request["bSortable_3"]),
                isConditionSortable = Convert.ToBoolean(Request["bSortable_4"]),
                isMileageSortable = Convert.ToBoolean(Request["bSortable_5"]),
                isStockNoSortable = Convert.ToBoolean(Request["bSortable_6"]),
                isTransmissionSortable = Convert.ToBoolean(Request["bSortable_7"]),
                isInteriorColorSortable = Convert.ToBoolean(Request["bSortable_8"]),
                isExteriorColorSortable = Convert.ToBoolean(Request["bSortable_9"]),
                isLocationSortable = Convert.ToBoolean(Request["bSortable_10"]),


                sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]),
                sortDirection = Request["sSortDir_0"]
            };

            var filteredData = _auto.GetDataSet("", param, listHubPropertysearchCriteria, out filterCount, "purchase");

            var totalCount = _auto.GetTotalCount("");



            var result = from c in filteredData
                         select
                             new string[]
                             {
                         c.Vin,
                         c.Make,
                         c.Model,
                         c.Price.ToString(),
                         c.Condition,
                         c.Mileage.ToString(),
                         c.StockNumber,
                         //c.Transmission,
                         c.InteriorColor,
                         c.ExteriorColor,
                         c.DealerAddress,
                             };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filterCount,
                aaData = result
            },
                JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit


        [Route("{type}/edit/{vin}")]
        public ActionResult Add(string vin)
        {
            Repository.Models.Admin.Auto.Auto newAuto = _autovertical.GetAutoDetailsByVin(vin);
            //var x = User.Identity;

            string role = "";
            foreach (string item in User.Roles)
            {
                if (item == "Admin")
                {
                    role = "Admin";
                }
                else if (item == "Dealer" && role != "Admin")
                {
                    role = "Dealer";
                }
            }
            if (role == "Admin")
            {
                var lstUser = _dealer.GetDealerDetails();
                ViewBag.DealerName = lstUser;
                User dealer = _dealer.GetDealerDetailsByName(User.DealerName);
                newAuto.DealerId = dealer.DealerId;
                //newAuto.DealerId=User.
            }
            else if (role == "Dealer")
            {
                List<User> lstDealers = new List<User>();
                User dealer = _dealer.GetDealerDetailsByName(User.DealerName);
                lstDealers.Add(dealer);
                ViewBag.DealerName = lstDealers;
                //User dealer = _dealer.GetDealerDetailsByName(User.DealerName);
                newAuto.DealerId = dealer.DealerId;
            }


            ViewBag.isEdit = "T";

            return View(newAuto);
        }

        [Route("edit-auto")]
        [HttpPost]
        public JsonResult EditAuto(Auto objAuto)
        {



            User dealer = _dealer.GetDealerDetailsByName(objAuto.DealerName);

            // User dealer = new User();
            objAuto.DealerId = dealer.DealerId;
            objAuto.DealerName = dealer.DealerName;
            objAuto.DealerAddress = dealer.DealerAddress;
            objAuto.DealerCity = dealer.DealerCity;
            objAuto.DealerState = dealer.DealerState;
            objAuto.DealerZip = dealer.DealerZip;
            objAuto.DealerPhone = dealer.DealerPhone;
            objAuto.DealerEmail = dealer.DealerEmail;
            objAuto.ChatScript = dealer.ChatScript;
            objAuto.IsUpdatedByPortal = true;
            // objAuto.dealerbi
            bool IsInserted = _auto.UpdateAuto(objAuto);

            //#if DEBUG
            //            user.DealerEmail = "kaijar.habib@indusnet.co.in";//ankit.sarkar@indusnet.co.in,edwindantas@gmail.com,vikas@ntooitive.com";
            //#endif

            //            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates");
            //            var razorTemplateFileLocation = Path.Combine(templateFolderPath, "send_invitation.cshtml");
            //            var razorText = System.IO.File.ReadAllText(razorTemplateFileLocation);
            //            var model = new SendInvitation { FuzzyLink = LinkCreator.CreateAgentInvitationLink(user.DealerId) };
            //            var templateService = new TemplateService();
            //            var emailHtmlBody = templateService.Parse(razorText, model, null, null);


            //            var result = _mailBase.SendMail(new[] { user.DealerEmail }, "Next Step: Activate Your Account", emailHtmlBody);


            return Json(new { success = IsInserted, ParticipantId = objAuto.Vin });
        }

        #endregion

        #region Delete
        [Route("delete/{uniquid}/{isedit}")]

        public JsonResult DeleteAuto(string uniquid, bool isedit)
        {
            bool IsInserted = _auto.DeleteAuto(uniquid, isedit);
            return Json(new { success = true, ParticipantId = uniquid });
        }

        #endregion

        #region Manage


        [HttpGet]
        [Route("{type}/manage/{vin}")]
        public ActionResult EditAuto(string type, string vin)
        {
            var extraPropertyDetails = _autovertical.GetExtraProperty(type, vin);
            var ListedBy = _autovertical.GetListedType(type, vin);
            var autoDetails = _autovertical.GetAutoDetailsByVin(vin);

            ViewBag.ListedBy = ListedBy;
            if (extraPropertyDetails != null)
            {
                extraPropertyDetails.UniqueId = vin;
                extraPropertyDetails.Type = type;
            }
            else
            {
                extraPropertyDetails = new ManageAutoViewModel { UniqueId = vin, Type = type};
            }

            if (autoDetails!= null)
            {
                extraPropertyDetails.PhotosUrl = autoDetails.PhotosUrl;
            }
            else
            {
                extraPropertyDetails.PhotosUrl = new List<string>();
            }

            return View(extraPropertyDetails);
        }

        [Route("{type}/update")]
        [HttpPost]
        public ActionResult EditProperty(string type, ManageAutoViewModel viewModel)
        {
            var updateProperty = _autovertical.SetExtraProperty(type, viewModel);
            if (type.ToLower() == "auto")
            {
                var update = _autovertical.UpdateAutoSpotFeatured(viewModel.UniqueId, viewModel);

            }
            else if (type.ToLower() == "newhome")
            {
                //Class is not created for elastic search
                //var newhome = _ElasticSearchIndicesNewHome.Get(viewModel.UniqueId);
                //newhome.IsFeatured = viewModel.IsFeatured;
                //newhome.IsSpotlight = viewModel.IsSpotlight;
                //_ElasticSearchIndicesNewHome.UpdateIndex(newhome);
            }

            return Json(updateProperty);
        }

        ////[CustomAuthorize(Roles = "Admin")]
        //[Route("dealer-marketval/{uniquid}")]
        //public ActionResult MarketVal(string uniquid, bool isActivated)
        //{
        //    var deactivateUser = _dealer.MarketVal(uniquid, isActivated);

        //    var message = deactivateUser
        //        ? "Show Market Value Updated Successfully"
        //        : "Unable to Update Show Market Value. Please try again later.";


        //    return Json(new
        //    {
        //        Message = message
        //    },
        //        JsonRequestBehavior.AllowGet);
        //}


        ////[CustomAuthorize(Roles = "Admin")]
        //[Route("dealer-ownershipcost/{uniquid}")]
        //public ActionResult OwnershipCost(string uniquid, bool isActivated)
        //{
        //    var deactivateUser = _dealer.OwnershipCost(uniquid, isActivated);

        //    var message = deactivateUser
        //        ? "Show Ownership Cost Updated Successfully"
        //        : "Unable to Update Show Ownership Cost. Please try again later.";


        //    return Json(new
        //    {
        //        Message = message
        //    },
        //        JsonRequestBehavior.AllowGet);
        //}


        [Route("{type}/updateimage")]
        [HttpPost]
        public ActionResult EditImageList(string type, Repository.Models.Admin.Auto.Auto auto)
        {
           // var url = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + "/Car/" + uniqueId + "/" + fileName;

            var updateProperty = _autovertical.UpdateImageList(type, auto);
            return Json(updateProperty);
        
        }

        [Route("{vin}/{imageurl}")]
        [HttpPost]
        public ActionResult DeleteImageList(string vin,string imageurl)
        {
            var url = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority;
            var  file = imageurl.Replace(url,"");
            var fullpath = Server.MapPath(file);
            if (Directory.Exists(Path.GetDirectoryName(fullpath)))
            {
                System.IO.File.Delete(fullpath);
            }
            var updateProperty = _autovertical.PullImage(imageurl, vin);
            return Json(updateProperty);

        }

        [Route("getdetails/{type}/{uniqueid}")]
        public ActionResult GetDetails(string type, string uniqueid)
        {

            return Json(_autovertical.GetAutoDetailsByVin(uniqueid), JsonRequestBehavior.AllowGet);


        }

        #endregion




        #region Add
        //[CustomAuthorize(Roles = "Admin")]
        [Route("new-auto")]
        public ActionResult Add()
        {
            Repository.Models.Admin.Auto.Auto newAuto = new Repository.Models.Admin.Auto.Auto();

            //var x = User.Identity;

            string role = "";
            foreach (string item in User.Roles)
            {
                if (item == "Admin")
                {
                    role = "Admin";
                }
                else if (item == "Dealer" && role != "Admin")
                {
                    role = "Dealer";
                }
            }
            if (role == "Admin")
            {
                var lstUser = _dealer.GetDealerDetails();
                ViewBag.DealerName = lstUser;
            }
            else if (role == "Dealer")
            {
                List<User> lstDealers = new List<User>();
                User dealer = _dealer.GetDealerDetailsByName(User.DealerName);
                lstDealers.Add(dealer);
                ViewBag.DealerName = lstDealers;
            }


            //newAuto.StateList = _auto.GetStateList().ToList().Where(m => !string.IsNullOrEmpty(m.StateOrProvince)).ToList();
            //State st = new State();
            //st.StateOrProvince = "Other";
            //newOffc.StateList.Add(st);

            //newOffc.CityList = _office.GetCityList().ToList();
            //Cities ct = new Cities();
            //ct.City = "Other";
            //newOffc.CityList.Add(ct);

            //newOffc.StreetList = _office.GetStreetAddressList().ToList();
            //StreetAddress sa = new StreetAddress();
            //sa.FullStreetAddress = "Other";
            //newOffc.StreetList.Add(sa);

            //newOffc.ZipCodeList = _office.GetZipCodeList().ToList();
            //ZipCode zc = new ZipCode();
            //zc.PostalCode = "Other";
            //newOffc.ZipCodeList.Add(zc);
            ViewBag.isEdit = "F";
            return View(newAuto);
        }

        //[HttpGet]
        //[Route("citylist")]
        //public JsonResult CityList(string StateName)
        //{
        //    try
        //    {
        //        NewOffice newOffc = new NewOffice();
        //        newOffc.CityList = _office.GetCityList(StateName).ToList();
        //        Cities ct = new Cities();
        //        ct.City = "Other";
        //        newOffc.CityList.Add(ct);
        //        return Json(newOffc.CityList, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.message = ex.Message;
        //        throw ex;
        //    }
        //}

        //[HttpGet]
        //[Route("zipcodelist")]
        //public JsonResult ZipCodeList(string CityName)
        //{
        //    try
        //    {
        //        NewOffice newOffc = new NewOffice();
        //        newOffc.ZipCodeList = _office.GetZipCodeList(CityName).ToList();
        //        ZipCode zc = new ZipCode();
        //        zc.PostalCode = "Other";
        //        newOffc.ZipCodeList.Add(zc);
        //        return Json(newOffc.ZipCodeList, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.message = ex.Message;
        //        throw ex;
        //    }
        //}

        //[HttpGet]
        //[Route("fullstreetaddresslist")]
        //public JsonResult FullStreetAddressList(string CityName)
        //{
        //    try
        //    {
        //        NewOffice newOffc = new NewOffice();
        //        newOffc.StreetList = _office.GetStreetAddressList(CityName).ToList();
        //        StreetAddress sa = new StreetAddress();
        //        sa.FullStreetAddress = "Other";
        //        newOffc.StreetList.Add(sa);
        //        return Json(newOffc.StreetList, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.message = ex.Message;
        //        throw ex;
        //    }
        //}

        // [CustomAuthorize(Roles = "Admin")]
        [Route("add-auto")]
        public JsonResult AddAuto(Repository.Models.Admin.Auto.Auto newAuto)
        {
            Auto auto0 = _auto.GetAutoDetails(newAuto.Vin);
            if (auto0 == null)
            {
                Repository.Models.Admin.Auto.Auto auto = new Repository.Models.Admin.Auto.Auto();
                auto.Make = newAuto.Make;
                auto.Model = newAuto.Model;
                auto.Trim = newAuto.Trim;
                auto.Year = newAuto.Year;
                auto.Vin = newAuto.Vin;
                auto.Mileage = newAuto.Mileage;
                auto.Price = newAuto.Price;
                auto.Condition = newAuto.Condition;
                auto.InteriorColor = newAuto.InteriorColor;
                auto.ExteriorColor = newAuto.ExteriorColor;
                auto.Description = newAuto.Description;
                auto.PhotosUrl = newAuto.PhotosUrl;
                auto.StockNumber = newAuto.StockNumber;
                auto.Transmission = newAuto.Transmission;
                auto.StockNumber = newAuto.StockNumber;
                auto.StockNumber = newAuto.StockNumber;
                auto.Category = newAuto.Category;
                auto.VehicleSize = newAuto.VehicleSize;
                auto.VehicleStyle = newAuto.VehicleStyle;


                User dealer = _dealer.GetDealerDetailsByName(newAuto.DealerName);

                // User dealer = new User();
                auto.DealerId = dealer.DealerId;
                auto.DealerName = dealer.DealerName;
                auto.DealerAddress = dealer.DealerAddress;
                auto.DealerCity = dealer.DealerCity;
                auto.DealerState = dealer.DealerState;
                auto.DealerZip = dealer.DealerZip;
                auto.DealerPhone = dealer.DealerPhone;
                auto.DealerEmail = dealer.DealerEmail;
                auto.ChatScript = dealer.ChatScript;
                auto.IsUpdatedByPortal = true;
                //auto.Role = dealer.Role;
                //auto.DealerId = dealer.DealerId;
                //auto.DealerId = dealer.DealerId;

                //Address addr = new Address();
                //addr.Country = "US";
                //addr.City = newAuto.Address.City;
                //addr.FullStreetAddress = newAuto.Address.FullStreetAddress;
                //addr.StateOrProvince = newAuto.Address.StateOrProvince;
                //addr.PostalCode = newAuto.Address.PostalCode;
                //auto.Address = addr;
                //auto.OfficeEmail = newAuto.OfficeEmail;
                //auto.Website = newAuto.Website;
                _autovertical = NinjectConfig.Get<IAutoVertical>();
                List<Repository.Models.Admin.Auto.Auto> autoList = new List<Repository.Models.Admin.Auto.Auto>();
                autoList.Add(auto);
                bool IsInserted = _autovertical.InsertBulkAutoVertical(autoList);

                return Json(new { success = IsInserted, ParticipantId = auto.Vin });

            }
            else
            {
                return Json(new { success = false, ParticipantId = "", IsDuplicate = "T" });
            }


        }
        #endregion




        //[Route("{type}/deleteproperty")]
        //[HttpPost]
        //public ActionResult DeleteProperty(string type, string MlsNumber, bool isDeleted)
        //{
        //    var updateProperty = type == "newhome" ? _newHomeMain.DeleteProperty(MlsNumber, isDeleted) : _listHub.DeleteProperty(type, MlsNumber, isDeleted);
        //    return Json(updateProperty);
        //}

        //#region FileUpload
        //private string StorageRoot
        //{
        //    get { return Path.Combine(Server.MapPath("~/Files")); }
        //}

        ////DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //[AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        //public void Delete(string id)
        //{
        //    var filename = id;
        //    var filePath = Path.Combine(Server.MapPath("~/Files"), filename);

        //    if (System.IO.File.Exists(filePath))
        //    {
        //        System.IO.File.Delete(filePath);
        //    }
        //}

        ////DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //[HttpGet]
        //public void Download(string id)
        //{
        //    var filename = id;
        //    var filePath = Path.Combine(Server.MapPath("~/Files"), filename);

        //    var context = HttpContext;

        //    if (System.IO.File.Exists(filePath))
        //    {
        //        context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
        //        context.Response.ContentType = "application/octet-stream";
        //        context.Response.ClearContent();
        //        context.Response.WriteFile(filePath);
        //    }
        //    else
        //        context.Response.StatusCode = 404;
        //}

        //[HttpPost]
        //public ActionResult UploadFiles()
        //{
        //    var r = new List<ViewDataUploadFilesResult>();

        //    foreach (string file in Request.Files)
        //    {
        //        var statuses = new List<ViewDataUploadFilesResult>();
        //        var headers = Request.Headers;

        //        if (string.IsNullOrEmpty(headers["X-File-Name"]))
        //        {
        //            UploadWholeFile(Request, statuses);
        //        }
        //        else
        //        {
        //            UploadPartialFile(headers["X-File-Name"], Request, statuses);
        //        }

        //        JsonResult result = Json(statuses);
        //        result.ContentType = "text/plain";

        //        return result;
        //    }

        //    return Json(r);
        //}

        //private string EncodeFile(string fileName)
        //{
        //    return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        //}

        ////DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        ////Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        //private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        //{
        //    if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
        //    var file = request.Files[0];
        //    var inputStream = file.InputStream;

        //    var fullName = Path.Combine(StorageRoot, Path.GetFileName(fileName));

        //    using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
        //    {
        //        var buffer = new byte[1024];

        //        var l = inputStream.Read(buffer, 0, 1024);
        //        while (l > 0)
        //        {
        //            fs.Write(buffer, 0, l);
        //            l = inputStream.Read(buffer, 0, 1024);
        //        }
        //        fs.Flush();
        //        fs.Close();
        //    }
        //    statuses.Add(new ViewDataUploadFilesResult()
        //    {
        //        name = fileName,
        //        size = file.ContentLength,
        //        type = file.ContentType,
        //        url = "/Home/Download/" + fileName,
        //        delete_url = "/Home/Delete/" + fileName,
        //        thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
        //        delete_type = "GET",
        //    });
        //}

        ////DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        ////Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        //private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        //{
        //    for (int i = 0; i < request.Files.Count; i++)
        //    {
        //        var file = request.Files[i];

        //        var fullPath = Path.Combine(StorageRoot, Path.GetFileName(file.FileName));

        //        file.SaveAs(fullPath);

        //        statuses.Add(new ViewDataUploadFilesResult()
        //        {
        //            name = file.FileName,
        //            size = file.ContentLength,
        //            type = file.ContentType,
        //            url = "/Home/Download/" + file.FileName,
        //            delete_url = "/Home/Delete/" + file.FileName,
        //            thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
        //            delete_type = "GET",
        //        });
        //    }
        //}
        //#endregion








    }
}