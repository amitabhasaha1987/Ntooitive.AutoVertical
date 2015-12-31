using AdminInterface.Models.Mail;
using Newtonsoft.Json;
using RazorEngine.Templating;
using Repository.Interfaces.Admin.Dealer;
using Repository.Interfaces.Mail;
using Repository.Models.Admin.Dealer;
using Repository.Models.DataTable;
using Repository.Models.ViewModel;
using Security.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utility;

namespace AdminInterface.Controllers
{
    [RoutePrefix("dealer")]
    public class DealerController : BaseController
    {
        private readonly IDealer _dealer;
        private readonly IMailBase _mailBase;


        public DealerController(IDealer dealer, IMailBase mailBase)//, 
        {
            _dealer = dealer;
            _mailBase = mailBase;
        }
        //
        // GET: /Dealer/

        #region only Admin Can Access
        #region List
        //[CustomAuthorize(Roles = "Admin")]
        [Route("dealer-listing")]
        public ActionResult UserListing()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin")]
        [Route("dealer-listing-ajax-handler")]
        public ActionResult UserAjaxHandler(JQueryDataTableParamModel param)
        {
            long filterCount = 0;

            var listHubPropertysearchCriteria = new DealerDataTable()
            {
                isParticipantIdSearchable = Convert.ToBoolean(Request["bSearchable_0"]),
                isFirstNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]),
                isLastNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]),
                isEmailSearchable = Convert.ToBoolean(Request["bSearchable_3"]),
                isPrimaryContactPhoneSearchable = Convert.ToBoolean(Request["bSearchable_4"]),
                isOfficePhoneSearchable = Convert.ToBoolean(Request["bSearchable_5"]),

                isParticipantIdSortable = Convert.ToBoolean(Request["bSortable_0"]),
                isFirstNameSortable = Convert.ToBoolean(Request["bSortable_1"]),
                isLastNameSortable = Convert.ToBoolean(Request["bSortable_2"]),
                isEmailSortable = Convert.ToBoolean(Request["bSortable_3"]),
                isPrimaryContactPhoneSortable = Convert.ToBoolean(Request["bSortable_4"]),
                isOfficePhoneSortable = Convert.ToBoolean(Request["bSortable_5"]),


                sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]),
                sortDirection = Request["sSortDir_0"]
            };

            var filteredData = _dealer.GetDataSet("", param, listHubPropertysearchCriteria, out filterCount, "purchase");

            var totalCount = _dealer.GetTotalCount("");

            var result = from c in filteredData
                         select
                             new[]
                             {
                         string.IsNullOrEmpty(c.DealerId)?"":c.DealerId,
                         c.DealerName,
                         c.DealerAddress,
                         string.IsNullOrEmpty(c.DealerEmail)?"":c.DealerEmail,
                         c.DealerZip,
                         c.DealerPhone,
                         string.Join(",",c.Role),
                         Convert.ToString(c.IsActive),
                         Convert.ToString(c.IsEmailSend),
                             };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filterCount,
                aaData = result.Distinct()
            },
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add
        //        //[CustomAuthorize(Roles = "Admin")]
        //        //[Route("manage/{uniquid}")]
        //        public ActionResult ManageUser(string uniquid)
        //        {
        //            var manageUser = _dealer.GetAgentDetails(uniquid);
        //            return View(manageUser);
        //        }

        //        [CustomAuthorize(Roles = "Admin")]
        //        [Route("Dealer-activation/{uniquid}")]
        //        public ActionResult SendActivationMail(string uniquid)
        //        {
        //            string message;
        //            var agentDetails = _dealer.GetAgentDetails(uniquid);

        //            if (agentDetails != null)
        //            {
        //                var actuallMail = agentDetails.Email;
        //#if DEBUG
        //                agentDetails.Email = "ankit.sarkar@indusnet.co.in,edwindantas@gmail.com,vikas@ntooitive.com";
        //#endif

        //                var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates");
        //                var razorTemplateFileLocation = Path.Combine(templateFolderPath, "send_invitation.cshtml");
        //                var razorText = System.IO.File.ReadAllText(razorTemplateFileLocation);
        //                var model = new SendInvitation { FuzzyLink = LinkCreator.CreateAgentInvitationLink(agentDetails.ParticipantId) };
        //                var templateService = new TemplateService();
        //                var emailHtmlBody = templateService.Parse(razorText, model, null, null);


        //                var result = _mailBase.SendMail(new[] { agentDetails.Email }, "Next Step: Activate Your Account", emailHtmlBody);
        //                if (result)
        //                {
        //                    var dbResult = _dealer.InitiateRegistration(agentDetails.ParticipantId, actuallMail);
        //                    message = dbResult
        //                        ? "Activation mail send successfully. Please ask your Dealer to check inbox for more detail."
        //                        : "Unable to send the mail. Please try again later.";
        //                }
        //                else
        //                {
        //                    message = "Unable to send the mail. Please try again later.";
        //                }

        //            }
        //            else
        //            {
        //                message = "Unable to send the mail. Please try again later.";

        //            }


        //            return Json(new
        //            {
        //                Message = message
        //            },
        //                JsonRequestBehavior.AllowGet);
        //        }

        //        [CustomAuthorize(Roles = "Admin")]
        //        [Route("Dealer-deactivation/{uniquid}")]
        //        public ActionResult DeactivateAccount(string uniquid, bool isActivated)
        //        {
        //            var deactivateUser = _dealer.DeactiveAgent(uniquid, isActivated);

        //            var message = deactivateUser
        //                ? "Activation mail send successfully. Please ask your Dealer to check inbox for more detail."
        //                : "Unable to send the mail. Please try again later.";


        //            return Json(new
        //            {
        //                Message = message
        //            },
        //                JsonRequestBehavior.AllowGet);
        //        }

        //[CustomAuthorize(Roles = "Admin")]
        [Route("new-dealer")]
        public ActionResult Add()
        {
            ViewBag.isEdit = "F";
            return View(new User());
        }

        //        [CustomAuthorize(Roles = "Admin")]
        [Route("new-dealer")]
        [HttpPost]
        public JsonResult AddAgent(User user)
        {
            User dealer = _dealer.GetDealerDetailsByName(user.DealerName);

            if (dealer == null)
            {
                //add Dealer.
                user.IsUpdatedByPortal = true;
                user.DealerId = Utility.UtilityClass.GetUniqueKey();
                //user.ParticipantKey = "3yd-SDRE-" + user.ParticipantId;
                user.InitiateDate = DateTime.Now;
                user.Role = "Listing";
                user.Roles = new string[] { Utility.Roles.Dealer.ToString() };
                List<User> userList = new List<Repository.Models.Admin.Dealer.User>();
                userList.Add(user);
                bool IsInserted = _dealer.InsertBulkDealers(userList);

#if DEBUG
                user.DealerEmail = "kaijar.habib@indusnet.co.in";//ankit.sarkar@indusnet.co.in,edwindantas@gmail.com,vikas@ntooitive.com";
#endif

                var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates");
                var razorTemplateFileLocation = Path.Combine(templateFolderPath, "send_invitation.cshtml");
                var razorText = System.IO.File.ReadAllText(razorTemplateFileLocation);
                var model = new SendInvitation { FuzzyLink = LinkCreator.CreateAgentInvitationLink(user.DealerId) };
                var templateService = new TemplateService();
                var emailHtmlBody = templateService.Parse(razorText, model, null, null);


                var result = _mailBase.SendMail(new[] { user.DealerEmail }, "Next Step: Activate Your Account", emailHtmlBody);


                return Json(new { success = true, ParticipantId = user.DealerId, IsDuplicate = "F" });

            }
            else
            {
                return Json(new { success = true, ParticipantId = user.DealerId, IsDuplicate = "T" });
            }

        }
        #endregion

        #region Manage
        //[CustomAuthorize(Roles = "Admin")]
        [Route("manage/{uniquid}")]
        public ActionResult ManageUser(string uniquid)
        {
            var manageUser = _dealer.GetDealerDetails(uniquid);
            return View(manageUser);
        }
        #endregion

        #region Edit
        [Route("edit/{uniquid}/{isedit}")]
        public ActionResult Add(string uniquid, bool isedit)
        {
            var manageUser = _dealer.GetDealerDetails(uniquid);
            ViewBag.isEdit = "T";
            return View(manageUser);
        }

        [Route("edit-dealer")]
        [HttpPost]
        public JsonResult EditDealer(User user)
        {
            User dealer0 = _dealer.GetDealerDetails(user.DealerId);

            if (dealer0.DealerName == user.DealerName)
            {
                user.IsUpdatedByPortal = true;
                bool IsInserted = _dealer.UpdateDealer(user);
                return Json(new { success = true, ParticipantId = user.DealerId, IsDuplicate = "F" });
            }
            else
            {
                User dealer = _dealer.GetDealerDetailsByName(user.DealerName);

                if (dealer == null)
                {
                    user.IsUpdatedByPortal = true;
                    bool IsInserted = _dealer.UpdateDealer(user);
                    return Json(new { success = true, ParticipantId = user.DealerId, IsDuplicate = "F" });
                }
                else
                {
                    return Json(new { success = true, ParticipantId = user.DealerId, IsDuplicate = "T" });
                }
            }

           
        }
        #endregion

        #region Delete
        [Route("delete/{uniquid}/{isedit}")]

        public JsonResult DeleteDealer(string uniquid, bool isedit)
        {
            bool IsInserted = _dealer.DeleteDealer(uniquid, isedit);
            return Json(new { success = true, ParticipantId = uniquid });
        }
        #endregion

        #region only Agent & Admin Can Access
        [Route("dealer-profile")]
        // [CustomAuthorize(Roles = "Admin")]
        public ActionResult DealerProfile()
        {
            var dealer0 = _dealer.GetDealerDetails(User.UserId);

            return View(new Repository.Models.ViewModel.ResetPassword() {
            ProfileImage = dealer0.ProfileImage});
        }

        [Route("dealer-profile")]
        [HttpPost]
        //[CustomAuthorize(Roles = "Admin")]
        public ActionResult DealerProfile(Repository.Models.ViewModel.ResetPassword Model)
        {
            if (ModelState.IsValid)
            {
                if (_dealer.ResetPassword(User.UserId, Model.OldPassword, Model.NewPassword))
                    ViewData["success"] = true;
                else
                    ViewData["success"] = false;
            }
            else
            {
                ViewData["success"] = false;
            }

            return View();
        }
        #endregion


        #region activate/deactivate
        //[CustomAuthorize(Roles = "Admin")]
        [Route("dealer-activation/{uniquid}")]
        public ActionResult SendActivationMail(string uniquid)
        {
            string message;
            var agentDetails = _dealer.GetDealerDetails(uniquid);

            if (agentDetails != null)
            {
                var actuallMail = agentDetails.DealerEmail;
#if DEBUG
                agentDetails.DealerEmail = "ankit.sarkar@indusnet.co.in,edwindantas@gmail.com,vikas@ntooitive.com";
#endif

                var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates");
                var razorTemplateFileLocation = Path.Combine(templateFolderPath, "send_invitation.cshtml");
                var razorText = System.IO.File.ReadAllText(razorTemplateFileLocation);

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(agentDetails.DealerId);
                var encoded = System.Convert.ToBase64String(plainTextBytes);

                var model = new SendInvitation { FuzzyLink = LinkCreator.CreateAgentInvitationLink(encoded) };
                var templateService = new TemplateService();
                var emailHtmlBody = templateService.Parse(razorText, model, null, null);


                var result = _mailBase.SendMail(new[] { agentDetails.DealerEmail }, "Next Step: Activate Your Account", emailHtmlBody);
                if (result)
                {
                    var dbResult = _dealer.InitiateRegistration(agentDetails.DealerId, actuallMail);
                    message = dbResult
                        ? "Activation mail send successfully. Please ask your agent to check inbox for more detail."
                        : "Unable to send the mail. Please try again later.";
                }
                else
                {
                    message = "Unable to send the mail. Please try again later.";
                }

            }
            else
            {
                message = "Unable to send the mail. Please try again later.";

            }


            return Json(new
            {
                Message = message
            },
                JsonRequestBehavior.AllowGet);
        }

        //[CustomAuthorize(Roles = "Admin")]
        [Route("dealer-deactivation/{uniquid}")]
        public ActionResult DeactivateAccount(string uniquid, bool isActivated)
        {
            var deactivateUser = _dealer.DeactiveDealer(uniquid, isActivated);

            var message = deactivateUser
                ? "Activation mail send successfully. Please ask your agent to check inbox for more detail."
                : "Unable to send the mail. Please try again later.";


            return Json(new
            {
                Message = message
            },
                JsonRequestBehavior.AllowGet);
        }

        [Route("activate/{hashCode}")]
        public ActionResult ActivateAgent(string hashCode)
        {
            var uniquid = UtilityClass.Base64Decode(hashCode);
            var agentDetails = _dealer.GetDealerDetails(uniquid);
            RegistartionViewModel rvm = new RegistartionViewModel();
            rvm.DealerName = agentDetails.DealerName;
            rvm.ParticipantId = agentDetails.DealerId;
            rvm.Email = agentDetails.DealerEmail;
            //var registartionViewModel = Mapper.Map<RegistartionViewModel>(agentDetails);
            return View(rvm);
        }
        #endregion

        [Route("registration")]
        public ActionResult Registration(RegistartionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activateUser = _dealer.SetPassword(model.ParticipantId, model.Email, HashPassword.Encrypt(model.Password));
                if (activateUser)
                {
#if DEBUG
                    model.Email = "kaijar.habib@indusnet.co.in";
#endif

                    var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates");
                    var razorTemplateFileLocation = Path.Combine(templateFolderPath, "agent_activation.cshtml");
                    var razorText = System.IO.File.ReadAllText(razorTemplateFileLocation);
                    var SendInvitation = new SendInvitation { FuzzyLink = ConfigurationManager.AppSettings["URL"] + "dealer/dealer-login" };
                    var templateService = new TemplateService();
                    var emailHtmlBody = templateService.Parse(razorText, SendInvitation, null, null);
                    _mailBase.SendMail(new[] { model.Email }, "Next Step: Login Your Account", emailHtmlBody);
                    return Redirect("/auto/auto-listing");

                }
                else
                {
                    return View("ActivateAgent", model);
                }
            }
            else
            {
                return View("ActivateAgent", model);
            }
        }

        [Route("script/{uniquid}")]
        [ValidateInput(false)]
        public ActionResult UpdateScript(string uniquid, string Script)
        {


            if (ModelState.IsValid)
            {
                var isUpdated = _dealer.UpdateChatScript(uniquid, Script);
                if (isUpdated)
                {

                    return Json(new
                    {
                        Message = ""
                    },
                JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        Message = ""
                    },
               JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Message = ""
                },
                JsonRequestBehavior.AllowGet);
            }
        }

        //[CustomAuthorize(Roles = "Admin")]
        [Route("dealer-featured/{uniquid}")]
        public ActionResult FeaturedAgent(string uniquid, bool isActivated)
        {
            var deactivateUser = _dealer.FeaturedDealer(uniquid, isActivated);

            var message = deactivateUser
                ? "Activation mail send successfully. Please ask your Dealer to check inbox for more detail."
                : "Unable to send the mail. Please try again later.";


            return Json(new
            {
                Message = message
            },
                JsonRequestBehavior.AllowGet);
        }

      


        #endregion

        #region Anonymous Access
        [Route("~/")]
        [Route("dealer-login")]
        public ActionResult Login()
        {
            if (User != null)
            {
                if (User.Roles.Contains("Admin"))
                {
                    return Redirect("/dealer/dealer-listing");
                }
                else if (User.Roles.Contains("Dealer"))
                {
                    return Redirect("/auto/auto-listing");
                }
                else
                {
                    return RedirectToAction("Index", "Dealer");
                }
            }
            return View("Index");
        }

        [HttpPost]
        [Route("dealer-login")]
        public ActionResult Login(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _dealer.Login(viewModel.UserEmail, viewModel.Password);

                if (user != null)
                {
                    var roles = user.Roles.Select(m => m).ToArray();

                    var serializeModel = new CustomPrincipalSerializeModel
                    {
                        UserId = user.DealerId.ToString(),
                        DealerName = user.DealerName,
                        DealerAddress = user.DealerAddress,
                        DealerCity = user.DealerCity,
                        Roles = roles,
                        ProfileImage = user.ProfileImage,
                        DealerEmail = user.DealerEmail
                    };

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                            user.DealerEmail,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(30),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    if (roles.Contains("Admin"))
                    {
                        return Redirect("/dealer/dealer-listing");
                    }
                    else if (roles.Contains("Dealer"))
                    {
                        return Redirect("/auto/auto-listing");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            return View("Index", viewModel);
        }


        [Route("dealer-signout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/dealer/dealer-login");
        }
        #endregion
    }
}