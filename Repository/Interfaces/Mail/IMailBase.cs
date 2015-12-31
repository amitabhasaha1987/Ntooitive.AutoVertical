using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.Mail
{
    public interface IMailBase
    {
        bool SendMail(string[] toEmails, string subject, string body, string[] ccEmails = null);
    }
}
