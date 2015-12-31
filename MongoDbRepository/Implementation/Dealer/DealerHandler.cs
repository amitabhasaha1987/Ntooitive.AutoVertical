using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbRepository.Repository;
using Repository.Interfaces.Admin.Dealer;
using Repository.Models.Admin.Dealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace MongoDbRepository.Implementation.Dealer
{
    public class DealerHandler : Repository<User>, IDealer
    {
        private new const string CollectionName = "";
        public DealerHandler()
            : base(CollectionName)
        {
        }
        public DealerHandler(IMongoDatabase database)
            : base(database, CollectionName)
        {

        }
        public DealerHandler(string connectionString, string databaseName)
            : base(connectionString, databaseName, CollectionName)
        {
        }
        public bool InsertBulkDealers(List<User> users)
        {
            base.CollectionName = "UserDetails";
            return base.InsertBulk(users);
        }

        public User Login(string email, string password)
        {
            base.CollectionName = "UserDetails";
            var tmpList = new User();
            using (var cursor = GetCollection()
                .FindAsync(m => m.DealerEmail == email && m.Password == HashPassword.Encrypt(password))
                .Result)
            {
                while (cursor.MoveNextAsync().Result)
                {
                    tmpList = cursor.Current.FirstOrDefault();
                }
                return tmpList;
            }
        }

        public User GetDealerDetails(string participantId)
        {
            base.CollectionName = "UserDetails";
            var tmpList = new User();
            using (var cursor = GetCollection()
                .FindAsync(m => m.DealerId == participantId)
                .Result)
            {
                while (cursor.MoveNextAsync().Result)
                {
                    tmpList = cursor.Current.FirstOrDefault();
                }
                return tmpList;
            }
        }

        public bool InitiateRegistration(string participateId, string email)
        {
            base.CollectionName = "UserDetails";

            var updateResult = GetCollection().UpdateOneAsync(
                m => m.DealerId == participateId && m.DealerEmail == email,
                Builders<User>.Update
                .Set(m => m.IsUpdatedByPortal, true)
                .Set(m => m.IsEmailSend, true)
                .Set(m => m.InitiateDate, DateTime.Now)).Result;

            return updateResult.ModifiedCount > 0;
        }

        public bool SetPassword(string participateId, string email, string hashPassword)
        {
            base.CollectionName = "UserDetails";

            var updateResult = GetCollection().UpdateOneAsync(
                m => m.DealerId == participateId && m.DealerEmail == email,
                Builders<User>.Update
                .Set(m => m.Password, hashPassword)
                .Set(m => m.IsActive, true).Set(m => m.IsUpdatedByPortal, true)
                .Set(m => m.ActivateDate, DateTime.Now)).Result;

            return updateResult.ModifiedCount > 0;
        }

        public bool DeactiveDealer(string uniquid, bool isActive)
        {
            base.CollectionName = "UserDetails";

            var updateResult = GetCollection().UpdateOneAsync(
                m => m.DealerId == uniquid,
                Builders<User>.Update
                    .Set(m => m.IsActive, isActive)
                    .Set(m => m.IsEmailSend, false)
                    .Set(m => m.IsUpdatedByPortal, true)
                    .Set(m => m.DeactivateDate, DateTime.Now)).Result;

            return updateResult.ModifiedCount > 0;
        }

        public bool UploadProfileImage(string uniquid, string url)
        {
            base.CollectionName = "UserDetails";

            var updateResult = GetCollection().UpdateOneAsync(
                m => m.DealerId == uniquid,
                Builders<User>.Update
                .Set(m => m.IsUpdatedByPortal, true)
                    .Set(m => m.ProfileImage, url)).Result;

            return updateResult.ModifiedCount > 0;
        }

        public bool ResetPassword(string uniquid, string oldpwd, string newpwd)
        {
            base.CollectionName = "UserDetails";
            var updateResult = GetCollection()
                                .UpdateOneAsync(m =>
                                    m.DealerId == uniquid &&
                                    m.Password == HashPassword.Encrypt(oldpwd),
                                     Builders<User>.Update
                                     .Set(m => m.IsUpdatedByPortal, true)
                    .Set(m => m.Password, HashPassword.Encrypt(newpwd))).Result;
            return updateResult.ModifiedCount > 0;
        }

        public bool FeaturedDealer(string uniquid, bool isActivated)
        {
            base.CollectionName = "UserDetails";

            var updateResult = GetCollection().UpdateOneAsync(
                m => m.DealerId == uniquid,
                Builders<User>.Update
                .Set(m => m.IsUpdatedByPortal, true)
                    .Set(m => m.IsFeatured, isActivated)).Result;

            return updateResult.ModifiedCount > 0;
        }

        public bool MarketVal(string uniquid, bool isActivated)
        {
            base.CollectionName = "UserDetails";

            var updateResult = GetCollection().UpdateOneAsync(
                m => m.DealerId == uniquid,
                Builders<User>.Update
                .Set(m => m.IsUpdatedByPortal, true)
                    .Set(m => m.ShowMarketVal, isActivated)).Result;

            return updateResult.ModifiedCount > 0;
        }

        public bool OwnershipCost(string uniquid, bool isActivated)
        {
            base.CollectionName = "UserDetails";

            var updateResult = GetCollection().UpdateOneAsync(
                m => m.DealerId == uniquid,
                Builders<User>.Update
                .Set(m => m.IsUpdatedByPortal, true)
                    .Set(m => m.ShowOwnershipCost, isActivated)).Result;

            return updateResult.ModifiedCount > 0;
        }

        public bool CertifiedDealer(string uniquid, bool isActivated)
        {
            base.CollectionName = "UserDetails";

            var updateResult = GetCollection().UpdateOneAsync(
                m => m.DealerId == uniquid,
                Builders<User>.Update
                .Set(m => m.IsUpdatedByPortal, true)
                    .Set(m => m.IsFeatured, isActivated)).Result;

            return updateResult.ModifiedCount > 0;
        }

        public bool IsDealerFeatured(string emailId)
        {
            base.CollectionName = "UserDetails";
            var aggregate = GetCollection().Find<User>(m => m.DealerEmail == emailId).FirstOrDefaultAsync().Result;
            if (aggregate == null)
            {
                return false;
            }
            else
            {
                return aggregate.IsFeatured;
            }
        }

        public List<User> GetDataSet(string userEmail, global::Repository.Models.DataTable.JQueryDataTableParamModel dataTableParamModel, DealerDataTable propertyDataTableSerachCriteria, out long filteredCount, string type = "")
        {
            base.CollectionName = "UserDetails";
            var sortQuery = "";
            //var matchQuery = "{'DealerId' : {$ne : null}}";
            var matchQuery1 = "{$and : [{$or : [{'ExtProperties.IsDeleted' : {$exists: false}},{'ExtProperties.IsDeleted' : false}]},{'DealerId' : {$ne : ''}},{'DealerId' : {$ne : null}}]}";
            var matchQuery = "";

            if (propertyDataTableSerachCriteria.sortColumnIndex == 0 && propertyDataTableSerachCriteria.isParticipantIdSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{_id : 1}" : "{_id : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 1 && propertyDataTableSerachCriteria.isFirstNameSearchable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{DealerName : 1}" : "{DealerName : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 2 && propertyDataTableSerachCriteria.isLastNameSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{DealerAddress : 1}" : "{DealerAddress : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 3 && propertyDataTableSerachCriteria.isEmailSearchable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{DealerEmail : 1}" : "{DealerEmail : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 4 && propertyDataTableSerachCriteria.isPrimaryContactPhoneSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{DealerZip : 1}" : "{DealerZip : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 5 && propertyDataTableSerachCriteria.isOfficePhoneSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{DealerPhone : 1}" : "{DealerPhone : -1}";
            }

            if (!string.IsNullOrEmpty(dataTableParamModel.sSearch))
            {
                var startstr = "{$or: [";
                var endstr = "]}";
                var listOfmatchQuery = new List<string>();

                if (propertyDataTableSerachCriteria.isParticipantIdSearchable)
                {
                    listOfmatchQuery.Add("{'_id': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isParticipantIdSearchable)
                {
                    listOfmatchQuery.Add("{'DealerId': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isFirstNameSearchable)
                {
                    listOfmatchQuery.Add("{'DealerName': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isLastNameSearchable)
                {
                    listOfmatchQuery.Add("{'DealerAddress': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isEmailSearchable)
                {
                    listOfmatchQuery.Add("{'DealerEmail': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isPrimaryContactPhoneSearchable)
                {
                    listOfmatchQuery.Add("{'DealerZip': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isOfficePhoneSearchable)
                {
                    listOfmatchQuery.Add("{'DealerPhone': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                //listOfmatchQuery.Add(matchQuery);
                matchQuery = startstr + string.Join(",", listOfmatchQuery) + endstr;
            }


            matchQuery = matchQuery.Replace(@"\", "");

            var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sortQuery);


            if (matchQuery == "")
            {
                var matchdoc1 = BsonSerializer.Deserialize<BsonDocument>(matchQuery1);
                var userListings = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                               .Match(matchdoc1)
                                .Sort(sortDoc)
                                .Skip(dataTableParamModel.iDisplayStart)
                                .Limit(dataTableParamModel.iDisplayLength)
                                .ToListAsync()
                                .Result.ToList();

                filteredCount = GetCollection().CountAsync(matchdoc1).Result;
                return userListings;
            }
            else
            {
                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var matchdoc1 = BsonSerializer.Deserialize<BsonDocument>(matchQuery1);
                var userListings = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
               .Match(matchdoc1).Match(matchDoc)
                .Sort(sortDoc)
                .Skip(dataTableParamModel.iDisplayStart)
                .Limit(dataTableParamModel.iDisplayLength)
                .ToListAsync()
                .Result.ToList();

                filteredCount = GetCollection().CountAsync(matchDoc).Result;
                return userListings;
            }

        }

        public long GetTotalCount(string userEmail, string type = "")
        {
            base.CollectionName = "UserDetails";

            return GetCollection().CountAsync(new BsonDocument()).Result;
        }

        public User GetDealerDetailsByName(string dealerName)
        {
            base.CollectionName = "UserDetails";
            var tmpList = new User();
            using (var cursor = GetCollection()
                .FindAsync(m => m.DealerName == dealerName)
                .Result)
            {
                while (cursor.MoveNextAsync().Result)
                {
                    tmpList = cursor.Current.FirstOrDefault();
                }
                return tmpList;
            }
        }
        public List<User> GetDealerDetails()
        {
            try
            {
                base.CollectionName = "UserDetails";// Convert.ToString(DbCollections.PurchaseListHubFeed);
                //var tmpList = new List<PropertyDetails>();

                // base.CollectionName = "AutoVertical";
                // var matchQuery = "{'_id': ObjectId('" + Id + "')}";
                var projectQuery =
                   "{'DealerId' : '$DealerId','DealerName' : '$DealerName'}";

                //var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })

                    .Project(projectDoc).ToListAsync().Result.Select(m => BsonSerializer.Deserialize<User>(m)).ToList<User>();

                // var myObj = BsonSerializer.Deserialize<Auto>(aggregate.FirstOrDefaultAsync().Result);
                return aggregate;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool UpdateChatScript(string uniquid, string strChatScript)
        {
            base.CollectionName = "UserDetails";
            var updateResult = GetCollection()
                                .UpdateOneAsync(m =>
                                    m.DealerId == uniquid,
                                     Builders<User>.Update
                                     .Set(m => m.IsUpdatedByPortal, true)
                    .Set(m => m.ChatScript, strChatScript)).Result;
            return updateResult.ModifiedCount > 0;
        }

        public bool UpdateDealer(User objUser)
        {
            base.CollectionName = "UserDetails";
            var updateResult = GetCollection()
                                .UpdateOneAsync(m =>
                                    m.DealerId == objUser.DealerId,
                                     Builders<User>.Update
                    .Set(m => m.DealerName, objUser.DealerName)
                    .Set(m => m.DealerAddress, objUser.DealerAddress)
                    .Set(m => m.DealerCity, objUser.DealerCity)
                    .Set(m => m.DealerState, objUser.DealerState)
                    .Set(m => m.DealerZip, objUser.DealerZip)
                    .Set(m => m.DealerPhone, objUser.DealerPhone)
                    .Set(m => m.DealerEmail, objUser.DealerEmail)
                    .Set(m => m.IsCertified, objUser.IsCertified)
                    .Set(m => m.IsFeatured, objUser.IsFeatured)
                    .Set(m => m.DealerBio, objUser.DealerBio)
                    .Set(m => m.IsUpdatedByPortal, true)
                    ).Result;
            return updateResult.ModifiedCount > 0;
        }

        public bool DeleteDealer(string DealerId, bool isDeleted)
        {
            try
            {
                base.CollectionName = "UserDetails";

                var filter = Builders<User>.Filter.Eq("DealerId", DealerId);
                var update = Builders<User>.Update
                    .Set(m => m.IsUpdatedByPortal, true)
                    .Set("ExtProperties.IsDeleted", isDeleted);

                var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                return results.ModifiedCount > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<User> GetAllFeaturedUsers(int count)
        {
            try
            {
                base.CollectionName = "UserDetails";

                var results = GetCollection().Find(m => m.IsFeatured == true, new FindOptions()
                {

                }).Limit(count).ToListAsync().Result;

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
