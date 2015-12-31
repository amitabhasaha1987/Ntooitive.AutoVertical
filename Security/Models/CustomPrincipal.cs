using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Security.Models
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            return Roles.Any(role.Contains);
        }

        public CustomPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public string UserId { get; set; }
        public string DealerName { get; set; }
        public string Email { get; set; }

        public string ParticipantId { get; set; }
        public string ProfileImage { get; set; }
        public string[] Roles { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public string UserId { get; set; }
        public string DealerName { get; set; }
        public string DealerAddress { get; set; }
        public string DealerCity { get; set; }
        public string DealerEmail { get; set; }
        public string[] Roles { get; set; }
        public string ProfileImage { get; set; }
        public string ParticipantId { get; set; }

    }
}
