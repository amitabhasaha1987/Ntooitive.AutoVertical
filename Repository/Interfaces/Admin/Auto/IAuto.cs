using Repository.Interfaces.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;
//using Repository.Models.Admin.Dealer;
using Repository.Models.Admin.Auto;

namespace Repository.Interfaces.Admin.Auto
{
    public interface IAuto : IDataTable<Repository.Models.Admin.Auto.Auto, AutoDataTable>
    {
        //bool InsertBulkOffices(List<Repository.Models.ListHub.Office> users);
        Repository.Models.Admin.Auto.Auto GetAutoDetails(string autoId);
        bool UpdateAuto(Repository.Models.Admin.Auto.Auto objUser);

        bool DeleteAuto(string Vin, bool isDeleted);
        List<CarListing> GetAllFeaturedAutos(int count);
        List<CarListing> GetAllClassifiedCars(int count);
        //IEnumerable<State> GetStateList();
        //IEnumerable<Repository.Models.Admin.Office.Cities> GetCityList(string StateName);
        //IEnumerable<Repository.Models.Admin.Office.Cities> GetCityList();
        //IEnumerable<Repository.Models.Admin.Office.ZipCode> GetZipCodeList(string City);
        //IEnumerable<Repository.Models.Admin.Office.ZipCode> GetZipCodeList();
        //IEnumerable<Repository.Models.Admin.Office.StreetAddress> GetStreetAddressList(string City);
        //IEnumerable<Repository.Models.Admin.Office.StreetAddress> GetStreetAddressList();
        //bool UploadProfileImage(string uniquid, string url);
    }
}
