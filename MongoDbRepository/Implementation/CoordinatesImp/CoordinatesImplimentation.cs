using MongoDB.Driver;
using MongoDbRepository.Repository;
using Repository.Interfaces;
using Repository.Models.Map;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbRepository.Implementation.CoordinatesImp
{
    public class CoordinatesImplimentation : Repository<Coordinates>, ICoordinates
    {
        private new const string CollectionName = "";

        public CoordinatesImplimentation()
            : base(CollectionName)
        {
        }

        public CoordinatesImplimentation(IMongoDatabase database)
            : base(database, CollectionName)
        {

        }
        public CoordinatesImplimentation(string connectionString, string databaseName)
            : base(connectionString, databaseName, CollectionName)
        {
        }
        public global::Repository.Models.Map.Coordinates GetCoordinates(string address)
        {
            try
            {
                System.IO.TextWriter sw = null;
                var logpath = ConfigurationManager.AppSettings["logpath"];
               // string logpath = "E:\\Auto\\AutoVerticals\\Ntooitive.AutoVertical\\DataImportConsole\\DumpData";
                sw = System.IO.File.AppendText(logpath + "\\" + "schedulelog.txt");
                sw.WriteLine("Address" + address);
                sw.Flush();
                sw.Close();
                sw = null;
                base.CollectionName = "Coordinates";
                return GetCollection().Find<Coordinates>(m => m.Address == address).FirstOrDefaultAsync().Result;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "GetCoordinates");
                throw;
            }
        }


        public bool InsertBulkCoordinates(List<Coordinates> coordinates)
        {
            try
            {
                base.CollectionName = "Coordinates";
                GetCollection().InsertManyAsync(coordinates).Wait();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
