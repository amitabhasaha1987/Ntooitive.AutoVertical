using Repository.Interfaces.DataTable;
using Repository.Models.Admin.Dealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.Admin.Dealer
{
    public interface IDealer : IDataTable<User, DealerDataTable>
    {
        bool InsertBulkDealers(List<User> users);
        User Login(string email, string password);
        User GetDealerDetails(string participantId);
        bool InitiateRegistration(string participateId, string email);
        bool SetPassword(string participateId, string email, string hashPassword);
        bool DeactiveDealer(string uniquid, bool isActive);
        bool UploadProfileImage(string uniquid, string url);

        bool ResetPassword(string uniquid, string oldpwd, string newpwd);
        bool FeaturedDealer(string uniquid, bool isActivated);

        bool MarketVal(string uniquid, bool isActivated);
        bool OwnershipCost(string uniquid, bool isActivated);
        bool IsDealerFeatured(string emailId);
        User GetDealerDetailsByName(string participantId);
        List<User> GetDealerDetails();

        bool UpdateChatScript(string uniquid, string strChatScript);

        bool CertifiedDealer(string uniquid, bool isActivated);

        bool UpdateDealer(User objUser);

        bool DeleteDealer(string DealerId, bool isDeleted);

        List<User> GetAllFeaturedUsers(int count);
    }
}
