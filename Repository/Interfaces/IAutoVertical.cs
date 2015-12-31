using Repository.Models;
using Repository.Models.Admin.Auto;
using Repository.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAutoVertical
    {
        bool InsertBulkAutoVertical(List<Auto> entities);
        IEnumerable<Make> GetMakeRecords();
        IEnumerable<VehicleType> GetVehicleTypeRecords();
        IEnumerable<Model> GetModelRecords();
        IEnumerable<VehicleType> GetVehicleTypeRecords(string MakeName);
        IEnumerable<Model> GetModelRecords(string TypeName, string MakeName);
        IEnumerable<Model> GetModelRecords(string MakeName);
        IEnumerable<CarListing> GetVehicleList(AdvanceSearch advSearch, int skip, int limit);

        IEnumerable<CarListing> GetVehicleListForDealer(AdvanceSearch advSearch, int skip, int limit);
        Auto GetAutoDetails(string Id);
                int GetVehicleRecordCount(AdvanceSearch advSearch);
        int GetVehicleRecordCountForDealer(AdvanceSearch advSearch);
        List<CarColors> GetExteriorColor(string MakeName);
        List<CarColors> GetInteriorColor(string MakeName);
        IEnumerable<AutoCompleteDetails> GetAutoCompleteDetails(string searchKey);
        List<CarYears> GetCarYear();
        void ProcessFeed(List<Repository.Models.Admin.Auto.Auto> lstAuto);
        bool DeleteProperty(string Vin, bool isDeleted);
        List<Auto> GetAllAuto();

        bool SetExtraProperty(string type, ManageAutoViewModel autoViewModel);
        ManageAutoViewModel GetExtraProperty(string type, string mlsid);

        string GetListedType(string type, string vin);

        bool UpdateAutoSpotFeatured(string vin, ManageAutoViewModel autoViewModel);

        bool UpdateImageList(string type, Auto autoViewModel);
        bool PullImage(string imageurl,string vin);
        Auto GetAutoDetailsByVin(string Id);
        Auto GetAutoDetailsById(string Id);

        IEnumerable<UserSrch> GetUserRecords();
   

        //bool InsertBulkAutoVertical(List<Repository.Models.Admin.Auto.Auto> entities);

    }
}
