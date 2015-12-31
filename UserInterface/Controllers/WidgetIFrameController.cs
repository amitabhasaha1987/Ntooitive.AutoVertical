using Repository.Interfaces;
using Repository.Interfaces.Admin.Auto;
using Repository.Interfaces.Admin.Dealer;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Controllers
{
    public class WidgetIFrameController : Controller
    {
        //
        // GET: /WidgetIFrame/
        private readonly IDealer _dealer;
        private readonly IAuto _auto;
        readonly IAutoVertical _autoVerticalService;

        public WidgetIFrameController(IDealer dealer, IAuto auto, IAutoVertical autoVerticalService)
        {
            _dealer = dealer;
            _auto = auto;
            _autoVerticalService = autoVerticalService;

        }
        public ActionResult SearchHome()
        {
            IndexModelView indexModelView = new IndexModelView();
            indexModelView.MakeList = _autoVerticalService.GetMakeRecords().ToList();
            indexModelView.VehicleTypeList = _autoVerticalService.GetVehicleTypeRecords().ToList();
            indexModelView.ModelList = _autoVerticalService.GetModelRecords().ToList();
            indexModelView.UserList = _autoVerticalService.GetUserRecords().ToList();

            return View(indexModelView);
        }

        public ActionResult GetAllFeaturedAgent(int count, string type="v")
        {
            var featuredUser = _dealer.GetAllFeaturedUsers(count);
            /*if (type.ToUpper() == "H")
            {
                return View("~\\Views\\WidgetApi\\GetAllFeaturedAgentHorizon.cshtml", featuredUser);
            }
            else if (type.ToUpper() == "V")
            {
                return View(featuredUser);
            }*/
            return View(featuredUser);
        }

        public ActionResult GetAllFeaturedCars(int count, string type = "v")
        {
            var featuredUser = _auto.GetAllFeaturedAutos(count);
            /*if (type.ToUpper() == "H")
            {
                return View("~\\Views\\WidgetApi\\GetAllFeaturedCarsHorizon.cshtml", featuredUser);
            }
            else if (type.ToUpper() == "V")
            {
                return View(featuredUser);
            }*/
            return View(featuredUser);
        }
        
        public ActionResult GetAllClassifiedCars(int count, string type = "h")
        {
            var featuredUser = _auto.GetAllClassifiedCars(count);
            /*if (type.ToUpper() == "H")
            {
                return View(featuredUser);
            }
            else if (type.ToUpper() == "V")
            {
                return View(featuredUser);
            }*/
            return View(featuredUser);
        }
	}
}