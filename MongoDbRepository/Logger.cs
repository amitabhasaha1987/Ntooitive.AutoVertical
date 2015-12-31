using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbRepository
{
    public class Logger : ILogger
    {
        public Task<IEnumerable<Repository.Models.Log>> GetLogHistories(System.Linq.Expressions.Expression<Func<Repository.Models.Log, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Repository.Models.Log> Create(Repository.Models.Log entity)
        {
            throw new NotImplementedException();
        }

        public Task<Repository.Models.Log> CreateSpecificId(Repository.Models.Log entity)
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

        public Task<IEnumerable<Repository.Models.Log>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Repository.Models.Log> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Repository.Models.Log>> InsertBulk(IEnumerable<Repository.Models.Log> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Repository.Models.Log entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatedOn(Repository.Models.Log account, DateTime date)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
