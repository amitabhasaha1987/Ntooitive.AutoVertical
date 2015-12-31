using MongoDB.Driver;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbRepository
{
    public class Accounts : IAccounts
    {
        public Task<Repository.Models.Account> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Repository.Models.Account> Create(Repository.Models.Account entity)
        {
            IMongoDatabase imongoDatabase = Factory.CreateMongoDatabase();
            IMongoCollection<Repository.Models.Account> collection = imongoDatabase.GetCollection<Repository.Models.Account>("Account");
            collection.InsertOneAsync(entity).Wait();
            //return acc;
            throw new NotImplementedException();
        }

        public Task<Repository.Models.Account> CreateSpecificId(Repository.Models.Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Repository.Models.Account>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Repository.Models.Account> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Repository.Models.Account>> InsertBulk(IEnumerable<Repository.Models.Account> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Repository.Models.Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatedOn(Repository.Models.Account account, DateTime date)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public Task<bool> Login(string userId, string password)
        {
            throw new NotImplementedException();
        }
    }
}
