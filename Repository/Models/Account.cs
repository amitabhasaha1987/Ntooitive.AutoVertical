using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Repository.Models
{
    public class Account : BaseEntity
    {
        public string Account_FirstName { get; set; }

        public string Account_MiddleName { get; set; }

        public string Account_LastName { get; set; }

        public string Account_PictureUrl { get; set; }

        public string Account_Country { get; set; }

        public string Account_City { get; set; }

        public string Account_Email { get; set; }

        public DateTime? Account_DateOfBirth { get; set; }

        public string Account_Gender { get; set; }
    }
}
