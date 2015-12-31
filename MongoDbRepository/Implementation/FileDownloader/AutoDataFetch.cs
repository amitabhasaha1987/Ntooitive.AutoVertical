using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbRepository.Repository;
using Repository.Interfaces.Downloader;
using Repository.Models.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbRepository.Implementation.FileDownloader
{
    public class AutoDataFetch : Repository<SaveLink>, IFetcher
    {
        private new const string CollectionName = "";

        public AutoDataFetch()
            : base(CollectionName)
        {
        }

        public AutoDataFetch(IMongoDatabase database)
            : base(database, CollectionName)
        {
        }

        public AutoDataFetch(string connectionString, string databaseName)
            : base(connectionString, databaseName, CollectionName)
        {
        }
        public bool SetUrl(string fileName)
        {
            try
            {
                base.CollectionName = "ArchiveLink";

                var update = Builders<SaveLink>.Update
                    .Set("Url", fileName);

                var results = GetCollection().UpdateOneAsync(new BsonDocument(), update, new UpdateOptions()
                {
                    IsUpsert = true
                }).Result;

                return results.ModifiedCount > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetUrl()
        {
            try
            {
                base.CollectionName = "ArchiveLink";
                var aggregate =
                     GetCollection().Find(new BsonDocument()).FirstOrDefaultAsync().Result;
                return aggregate != null ? aggregate.Url : string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
