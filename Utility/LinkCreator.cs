using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class LinkCreator
    {
        public static string CreateAgentInvitationLink(string uniqueid)
        {
            var currentDomain = ConfigurationManager.AppSettings["URL"];

            var hashUniqueId = UtilityClass.Base64Encode(uniqueid);
            return currentDomain + "dealer/activate/" + hashUniqueId;
        }
    }
}
