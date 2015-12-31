using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.Admin.Dealer
{
    public class User : Dealer
    {
        public bool IsActive { get; set; }
        public bool IsEmailSend { get; set; }
        public DateTime ActivateDate { get; set; }
        public DateTime DeactivateDate { get; set; }
        public DateTime InitiateDate { get; set; }
        public bool IsCertified { get; set; }
        public bool IsFeatured { get; set; }
        public string ProfileImage { get; set; }
        public string OfficeImage { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }

        public bool ShowMarketVal { get; set; }
        public bool ShowOwnershipCost { get; set; }
    }
}
