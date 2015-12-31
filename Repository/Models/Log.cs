using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class Log : BaseEntity
    {
        public string Log_Message { get; set; }

        public LogDataWrapper Log_Data { get; set; }
        public bool Log_IsPickedUp { get; set; }
    }
    public class LogDataWrapper
    {
        public string LogDataWrapper_AccountId { get; set; }
        public string LogDataWrapper_InsightId { get; set; }
        public string LogDataWrapper_Exception { get; set; }
    }
}
