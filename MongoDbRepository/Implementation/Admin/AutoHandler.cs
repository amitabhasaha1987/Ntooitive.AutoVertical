//using MongoDbRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Repository.Models.Admin;
using Repository.Interfaces.Admin.Auto;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Repository.Models.DataTable;
using Repository.Models.Admin.Auto;
using Repository.Models;

namespace MongoDbRepository.Implementation.Admin
{

    public class AutoHandler : MongoDbRepository.Repository.Repository<Auto>, IAuto
    {
        private new const string CollectionName = "";

        public AutoHandler()
            : base(CollectionName)
        {
        }

        public AutoHandler(IMongoDatabase database)
            : base(database, CollectionName)
        {

        }
        public AutoHandler(string connectionString, string databaseName)
            : base(connectionString, databaseName, CollectionName)
        {
        }

        public List<Auto> GetDataSet(string userEmail, JQueryDataTableParamModel dataTableParamModel,
           AutoDataTable propertyDataTableSerachCriteria, out long filteredCount, string type = "")
        {
            base.CollectionName = "AutoVertical";
            var sortQuery = "";

            //           var matchQuery = "{$or : [{IsDeletedByPortal : {$exists : false} },{IsDeletedByPortal : false}]}";
            //matchQuery = "{$and: [{$or: [{IsDeletedByPortal: {$exists: false}}, {IsDeletedByPortal: false}]}," + matchQuery + endstr;

            //              var newhomelisting = $("#newhomelisting").dataTable({ bRetrieve: true });
            //              newhomelisting.fnDraw(true);

            //var m = "{$and: [{$or: [{ExtProperties.IsDeleted: {$exists: false}},{ExtProperties.IsDeleted: false}]},{Vin:{$ne : ''}}]}";
            //var matchQuery = "{$and:[{Vin:{$ne : ''}},{Vin:{$ne : ''}}}";
            var matchQuery1 = "{$and : [{$or : [{'ExtProperties.IsDeleted' : {$exists : false}},{'ExtProperties.IsDeleted' : false}]},{'Vin' : {$ne : ''}}]}";
            var matchQuery = "";


            if (propertyDataTableSerachCriteria.sortColumnIndex == 0 && propertyDataTableSerachCriteria.isVinSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{Vin : 1}" : "{Vin : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 1 && propertyDataTableSerachCriteria.isMakeSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{Make : 1}" : "{Make : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 2 && propertyDataTableSerachCriteria.isModelSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{Model : 1}" : "{Model : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 3 && propertyDataTableSerachCriteria.isPriceSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{Price : 1}" : "{Price : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 4 && propertyDataTableSerachCriteria.isConditionSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{Condition : 1}" : "{Condition : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 5 && propertyDataTableSerachCriteria.isMileageSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{Mileage : 1}" : "{Mileage : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 6 && propertyDataTableSerachCriteria.isStockNoSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{StockNo : 1}" : "{StockNo : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 7 && propertyDataTableSerachCriteria.isTransmissionSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{Transmission : 1}" : "{Transmission : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 8 && propertyDataTableSerachCriteria.isInteriorColorSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{InteriorColor : 1}" : "{InteriorColor : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 9 && propertyDataTableSerachCriteria.isExteriorColorSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{ExteriorColor : 1}" : "{ExteriorColor : -1}";
            }
            if (propertyDataTableSerachCriteria.sortColumnIndex == 10 && propertyDataTableSerachCriteria.isLocationSortable)
            {
                sortQuery = propertyDataTableSerachCriteria.sortDirection == "asc" ? "{Location : 1}" : "{Location : -1}";
            }

            if (!string.IsNullOrEmpty(dataTableParamModel.sSearch))
            {
                var startstr = "{$or: [";
                var endstr = "]}";
                var listOfmatchQuery = new List<string>();

                if (propertyDataTableSerachCriteria.isVinSearchable)
                {
                    listOfmatchQuery.Add("{'Vin': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isMakeSearchable)
                {
                    listOfmatchQuery.Add("{'Make': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isModelSearchable)
                {
                    listOfmatchQuery.Add("{'Model': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isPriceSearchable)
                {
                    listOfmatchQuery.Add("{'Price': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isConditionSearchable)
                {
                    listOfmatchQuery.Add("{'Condition': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isMileageSearchable)
                {
                    listOfmatchQuery.Add("{'Mileage': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isStockNoSearchable)
                {
                    listOfmatchQuery.Add("{'StockNo': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isTransmissionSearchable)
                {
                    listOfmatchQuery.Add("{'Transmission': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isInteriorColorSearchable)
                {
                    listOfmatchQuery.Add("{'InteriorColor': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isExteriorColorSearchable)
                {
                    listOfmatchQuery.Add("{'ExteriorColor': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                if (propertyDataTableSerachCriteria.isLocationSearchable)
                {
                    listOfmatchQuery.Add("{'Location': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                }
                listOfmatchQuery.Add("{'DealerName': {'$regex': '" + dataTableParamModel.sSearch + "', '$options': 'i' }}");
                matchQuery = startstr + string.Join(",", listOfmatchQuery) + endstr;
            }


            matchQuery = matchQuery.Replace(@"\", "");
            //var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
            var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sortQuery);


            if (matchQuery == "")
            {
                var matchdoc1 = BsonSerializer.Deserialize<BsonDocument>(matchQuery1);
                var AutoListings = GetCollection().Aggregate(new AggregateOptions()
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
                return AutoListings;
            }
            else
            {
                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var matchdoc1 = BsonSerializer.Deserialize<BsonDocument>(matchQuery1);
                var AutoListings = GetCollection().Aggregate(new AggregateOptions()
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
                return AutoListings;
            }



        }

        public long GetTotalCount(string userEmail, string type = "")
        {
            base.CollectionName = "AutoVertical";

            return GetCollection().CountAsync(new BsonDocument()).Result;
        }


        //    List<global::Repository.Models.Auto> global::Repository.Interfaces.DataTable.IDataTable<global::Repository.Models.Auto, AutoDataTable>.GetDataSet(string userEmail, JQueryDataTableParamModel dataTableParamModel, AutoDataTable serachCriteria, out long filteredCount, string type = "")
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public Auto GetAutoDetails(string autoId)
        {
            base.CollectionName = "AutoVertical";
            var tmpList = new Auto();
            using (var cursor = GetCollection()
                .FindAsync(m => m.Vin == autoId)
                .Result)
            {
                while (cursor.MoveNextAsync().Result)
                {
                    tmpList = cursor.Current.FirstOrDefault();
                }
                return tmpList;
            }
        }

        public bool UpdateAuto(Auto objUser)
        {
            base.CollectionName = "AutoVertical";
            var updateResult = GetCollection()
                                .UpdateOneAsync(m =>
                                    m.Vin == objUser.Vin,
                                     Builders<Auto>.Update
                                      .Set(m => m.Make, objUser.Make)
                    .Set(m => m.Model, objUser.Model)
                    .Set(m => m.Trim, objUser.Trim)
                    .Set(m => m.Year, objUser.Year)
                    .Set(m => m.Category, objUser.Category)
                    .Set(m => m.Mileage, objUser.Mileage)
                    .Set(m => m.Price, objUser.Price)
                    .Set(m => m.Condition, objUser.Condition)
                    .Set(m => m.InteriorColor, objUser.InteriorColor)
                     .Set(m => m.ExteriorColor, objUser.ExteriorColor)
                    .Set(m => m.Description, objUser.Description)
                //.Set(m => m.PhotosUrl, objUser.PhotosUrl)
                    .Set(m => m.StockNumber, objUser.StockNumber)
                    .Set(m => m.Transmission, objUser.Transmission)
                    .Set(m => m.DealerId, objUser.DealerId)
                    .Set(m => m.DealerName, objUser.DealerName)
                    .Set(m => m.DealerAddress, objUser.DealerAddress)
                    .Set(m => m.DealerCity, objUser.DealerCity)
                    .Set(m => m.DealerState, objUser.DealerState)
                    .Set(m => m.DealerZip, objUser.DealerZip)
                    .Set(m => m.VehicleSize, objUser.VehicleSize)
                     .Set(m => m.VehicleStyle, objUser.VehicleStyle)
                     .Set(m => m.IsUpdatedByPortal, objUser.IsUpdatedByPortal)
                //.Set(m => m.DealerEmail, objUser.DealerEmail)
                //.Set(m => m.ExtProperties, objUser.ExtProperties)
                    ).Result;
            return updateResult.ModifiedCount > 0;
        }

        public bool DeleteAuto(string Vin, bool isDeleted)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var filter = Builders<Auto>.Filter.Eq("Vin", Vin);
                var update = Builders<Auto>.Update
                    .Set("ExtProperties.IsDeleted", isDeleted).Set(m => m.IsUpdatedByPortal, true);

                var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                return results.ModifiedCount > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CarListing> GetAllFeaturedAutos(int count)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var projectQuery =
                   "{'Id' : '$_id','Make' : '$Make','Model':'$Model'," +
                   "'Year':'$Year','Mileage':'$Mileage','Price':'$Price'," +
                   "'Condition':'$Condition','InteriorColor':'$InteriorColor','VehicleStyle':'$VehicleStyle'," +
                   "'ExteriorColor':'$ExteriorColor','Description':'$Description'," +
                   "'PhotosUrl':'$PhotosUrl','StockNumber':'$StockNumber','Transmission':'$Transmission'," +
                   "'DealerName':'$DealerName','DealerCity':'$DealerCity','DealerPhone':'$DealerPhone','DealerAddress':'$DealerAddress','ExtProperties':'$ExtProperties'}";

                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);



                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                                      .Match(m => m.ExtProperties.IsFeatured == true && m.Price > 0.0)
                                      .Project(projectDoc)
                                      .Limit(count)
                                      .ToListAsync()
                                      .Result
                                      .Select(m => BsonSerializer.Deserialize<CarListing>(m));

                return aggregate.ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }


        public List<CarListing> GetAllClassifiedCars(int count)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var projectQuery =
                   "{'Id' : '$_id','Make' : '$Make','Model':'$Model'," +
                   "'Year':'$Year','Mileage':'$Mileage','Price':'$Price'," +
                   "'Condition':'$Condition','InteriorColor':'$InteriorColor','VehicleStyle':'$VehicleStyle'," +
                   "'ExteriorColor':'$ExteriorColor','Description':'$Description'," +
                   "'PhotosUrl':'$PhotosUrl','StockNumber':'$StockNumber','Transmission':'$Transmission'," +
                   "'DealerCity':'$DealerCity','DealerPhone':'$DealerPhone','DealerAddress':'$DealerAddress','ExtProperties':'$ExtProperties'}";

                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);



                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                   {
                       AllowDiskUse = true
                   })
                                      .Match(m => m.IsClassified == true && m.Price > 0.0)
                                      .Project(projectDoc)
                                      .Limit(count)
                                      .ToListAsync()
                                      .Result
                                      .Select(m => BsonSerializer.Deserialize<CarListing>(m));

                return aggregate.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
