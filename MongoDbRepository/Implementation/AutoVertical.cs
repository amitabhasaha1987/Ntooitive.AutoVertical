using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using Repository.Interfaces;
using Repository.Models;
using MongoDbRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using System.Text.RegularExpressions;
using Repository.Models.ViewModel;
using MongoDbRepository.Implementation.APIFetch;
using Repository.Models.Admin.Auto;
using System.Configuration;

namespace MongoDbRepository.Implementation
{
    public class AddressWithLatLong
    {
        public string Address { get; set; }
        public Location Coordinates { get; set; }

    }
    public class AutoVertical : Repository<Auto>, IAutoVertical
    {
        private IFetchLatLong _fetchLatLong;
        private new const string CollectionName = "";
        public AutoVertical()
            : base(CollectionName)
        {
        }
        public AutoVertical(IMongoDatabase database, IFetchLatLong fetchLatLong)
            : base(database, CollectionName)
        {
            _fetchLatLong = fetchLatLong;
        }
        public AutoVertical(string connectionString, string databaseName)
            : base(connectionString, databaseName, CollectionName)
        {
        }
        public bool InsertBulkAutoVertical(List<Auto> entities)
        {
            var count = 0;
            base.CollectionName = "AutoVertical";
            var addresslist = new HashSet<AddressWithLatLong>();
            var zipcodes = new HashSet<AddressWithLatLong>();
            foreach (var entity in entities)
            {
                count = count + 1;

                if (!string.IsNullOrEmpty(entity.DealerAddress))
                {
                    var address = addresslist.FirstOrDefault(m => m.Address == entity.DealerAddress);
                    if (address != null)
                    {
                        entity.GeoLocation = new GeoPoint(address.Coordinates.lat, address.Coordinates.lng);

                    }
                    else
                    {

                        var fetchedLatlong = _fetchLatLong.GetLatitudeAndLongitude(entity.DealerAddress);

                        if (fetchedLatlong != null)
                        {
                            addresslist.Add(new AddressWithLatLong
                            {
                                Address = entity.DealerAddress,
                                Coordinates = fetchedLatlong
                            });
                            entity.GeoLocation = new GeoPoint(fetchedLatlong.lat, fetchedLatlong.lng);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(entity.DealerZip))
                {
                    var zipCode = zipcodes.FirstOrDefault(m => m.Address == entity.DealerZip);
                    if (zipCode != null)
                    {
                        entity.GeoLocation = new GeoPoint(zipCode.Coordinates.lat, zipCode.Coordinates.lng);

                    }
                    else
                    {
                        var fetchedLatlong = _fetchLatLong.GetLatitudeAndLongitude(entity.DealerZip);

                        if (fetchedLatlong != null)
                        {
                            zipcodes.Add(new AddressWithLatLong
                            {
                                Address = entity.DealerAddress,
                                Coordinates = fetchedLatlong
                            });
                            entity.GeoLocation = new GeoPoint(fetchedLatlong.lat, fetchedLatlong.lng);
                        }
                    }
                }
            }
            return InsertBulk(entities);
        }



        public IEnumerable<Make> GetMakeRecords()
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var match = "{'Make' : {$ne : ''}}";

                var group = "{_id : '$Make',Count : {$sum :1}}";
                var projectQuery = "{_id : 0,MakersName : '$_id',Count : '$Count'}";
                var sort = "{MakersName : 1}";
                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(match);

                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sort);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Make>(m));

                return aggregate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<UserSrch> GetUserRecords()
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var match = "{'DealerName' : {$ne : ''}}";

                var group = "{_id : '$DealerName',Count : {$sum :1}}";
                var projectQuery = "{_id : 0,DealerName : '$_id',Count : '$Count'}";
                var sort = "{DealerName : 1}";
                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(match);

                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sort);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<UserSrch>(m));

                return aggregate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<VehicleType> GetVehicleTypeRecords()
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var match = "{'VehicleStyle' : {$ne : ''}}";

                var group = "{_id : 'VehicleStyle',Count : {$sum :1}}";
                var projectQuery = "{_id : 0,VehicleStyle : '$_id',VehicleCount : '$Count'}";
                var sort = "{VehicleStyle : 1}";

                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sort);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);
                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(match);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<VehicleType>(m));

                return aggregate.Where(x => x.VehicleTypeName != null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Model> GetModelRecords()
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var match = "{'Model' : {$ne : ''}}";

                var group = "{_id : '$Model',Count : {$sum :1}}";
                var projectQuery = "{_id : 0,ModelsName : '$_id',Count : '$Count'}";
                var sort = "{ModelsName : 1}";

                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sort);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);
                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(match);


                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
            .Match(projectDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Model>(m));

                return aggregate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<VehicleType> GetVehicleTypeRecords(string MakeName)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var matchQuery = "{'Make': '" + MakeName + "'}";
                var group = "{_id : '$VehicleStyle',Count : {$sum :1}}";
                var projectQuery = "{_id : 0,VehicleTypeName : '$_id',VehicleCount : '$Count'}";
                var sort = "{VehicleStyle : 1}";

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sort);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<VehicleType>(m));

                return aggregate.Where(x => x.VehicleTypeName != null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Model> GetModelRecords(string TypeName, string MakeName)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                string matchQuery = null;
                if (!string.IsNullOrEmpty(TypeName) && !string.IsNullOrEmpty(MakeName))
                {
                    matchQuery = "{'Make': {" + Utility.UtilityClass.GetString(MakeName) + "},'VehicleStyle':'" + TypeName + "'}";
                }
                else if (string.IsNullOrEmpty(TypeName) && !string.IsNullOrEmpty(MakeName))
                {
                    matchQuery = "{'Make': {" + Utility.UtilityClass.GetString(MakeName) + "}}";
                }
                else if (!string.IsNullOrEmpty(TypeName) && string.IsNullOrEmpty(MakeName))
                {
                    matchQuery = "{'VehicleStyle':'" + TypeName + "'}";
                }

                var group = "{_id : '$Model',Count : {$sum :1}}";
                var projectQuery = "{_id : 0,ModelsName : '$_id',Count : '$Count'}";
                var sort = "{ModelsName : 1}";

                var matchDoc = string.IsNullOrEmpty(matchQuery) ? null : BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sort);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);
                IEnumerable<Model> mod = null;
                if (matchDoc == null)
                {
                    mod = GetCollection().Aggregate(new AggregateOptions()
                    {
                        AllowDiskUse = true
                    })
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Model>(m));
                }
                else
                {
                    mod = GetCollection().Aggregate(new AggregateOptions()
                    {
                        AllowDiskUse = true
                    })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Model>(m));
                }
                //var aggregate = GetCollection().Aggregate(new AggregateOptions()
                //{
                //    AllowDiskUse = true
                //})
                //.Match(matchDoc)
                //.Group(groupDoc)
                //.Project(projectDoc)
                //.Sort(sortDoc)
                //.ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Model>(m));

                return mod.Where(x => x.ModelsName != null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Model> GetModelRecords(string MakeName)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                string matchQuery = null;

                if (!string.IsNullOrEmpty(MakeName))
                {
                    matchQuery = "{'Make': {" + Utility.UtilityClass.GetString(MakeName) + "}}";
                }


                var group = "{_id : '$Model',Count : {$sum :1}}";
                var projectQuery = "{_id : 0,ModelsName : '$_id',Count : '$Count'}";
                var sort = "{ModelsName : 1}";

                var matchDoc = string.IsNullOrEmpty(matchQuery) ? null : BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sort);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);
                IEnumerable<Model> mod = null;
                if (matchDoc == null)
                {
                    mod = GetCollection().Aggregate(new AggregateOptions()
                    {
                        AllowDiskUse = true
                    })
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Model>(m));
                }
                else
                {
                    mod = GetCollection().Aggregate(new AggregateOptions()
                    {
                        AllowDiskUse = true
                    })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Model>(m));
                }
                //var aggregate = GetCollection().Aggregate(new AggregateOptions()
                //{
                //    AllowDiskUse = true
                //})
                //.Match(matchDoc)
                //.Group(groupDoc)
                //.Project(projectDoc)
                //.Sort(sortDoc)
                //.ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Model>(m));

                return mod.Where(x => x.ModelsName != null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CarListing> GetVehicleList(AdvanceSearch advSearch, int skip, int limit)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                IEnumerable<CarListing> aggregate = new List<CarListing>();
                var matchQuery = Utility.UtilityClass.CreateQueryString(advSearch.MinPrice, advSearch.MaxPrice, advSearch.Mileage, advSearch.FromYear, advSearch.ToYear,
                                                                        advSearch.UsedNew, advSearch.Makes, advSearch.ModelList, advSearch.VehicleType, advSearch.ExSelectedColor,
                                                                        advSearch.InSelectedColor, advSearch.Make, advSearch.Model);

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                var projectQuery =
                   "{'Id' : '$_id','Make' : '$Make','Model':'$Model'," +
                   "'Year':'$Year','Mileage':'$Mileage','Price':'$Price'," +
                   "'Condition':'$Condition','InteriorColor':'$InteriorColor','VehicleStyle':'$VehicleStyle'," +
                   "'ExteriorColor':'$ExteriorColor','Description':'$Description'," +
                   "'PhotosUrl':'$PhotosUrl','StockNumber':'$StockNumber','Transmission':'$Transmission','DealerAddress':'$DealerAddress','ExtProperties':'$ExtProperties'}";

                //var sortBy ="{'ExtProperties.IsFeatured':-1,'ExtProperties.IsSpotlight':-1,'Price':1}";
                var sortBy = advSearch.SortingOrder == "0" ? "{Price : 1}" : "{" + advSearch.SortingOrder.Split('_')[0] + ":" + (advSearch.SortingOrder.Split('_')[1] == "Asc" ? 1 : -1) + "}";
                var sortBy3 = "{'ExtProperties.IsFeatured':-1}";
                var sortBy2 = "{'ExtProperties.IsSpotlight':-1}";
                var sortBy1 = "{'ExtProperties' : -1 }";



                //var sortBy = advSearch.SortingOrder == "0" ? "{Price : 1}" : "{" + advSearch.SortingOrder.Split('_')[0] + ":" + (advSearch.SortingOrder.Split('_')[1] == "Asc" ? 1 : -1) + "}";
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);
                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sortBy);
                var sortDoc1 = BsonSerializer.Deserialize<BsonDocument>(sortBy1);
                var sortDoc2 = BsonSerializer.Deserialize<BsonDocument>(sortBy2);
                var sortDoc3 = BsonSerializer.Deserialize<BsonDocument>(sortBy3);

                if ((!string.IsNullOrEmpty(advSearch.Location) && advSearch.Location != "0") || !string.IsNullOrEmpty(advSearch.ZipCode))
                {

                    if (advSearch.ZipCode != "undefined")
                    {

                        var geolocation = _fetchLatLong.GetLatitudeAndLongitude(advSearch.ZipCode);
                        if (geolocation != null)
                        {
                            var geoPoint = new BsonDocument
                            {
                                {"type", "Point"},
                                {"coordinates", new BsonArray(new double[] {geolocation.lng, geolocation.lat})}
                            };

                            if (Convert.ToDouble(advSearch.Location) > 0)
                            {

                                var maxDistanceInMiles = Convert.ToDouble(advSearch.Location) * 1609.34;

                                var geoNearOptions = new BsonDocument
                                {
                                    {"near", geoPoint},
                                    {"distanceField", "CalculatedDistance"},
                                    {"maxDistance", Convert.ToDouble(maxDistanceInMiles)},
                                    {"distanceMultiplier", Convert.ToDouble("0.000621371")},
                                    {"query", matchDoc},
                                    {"limit", 3000},
                                    {"spherical", true},
                                };

                                var stage =
                                    new BsonDocumentPipelineStageDefinition<Auto, CarListing>(
                                        new BsonDocument { { "$geoNear", geoNearOptions } });
                                aggregate = GetCollection().Aggregate(new AggregateOptions()
                                {
                                    AllowDiskUse = true
                                })
                                    .AppendStage(stage)
                                    .Project(projectDoc)
                                    .Sort(sortDoc)
                                    .Sort(sortDoc1)
                                    .Sort(sortDoc2)
                                    .Sort(sortDoc3)
                                    .Skip(skip)
                                    .Limit(limit)
                                    .ToListAsync()
                                    .Result
                                    .Select(m => BsonSerializer.Deserialize<CarListing>(m));

                            }
                            else
                            {
                                matchQuery = !string.IsNullOrEmpty(matchQuery) ? string.IsNullOrEmpty(advSearch.ZipCode)
                                 ? matchQuery
                                 : matchQuery + ",'DealerZip':'" + advSearch.ZipCode + "'" : "'DealerZip':'" + advSearch.ZipCode + "'";
                                matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                                aggregate = GetCollection().Aggregate(new AggregateOptions()
                                {
                                    AllowDiskUse = true
                                })
                                    .Match(matchDoc)
                                    .Project(projectDoc)
                                    .Sort(sortDoc)
                                    .Sort(sortDoc1)
                                    .Sort(sortDoc2)
                                    .Sort(sortDoc3)
                                    .Skip(skip)
                                    .Limit(limit)
                                    .ToListAsync()
                                    .Result
                                    .Select(m => BsonSerializer.Deserialize<CarListing>(m));
                            }
                        }
                        else
                        {
                            matchQuery = !string.IsNullOrEmpty(matchQuery) ? string.IsNullOrEmpty(advSearch.ZipCode)
                                ? matchQuery
                                : matchQuery + ",'DealerZip':'" + advSearch.ZipCode + "'" : "'DealerZip':'" + advSearch.ZipCode + "'";
                            matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                            aggregate = GetCollection().Aggregate(new AggregateOptions()
                            {
                                AllowDiskUse = true
                            })
                            .Match(matchDoc)
                            .Project(projectDoc)
                            .Sort(sortDoc)
                            .Sort(sortDoc1)
                            .Sort(sortDoc2)
                            .Sort(sortDoc3)
                            .Skip(skip)
                            .Limit(limit)
                            .ToListAsync()
                            .Result
                            .Select(m => BsonSerializer.Deserialize<CarListing>(m));
                        }
                    }
                    else
                    {
                        aggregate = GetCollection().Aggregate(new AggregateOptions()
                        {
                            AllowDiskUse = true
                        })
                            .Match(matchDoc)
                            .Project(projectDoc)
                            .Sort(sortDoc)
                            .Sort(sortDoc1)
                            .Sort(sortDoc2)
                            .Sort(sortDoc3)
                            .Skip(skip).Limit(limit)
                            .ToListAsync()
                            .Result
                            .Select(m => BsonSerializer.Deserialize<CarListing>(m));
                    }
                }
                else
                {
                    aggregate = GetCollection().Aggregate(new AggregateOptions()
                    {
                        AllowDiskUse = true
                    })
                        .Match(matchDoc)
                        .Project(projectDoc)
                        .Sort(sortDoc)
                        .Sort(sortDoc1)
                        .Sort(sortDoc2)
                        .Sort(sortDoc3)
                        .Skip(skip).Limit(limit)
                        .ToListAsync()
                        .Result
                        .Select(m => BsonSerializer.Deserialize<CarListing>(m));
                }

                //var x = aggregate.OrderBy(m => m.ExtProperties != null ?m.Price: m.ExtProperties.IsSpotlight);
                //var y = x.OrderBy(m => m.ExtProperties.IsFeatured);

                return aggregate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CarListing> GetVehicleListForDealer(AdvanceSearch advSearch, int skip, int limit)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                IEnumerable<CarListing> aggregate = new List<CarListing>();
                var matchQuery = Utility.UtilityClass.CreateQueryStringForDealer(advSearch.MinPrice, advSearch.MaxPrice, advSearch.Mileage, advSearch.FromYear, advSearch.ToYear,
                                                                        advSearch.UsedNew, advSearch.Dealers, advSearch.Makes, advSearch.ModelList, advSearch.VehicleType, advSearch.ExSelectedColor,
                                                                        advSearch.InSelectedColor, advSearch.DealerSrch, advSearch.Make, advSearch.Model);

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                var projectQuery =
                   "{'Id' : '$_id','Make' : '$Make','Model':'$Model'," +
                   "'Year':'$Year','Mileage':'$Mileage','Price':'$Price'," +
                   "'Condition':'$Condition','InteriorColor':'$InteriorColor','VehicleStyle':'$VehicleStyle'," +
                   "'ExteriorColor':'$ExteriorColor','Description':'$Description'," +
                   "'PhotosUrl':'$PhotosUrl','StockNumber':'$StockNumber','Transmission':'$Transmission','DealerAddress':'$DealerAddress','ExtProperties':'$ExtProperties'}";

                var sortBy = advSearch.SortingOrder == "0" ? "{Price : 1}" : "{" + advSearch.SortingOrder.Split('_')[0] + ":" + (advSearch.SortingOrder.Split('_')[1] == "Asc" ? 1 : -1) + "}";
                var sortBy3 = "{'ExtProperties.IsFeatured':-1}";
                var sortBy2 = "{'ExtProperties.IsSpotlight':-1}";
                var sortBy1 = "{'ExtProperties' : -1 }";

                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sortBy);
                var sortDoc1 = BsonSerializer.Deserialize<BsonDocument>(sortBy1);
                var sortDoc2 = BsonSerializer.Deserialize<BsonDocument>(sortBy2);
                var sortDoc3 = BsonSerializer.Deserialize<BsonDocument>(sortBy3);

                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                if ((!string.IsNullOrEmpty(advSearch.Location) && advSearch.Location != "0") || !string.IsNullOrEmpty(advSearch.ZipCode))
                {

                    if (advSearch.ZipCode != "undefined")
                    {

                        var geolocation = _fetchLatLong.GetLatitudeAndLongitude(advSearch.ZipCode);
                        if (geolocation != null)
                        {
                            var geoPoint = new BsonDocument
                            {
                                {"type", "Point"},
                                {"coordinates", new BsonArray(new double[] {geolocation.lng, geolocation.lat})}
                            };

                            if (Convert.ToDouble(advSearch.Location) > 0)
                            {

                                var maxDistanceInMiles = Convert.ToDouble(advSearch.Location) * 1609.34;

                                var geoNearOptions = new BsonDocument
                                {
                                    {"near", geoPoint},
                                    {"distanceField", "CalculatedDistance"},
                                    {"maxDistance", Convert.ToDouble(maxDistanceInMiles)},
                                    {"distanceMultiplier", Convert.ToDouble("0.000621371")},
                                    {"query", matchDoc},
                                    {"limit", 3000},
                                    {"spherical", true},
                                };

                                var stage =
                                    new BsonDocumentPipelineStageDefinition<Auto, CarListing>(
                                        new BsonDocument { { "$geoNear", geoNearOptions } });
                                aggregate = GetCollection().Aggregate(new AggregateOptions()
                                {
                                    AllowDiskUse = true
                                })
                                    .AppendStage(stage)
                                    .Project(projectDoc)
                                    .Sort(sortDoc)
                                    .Sort(sortDoc1)
                                    .Sort(sortDoc2)
                                    .Sort(sortDoc3)
                                    .Skip(skip)
                                    .Limit(limit)
                                    .ToListAsync()
                                    .Result
                                    .Select(m => BsonSerializer.Deserialize<CarListing>(m));

                            }
                            else
                            {
                                matchQuery = !string.IsNullOrEmpty(matchQuery) ? string.IsNullOrEmpty(advSearch.ZipCode)
                                 ? matchQuery
                                 : matchQuery + ",'DealerZip':'" + advSearch.ZipCode + "'" : "'DealerZip':'" + advSearch.ZipCode + "'";
                                matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                                aggregate = GetCollection().Aggregate(new AggregateOptions()
                                {
                                    AllowDiskUse = true
                                })
                                    .Match(matchDoc)
                                    .Project(projectDoc)
                                    .Sort(sortDoc)
                                    .Sort(sortDoc1)
                                    .Sort(sortDoc2)
                                    .Sort(sortDoc3)
                                    .Skip(skip)
                                    .Limit(limit)
                                    .ToListAsync()
                                    .Result
                                    .Select(m => BsonSerializer.Deserialize<CarListing>(m));
                            }
                        }
                        else
                        {
                            matchQuery = !string.IsNullOrEmpty(matchQuery) ? string.IsNullOrEmpty(advSearch.ZipCode)
                                ? matchQuery
                                : matchQuery + ",'DealerZip':'" + advSearch.ZipCode + "'" : "'DealerZip':'" + advSearch.ZipCode + "'";
                            matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                            aggregate = GetCollection().Aggregate(new AggregateOptions()
                            {
                                AllowDiskUse = true
                            })
                            .Match(matchDoc)
                            .Project(projectDoc)
                            .Sort(sortDoc)
                            .Sort(sortDoc1)
                            .Sort(sortDoc2)
                            .Sort(sortDoc3)
                            .Skip(skip)
                            .Limit(limit)
                            .ToListAsync()
                            .Result
                            .Select(m => BsonSerializer.Deserialize<CarListing>(m));
                        }
                    }
                    else
                    {
                        aggregate = GetCollection().Aggregate(new AggregateOptions()
                        {
                            AllowDiskUse = true
                        })
                            .Match(matchDoc)
                            .Project(projectDoc)
                            .Sort(sortDoc)
                            .Sort(sortDoc1)
                            .Sort(sortDoc2)
                            .Sort(sortDoc3)
                            .Skip(skip).Limit(limit)
                            .ToListAsync()
                            .Result
                            .Select(m => BsonSerializer.Deserialize<CarListing>(m));
                    }
                }
                else
                {
                    aggregate = GetCollection().Aggregate(new AggregateOptions()
                    {
                        AllowDiskUse = true
                    })
                        .Match(matchDoc)
                        .Project(projectDoc)
                        .Sort(sortDoc)
                        .Sort(sortDoc1)
                        .Sort(sortDoc2)
                        .Sort(sortDoc3)
                        .Skip(skip).Limit(limit)
                        .ToListAsync()
                        .Result
                        .Select(m => BsonSerializer.Deserialize<CarListing>(m));
                }

                return aggregate;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Auto GetAutoDetails(string Id)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var matchQuery = "{'_id': ObjectId('" + Id + "')}";
                var projectQuery =
                   "{'DealerId' : '$DealerId','DealerName' : '$DealerName','Make' : '$Make','Model':'$Model','Trim':'$Trim','Year':'$Year','Vin':'$Vin','Category':'$Category'," +
                   "'Mileage':'$Mileage','Price':'$Price','Condition':'$Condition','InteriorColor':'$InteriorColor','ExteriorColor':'$ExteriorColor'," +
                   "'Description':'$Description','DealershipPhone':'$DealershipPhone','DealershipCity':'$DealershipCity','DealershipState':'$DealershipState'," +
                   "'DealershipZip':'$DealershipZip','PhotosUrl':'$PhotosUrl','StockNumber':'$StockNumber','Transmission':'$Transmission','DealerAddress':'$DealerAddress'," +
                   "'DealerCity':'$DealerCity','DealerState':'$DealerState','DealerZip':'$DealerZip','DealerPhone':'$DealerPhone','DealerEmail':'$DealerEmail'," +
                   "'LeadEmailAddress':'$LeadEmailAddress','CCEmailAddress':'$CCEmailAddress','LeadType':'$LeadType','GeoLocation':'$GeoLocation','VehicleStyle':'$VehicleStyle','VehicleSize':'$VehicleSize'}";

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                    .Match(matchDoc)
                    .Project(projectDoc);//.ToListAsync().Result.Select(m => BsonSerializer.Deserialize<CarListing>(m));

                var myObj = BsonSerializer.Deserialize<Auto>(aggregate.FirstOrDefaultAsync().Result);
                return myObj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetVehicleRecordCount(AdvanceSearch advSearch)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var matchQuery = Utility.UtilityClass.CreateQueryString(advSearch.MinPrice, advSearch.MaxPrice, advSearch.Mileage, advSearch.FromYear, advSearch.ToYear,
                                                                        advSearch.UsedNew, advSearch.Makes, advSearch.ModelList, advSearch.VehicleType, advSearch.ExSelectedColor,
                                                                        advSearch.InSelectedColor, advSearch.Make, advSearch.Model);
                int aggregate = 0;
                var matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                if ((!string.IsNullOrEmpty(advSearch.Location) && advSearch.Location != "0") || !string.IsNullOrEmpty(advSearch.ZipCode))
                {

                    if (advSearch.ZipCode != "undefined")
                    {

                        var geolocation = _fetchLatLong.GetLatitudeAndLongitude(advSearch.ZipCode);
                        if (geolocation != null)
                        {
                            var geoPoint = new BsonDocument
                            {
                                {"type", "Point"},
                                {"coordinates", new BsonArray(new double[] {geolocation.lng, geolocation.lat})}
                            };

                            if (Convert.ToDouble(advSearch.Location) > 0)
                            {
                                var maxDistanceInMiles = Convert.ToDouble(advSearch.Location) * 1609.34;

                                var geoNearOptions = new BsonDocument
                                {
                                    {"near", geoPoint},
                                    {"distanceField", "CalculatedDistance"},
                                    {"maxDistance", Convert.ToDouble(maxDistanceInMiles)},
                                    {"distanceMultiplier", Convert.ToDouble("0.000621371")},
                                    {"query", matchDoc},
                                    {"limit", 3000},
                                    {"spherical", true},
                                };

                                var stage =
                                    new BsonDocumentPipelineStageDefinition<Auto, BsonDocument>(
                                        new BsonDocument { { "$geoNear", geoNearOptions } });
                                aggregate = GetCollection().Aggregate(new AggregateOptions()
                                {
                                    AllowDiskUse = true
                                })
                                    .AppendStage(stage)
                                    .ToListAsync()
                                    .Result.Count();
                            }
                            else
                            {
                                matchQuery = !string.IsNullOrEmpty(matchQuery) ? string.IsNullOrEmpty(advSearch.ZipCode)
                                 ? matchQuery
                                 : matchQuery + ",'DealerZip':'" + advSearch.ZipCode + "'" : "'DealerZip':'" + advSearch.ZipCode + "'";

                                matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                                aggregate = GetCollection().Aggregate(new AggregateOptions()
                                {
                                    AllowDiskUse = true
                                }).Match(matchDoc)
                                    .ToListAsync()
                                    .Result.Count();
                            }
                        }
                        else
                        {
                            matchQuery = !string.IsNullOrEmpty(matchQuery) ? string.IsNullOrEmpty(advSearch.ZipCode)
                               ? matchQuery
                               : matchQuery + ",'DealerZip':'" + advSearch.ZipCode + "'" : "'DealerZip':'" + advSearch.ZipCode + "'";

                            matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                            aggregate = GetCollection().Aggregate(new AggregateOptions()
                            {
                                AllowDiskUse = true
                            }).Match(matchDoc)
                                .ToListAsync()
                                .Result.Count();
                        }
                    }
                    else
                    {
                        aggregate = (int)GetCollection().CountAsync(matchDoc).Result;
                    }
                }
                else
                {
                    aggregate = GetCollection().Aggregate(new AggregateOptions()
                    {
                        AllowDiskUse = true
                    })
                       .Match(matchDoc).ToListAsync().Result.Count();
                }

                return aggregate;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetVehicleRecordCountForDealer(AdvanceSearch advSearch)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var matchQuery = Utility.UtilityClass.CreateQueryStringForDealer(advSearch.MinPrice, advSearch.MaxPrice, advSearch.Mileage, advSearch.FromYear, advSearch.ToYear,
                                                                        advSearch.UsedNew, advSearch.Dealers, advSearch.Makes, advSearch.ModelList, advSearch.VehicleType, advSearch.ExSelectedColor,
                                                                        advSearch.InSelectedColor, advSearch.Make, advSearch.Make, advSearch.Model);
                int aggregate = 0;
                var matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                if ((!string.IsNullOrEmpty(advSearch.Location) && advSearch.Location != "0") || !string.IsNullOrEmpty(advSearch.ZipCode))
                {

                    if (advSearch.ZipCode != "undefined")
                    {

                        var geolocation = _fetchLatLong.GetLatitudeAndLongitude(advSearch.ZipCode);
                        if (geolocation != null)
                        {
                            var geoPoint = new BsonDocument
                            {
                                {"type", "Point"},
                                {"coordinates", new BsonArray(new double[] {geolocation.lng, geolocation.lat})}
                            };

                            if (Convert.ToDouble(advSearch.Location) > 0)
                            {
                                var maxDistanceInMiles = Convert.ToDouble(advSearch.Location) * 1609.34;

                                var geoNearOptions = new BsonDocument
                                {
                                    {"near", geoPoint},
                                    {"distanceField", "CalculatedDistance"},
                                    {"maxDistance", Convert.ToDouble(maxDistanceInMiles)},
                                    {"distanceMultiplier", Convert.ToDouble("0.000621371")},
                                    {"query", matchDoc},
                                    {"limit", 3000},
                                    {"spherical", true},
                                };

                                var stage =
                                    new BsonDocumentPipelineStageDefinition<Auto, BsonDocument>(
                                        new BsonDocument { { "$geoNear", geoNearOptions } });
                                aggregate = GetCollection().Aggregate(new AggregateOptions()
                                {
                                    AllowDiskUse = true
                                })
                                    .AppendStage(stage)
                                    .ToListAsync()
                                    .Result.Count();
                            }
                            else
                            {
                                matchQuery = !string.IsNullOrEmpty(matchQuery) ? string.IsNullOrEmpty(advSearch.ZipCode)
                                 ? matchQuery
                                 : matchQuery + ",'DealerZip':'" + advSearch.ZipCode + "'" : "'DealerZip':'" + advSearch.ZipCode + "'";

                                matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                                aggregate = GetCollection().Aggregate(new AggregateOptions()
                                {
                                    AllowDiskUse = true
                                }).Match(matchDoc)
                                    .ToListAsync()
                                    .Result.Count();
                            }
                        }
                        else
                        {
                            matchQuery = !string.IsNullOrEmpty(matchQuery) ? string.IsNullOrEmpty(advSearch.ZipCode)
                               ? matchQuery
                               : matchQuery + ",'DealerZip':'" + advSearch.ZipCode + "'" : "'DealerZip':'" + advSearch.ZipCode + "'";

                            matchDoc = BsonSerializer.Deserialize<BsonDocument>("{" + matchQuery + "}");
                            aggregate = GetCollection().Aggregate(new AggregateOptions()
                            {
                                AllowDiskUse = true
                            }).Match(matchDoc)
                                .ToListAsync()
                                .Result.Count();
                        }
                    }
                    else
                    {
                        aggregate = (int)GetCollection().CountAsync(matchDoc).Result;
                    }
                }
                else
                {
                    aggregate = GetCollection().Aggregate(new AggregateOptions()
                    {
                        AllowDiskUse = true
                    })
                       .Match(matchDoc).ToListAsync().Result.Count();
                }

                return aggregate;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<CarColors> GetExteriorColor(string MakeName)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var matchQuery = string.IsNullOrEmpty(MakeName)
                    ? "{ExteriorColor : {$ne : ''}}"
                    : "{'Make': {" + Utility.UtilityClass.GetString(MakeName) + "},ExteriorColor : {$ne : ''}}";

                var group = "{_id : null,Colors : {$addToSet : '$ExteriorColor'}}";
                var projectQuery = "{_id : 0,Colors : 1}";

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<CarColors>(m));

                return aggregate.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CarColors> GetInteriorColor(string MakeName)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var matchQuery = string.IsNullOrEmpty(MakeName)
                                   ? "{InteriorColor : {$ne : ''}}"
                                   : "{'Make': {" + Utility.UtilityClass.GetString(MakeName) + "},InteriorColor : {$ne : ''}}";

                var group = "{_id : null,Colors : {$addToSet : '$InteriorColor'}}";
                var projectQuery = "{_id : 0,Colors : 1}";

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<CarColors>(m));

                return aggregate.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<AutoCompleteDetails> GetAutoCompleteDetails(string searchKey)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var matchQuery = "{'SearchString': {'$regex': '" + searchKey + "', '$options': 'i' }}";
                var group = "{_id: null,'SearchResult':{$addToSet: '$SearchString'}}";
                var projectQuery = "{_id : 0,SearchResult : 1}";

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                .Match(matchDoc)
                .Group(groupDoc)
                .Project(projectDoc).ToListAsync().Result.Select(m => BsonSerializer.Deserialize<AutoCompleteDetails>(m));

                return aggregate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CarYears> GetCarYear()
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var group = "{_id : null,Years : {$addToSet : '$Year'}}";
                var projectQuery = "{_id : 0,Years : 1}";
                var sort = "{Years : 1}";

                var sortDoc = BsonSerializer.Deserialize<BsonDocument>(sort);
                var groupDoc = BsonSerializer.Deserialize<BsonDocument>(group);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                .Group(groupDoc)
                .Project(projectDoc)
                .Sort(sortDoc)
                .ToListAsync().Result.Select(m => BsonSerializer.Deserialize<CarYears>(m));

                return aggregate.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcessFeed(List<Auto> lstAuto)
        {
            try
            {
                Console.WriteLine("Feed processing started.. ");
                Console.WriteLine(Environment.NewLine);
                // List<Auto> lstOld = new List<Auto>();

                var i = 0;
                foreach (var newAuto in lstAuto)
                {
                    Console.WriteLine(i);
                    i = i + 1;
                    Console.WriteLine(".");
                    // var category = Convert.ToString(newlisthublisting.ListingCategory).ToLower();

                    //In case of Classified Feed Vin number is coming blank so need to check otherwise "GetAutoDetailsByVinNumber" give error.
                    if (!String.IsNullOrEmpty(newAuto.Vin))
                    {

                        var hasAnyDoc = this.GetAutoDetailsByVinNumber(newAuto.Vin);

                        // Insert If No previous document found
                        if (hasAnyDoc != null)
                        {
                            if (hasAnyDoc.IsUpdatedByPortal != true)
                            {
                                string strVin = newAuto.Vin;
                                if (!String.IsNullOrEmpty(hasAnyDoc.Expiredate))
                                {
                                    DateTime expDate = Convert.ToDateTime(hasAnyDoc.Expiredate);
                                    int result = System.DateTime.Compare(expDate, System.DateTime.Today.Date);
                                    if (result > 0)
                                    {
                                        DeleteProperty(strVin, true);
                                    }
                                }
                            }

                        }
                        if (hasAnyDoc == null)
                        {

                            this.InsertAutoVertical(newAuto);
                        }
                        else
                        {
                            if (hasAnyDoc.IsUpdatedByPortal != true)
                            {
                                // Update if previous document found but w.r.t Modification timestamp we are updating the collection
                                var newlisting = newAuto;
                                var oldlisting = hasAnyDoc;
                                //lstOld.Add(hasAnyDoc);
                                //var newModifiedDate = Convert.ToDateTime(newlisting.ModificationTimestamp.Text);
                                //var oldModifiedDate = Convert.ToDateTime(oldlisting.ModificationTimestamp.Text);

                                //if (newModifiedDate > oldModifiedDate)
                                //{

                                var compareObjects = new CompareLogic()
                                {
                                    Config = new ComparisonConfig()
                                    {
                                        CompareChildren = true, //this turns deep compare one, otherwise it's shallow
                                        CompareFields = false,
                                        CompareReadOnly = true,
                                        ComparePrivateFields = false,
                                        ComparePrivateProperties = false,
                                        CompareProperties = true,
                                        MaxDifferences = 10000,
                                        IgnoreUnknownObjectTypes = true
                                    }
                                };

                                var resultCompare = compareObjects.Compare(newlisting, oldlisting);

                                var diff = resultCompare.Differences;
                                if (diff.Count > 0)
                                {
                                    foreach (var chnagedProperty in diff)
                                    {
                                        //if (chnagedProperty.ParentPropertyName != ".Id")
                                        //{
                                        if (chnagedProperty.PropertyName.ToLower().Contains("photosurl"))
                                        {
                                            //if (chnagedProperty.PropertyName.Contains("0"))
                                            //{
                                            this.SetUnSetSpecificProperty("", newAuto.Vin,
                                                chnagedProperty.PropertyName.Remove(0, 1)
                                                    .Replace("[", ".")
                                                    .Replace("]", ""),
                                                newlisting);
                                            // }

                                        }
                                        else if (chnagedProperty.PropertyName.ToLower().Contains("searcharray"))
                                        {
                                            if (chnagedProperty.PropertyName.ToLower().Any(char.IsDigit))
                                            {
                                                chnagedProperty.PropertyName = chnagedProperty.ParentPropertyName;
                                            }
                                            this.SetUnSetSpecificProperty("", newAuto.Vin,
                                                chnagedProperty.PropertyName.Remove(0, 1)
                                                    .Replace("[", ".")
                                                    .Replace("]", ""),
                                                newlisting);


                                        }
                                        //else if (chnagedProperty.PropertyName.ToLower().Contains("geolocation"))
                                        //{
                                        //    //if (chnagedProperty.PropertyName.Contains("0"))
                                        //    //{
                                        //    this.SetUnSetSpecificProperty("", newAuto.Vin,
                                        //        chnagedProperty.PropertyName.Remove(0, 1)
                                        //            .Replace("[", ".")
                                        //            .Replace("]", ""),
                                        //        newlisting);
                                        //    // }

                                        //}
                                        else
                                        {
                                            if (!chnagedProperty.PropertyName.Contains("ExtProperties") && !chnagedProperty.PropertyName.Contains("GeoLocation"))
                                            {
                                                this.SetSpecificProperty(newAuto.Vin,
                                                     chnagedProperty.PropertyName.Remove(0, 1)
                                                      .Replace("[", ".")
                                                       .Replace("]", ""),
                                                      chnagedProperty.Object1Value);
                                            }

                                        }

                                        //}
                                    }
                                }
                                //}
                            }

                        }

                    }
                    else
                    {
                        this.InsertAutoVertical(newAuto);
                    }//End of If newAuto.Vin != null or empty
                }
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Feed processing ended.. ");
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "InsertAutoVertical");
                throw;
            }
            //return lstOld;
        }

        public Auto GetAutoDetailsByVinNumber(string Vin)
        {
            try
            {
                base.CollectionName = "AutoVertical";// Convert.ToString(DbCollections.PurchaseListHubFeed);
                //var tmpList = new List<PropertyDetails>();

                var aggregate =
                    GetCollection().Find<Auto>(m => m.Vin == Vin).FirstOrDefaultAsync().Result;
                return aggregate;

            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "GetAutoDetailsByVinNumber");
                throw ex;
            }
        }

        public List<Auto> GetAllAuto()
        {
            try
            {
                base.CollectionName = "AutoVertical";// Convert.ToString(DbCollections.PurchaseListHubFeed);
                //var tmpList = new List<PropertyDetails>();

                // base.CollectionName = "AutoVertical";
                // var matchQuery = "{'_id': ObjectId('" + Id + "')}";
                var projectQuery =
                   "{'DealerId' : '$DealerId','DealerName' : '$DealerName','Make' : '$Make','Model':'$Model','Trim':'$Trim','Year':'$Year','Vin':'$Vin','Category':'$Category'," +
                   "'Mileage':'$Mileage','Price':'$Price','Condition':'$Condition','SearchArray':'$SearchArray','InteriorColor':'$InteriorColor','ExteriorColor':'$ExteriorColor'," +
                   "'Description':'$Description','DealershipPhone':'$DealershipPhone','DealershipCity':'$DealershipCity','DealershipState':'$DealershipState'," +
                   "'DealershipZip':'$DealershipZip','PhotosUrl':'$PhotosUrl','StockNumber':'$StockNumber','Transmission':'$Transmission','DealerAddress':'$DealerAddress'," +
                   "'DealerCity':'$DealerCity','DealerState':'$DealerState','DealerZip':'$DealerZip','DealerPhone':'$DealerPhone','DealerEmail':'$DealerEmail'," +
                   "'LeadEmailAddress':'$LeadEmailAddress','CCEmailAddress':'$CCEmailAddress','LeadType':'$LeadType','VehicleStyle':'$VehicleStyle','VehicleSize':'$VehicleSize','IsUpdatedByPortal':'$IsUpdatedByPortal'}";

                //var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })

                    .Project(projectDoc).ToListAsync().Result.Select(m => BsonSerializer.Deserialize<Auto>(m)).ToList<Auto>();

                // var myObj = BsonSerializer.Deserialize<Auto>(aggregate.FirstOrDefaultAsync().Result);
                return aggregate;

            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "GetAllAuto");
                throw ex;
            }
        }
        public bool InsertAutoVertical(Auto entities)
        {
            try
            {
                //if (type == "purchase")
                //{
                //    base.CollectionName = Convert.ToString(DbCollections.PurchaseListHubFeed);
                //}
                //else if (type == "rent")
                //{
                //    base.CollectionName = Convert.ToString(DbCollections.RentListHubFeed);
                //}
                //else
                //{
                //    base.CollectionName = Convert.ToString(DbCollections.NewHomesFeed);
                //}
                //var filter = Builders<ListHubListing>.Filter.Eq("UniqueNo", entities.UniqueNo);
                //var update = Builders<ListHubListing>.Update
                //    .Set("ExtProperties", entities);
                base.CollectionName = "AutoVertical";
                GetCollection().InsertOneAsync(entities).Wait();

                return true;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "InsertAutoVertical");
                throw;
            }
        }

        bool SetSpecificProperty(string uniqueidentifier, string propToUpdate, string value)
        {
            try
            {
                //base.CollectionName =
                //    Convert.ToString(type == "purchase"
                //        ? DbCollections.PurchaseListHubFeed
                //        : DbCollections.RentListHubFeed);
                if (value == "(null)")
                {
                    value = null;
                }
                var filter = Builders<Auto>.Filter.Eq("Vin", uniqueidentifier);
                var update = Builders<Auto>.Update
                    .Set(propToUpdate, value);

                var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                return results.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "SetSpecificProperty");
                throw;
            }
        }
        private bool SetUnSetSpecificProperty(string type,
            string uniqueidentifier, string unsetpropertyname, Auto lstAuto, bool onlySet = false)
        {
            try
            {
                //base.CollectionName =
                //    Convert.ToString(type == "purchase"
                //        ? DbCollections.PurchaseListHubFeed
                //        : DbCollections.RentListHubFeed);

                var filter = Builders<Auto>.Filter.Eq("Vin", uniqueidentifier);
                if (onlySet)
                {
                    var update = Builders<Auto>.Update.Set(unsetpropertyname,
                          lstAuto.PhotosUrl);
                    var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                    return results.ModifiedCount > 0;
                }
                else
                {
                    if (unsetpropertyname.Contains("PhotosUrl"))
                    {
                        if (lstAuto.PhotosUrl != null)
                        {
                            if (unsetpropertyname.Any(char.IsDigit))
                            {
                                unsetpropertyname = Regex.Replace(unsetpropertyname, @"[\d-]", string.Empty);
                                unsetpropertyname = unsetpropertyname.Remove(unsetpropertyname.Length - 1, 1);
                            }

                            var update1 = Builders<Auto>.Update.Unset(unsetpropertyname);
                            var tmpResult = GetCollection().UpdateOneAsync(filter, update1, new UpdateOptions()).Result;

                            if (tmpResult.ModifiedCount > 0)
                            {
                                var update = Builders<Auto>.Update.PushEach(unsetpropertyname,
                                    lstAuto.PhotosUrl);
                                var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                                return results.ModifiedCount > 0;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (unsetpropertyname.ToLower().Contains("searcharray"))
                    {
                        var update1 = Builders<Auto>.Update.Unset(unsetpropertyname);
                        var tmpResult = GetCollection().UpdateOneAsync(filter, update1, new UpdateOptions()).Result;

                        if (tmpResult.ModifiedCount > 0)
                        {
                            var update = Builders<Auto>.Update.PushEach(unsetpropertyname,
                                lstAuto.SearchArray);
                            var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                            return results.ModifiedCount > 0;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "SetUnSetSpecificProperty");
                throw;
            }
        }

        public bool DeleteProperty(string Vin, bool isDeleted)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var filter = Builders<Auto>.Filter.Eq("Vin", Vin);
                var update = Builders<Auto>.Update
                    .Set("ExtProperties.IsDeleted", isDeleted);

                var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                return results.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "DeleteProperty");
                throw;
            }
        }



        public bool SetExtraProperty(string type, ManageAutoViewModel autoViewModel)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var filter = Builders<Auto>.Filter.Eq("Vin", autoViewModel.UniqueId);
                var update = Builders<Auto>.Update
                    .Set("ExtProperties", autoViewModel);

                var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                return results.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "SetExtraProperty");
                throw;
            }
        }

        public ManageAutoViewModel GetExtraProperty(string type, string vin)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var aggregate = GetCollection().Find<Auto>(m => m.Vin == vin).FirstOrDefaultAsync().Result;
                return aggregate.ExtProperties;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "GetExtraProperty");
                throw;
            }
        }

        public string GetListedType(string type, string vin)
        {
            try
            {


                base.CollectionName = "AutoVertical";

                var aggregate = GetCollection().Find<Auto>(m => m.Vin == vin).FirstOrDefaultAsync().Result;
                return aggregate.ListedBy;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "GetListedType");
                throw;
            }
        }

        public bool UpdateAutoSpotFeatured(string vin, ManageAutoViewModel autoViewModel)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var updateResult = GetCollection().UpdateOneAsync(
                    m => m.Vin == vin,
                    Builders<Auto>.Update
                    .Set(m => m.ExtProperties.IsFeatured, autoViewModel.IsFeatured)
                    .Set(m => m.ExtProperties.IsSpotlight, autoViewModel.IsSpotlight)
                    .Set(m => m.ExtProperties.ShowMarketVal, autoViewModel.ShowMarketVal)
                    .Set(m => m.ExtProperties.ShowOwnershipCost, autoViewModel.ShowOwnershipCost)
                    .Set(m => m.IsUpdatedByPortal, true)
                    ).Result;
                return updateResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "UpdateAutoSpotFeatured");
                throw;

            };



        }

        public bool UpdateImageList(string type, Auto autoViewModel)
        {
            try
            {
                base.CollectionName = "AutoVertical";

                var filter = Builders<Auto>.Filter.Eq("Vin", autoViewModel.Vin);
                var update = Builders<Auto>.Update
                    .PushEach("PhotosUrl", autoViewModel.PhotosUrl).Set(m => m.IsUpdatedByPortal, true);

                var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                return results.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "UpdateImageList");
                throw;
            }
        }

        public Auto GetAutoDetailsByVin(string Id)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var matchQuery = "{'Vin': '" + Id + "'}";
                var projectQuery =
                   "{'DealerName' : '$DealerName','Make' : '$Make','Model':'$Model','Trim':'$Trim','Year':'$Year','Vin':'$Vin','Category':'$Category'," +
                   "'Mileage':'$Mileage','Price':'$Price','Condition':'$Condition','InteriorColor':'$InteriorColor','ExteriorColor':'$ExteriorColor'," +
                   "'Description':'$Description','DealershipPhone':'$DealershipPhone','DealershipCity':'$DealershipCity','DealershipState':'$DealershipState'," +
                   "'DealershipZip':'$DealershipZip','PhotosUrl':'$PhotosUrl','StockNumber':'$StockNumber','Transmission':'$Transmission','DealerAddress':'$DealerAddress'," +
                   "'DealerCity':'$DealerCity','DealerState':'$DealerState','DealerZip':'$DealerZip','DealerPhone':'$DealerPhone','DealerEmail':'$DealerEmail'," +
                   "'GeoLocation':'$GeoLocation','ChatScript':'$ChatScript','VehicleStyle':'$VehicleStyle','VehicleSize':'$VehicleSize'}";

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                    .Match(matchDoc)
                    .Project(projectDoc);//.ToListAsync().Result.Select(m => BsonSerializer.Deserialize<CarListing>(m));

                var myObj = BsonSerializer.Deserialize<Auto>(aggregate.FirstOrDefaultAsync().Result);
                return myObj;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "GetAutoDetailsByVin");
                throw;
            }
        }

        public Auto GetAutoDetailsById(string Id)
        {
            try
            {
                base.CollectionName = "AutoVertical";
                var matchQuery = "{'_id': ObjectId('" + Id + "')}";
                var projectQuery =
                   "{'DealerName' : '$DealerName','Make' : '$Make','Model':'$Model','Trim':'$Trim','Year':'$Year','Vin':'$Vin','Category':'$Category'," +
                   "'Mileage':'$Mileage','Price':'$Price','Condition':'$Condition','InteriorColor':'$InteriorColor','ExteriorColor':'$ExteriorColor'," +
                   "'Description':'$Description','DealershipPhone':'$DealershipPhone','DealershipCity':'$DealershipCity','DealershipState':'$DealershipState'," +
                   "'DealershipZip':'$DealershipZip','PhotosUrl':'$PhotosUrl','StockNumber':'$StockNumber','Transmission':'$Transmission','DealerAddress':'$DealerAddress'," +
                   "'DealerCity':'$DealerCity','DealerState':'$DealerState','DealerZip':'$DealerZip','DealerPhone':'$DealerPhone','DealerEmail':'$DealerEmail'," +
                   "'GeoLocation':'$GeoLocation','ChatScript':'$ChatScript','VehicleStyle':'$VehicleStyle','VehicleSize':'$VehicleSize','IsClassified':'$IsClassified','ExtProperties':'$ExtProperties'}";

                var matchDoc = BsonSerializer.Deserialize<BsonDocument>(matchQuery);
                var projectDoc = BsonSerializer.Deserialize<BsonDocument>(projectQuery);

                var aggregate = GetCollection().Aggregate(new AggregateOptions()
                {
                    AllowDiskUse = true
                })
                    .Match(matchDoc)
                    .Project(projectDoc);//.ToListAsync().Result.Select(m => BsonSerializer.Deserialize<CarListing>(m));

                var myObj = BsonSerializer.Deserialize<Auto>(aggregate.FirstOrDefaultAsync().Result);
                return myObj;
            }
            catch (Exception ex)
            {
                Utility.UtilityClass.WriteLogException(ex, "GetAutoDetailsById");
                throw;
            }
        }


        public bool PullImage(string imageurl, string vin)
        {
            try
            {
                base.CollectionName = "AutoVertical";


                var filter = Builders<Auto>.Filter.Eq(m => m.Vin, vin);
                var update = Builders<Auto>.Update
                    .Pull(m => m.PhotosUrl, imageurl);

                var results = GetCollection().UpdateOneAsync(filter, update, new UpdateOptions()).Result;

                return results.ModifiedCount > 0;
            }
            catch (Exception ex)
            {

                return false;
            }
        }



        //public bool InsertBulkAutoVertical(List<Repository.Models> entities)
        //{
        //    var count = 0;
        //    base.CollectionName = "AutoVertical";
        //    var addresslist = new HashSet<AddressWithLatLong>();
        //    var zipcodes = new HashSet<AddressWithLatLong>();
        //    foreach (var entity in entities)
        //    {
        //        count = count + 1;

        //        if (!string.IsNullOrEmpty(entity.DealerAddress))
        //        {
        //            var address = addresslist.FirstOrDefault(m => m.Address == entity.DealerAddress);
        //            if (address != null)
        //            {
        //                entity.GeoLocation = new GeoPoint(address.Coordinates.lat, address.Coordinates.lng);

        //            }
        //            else
        //            {
        //                var fetchedLatlong = _fetchLatLong.GetLatitudeAndLongitude(entity.DealerAddress);

        //                if (fetchedLatlong != null)
        //                {
        //                    addresslist.Add(new AddressWithLatLong
        //                    {
        //                        Address = entity.DealerAddress,
        //                        Coordinates = fetchedLatlong
        //                    });
        //                    entity.GeoLocation = new GeoPoint(fetchedLatlong.lat, fetchedLatlong.lng);
        //                }
        //            }
        //        }
        //        else if (!string.IsNullOrEmpty(entity.DealerZip))
        //        {
        //            var zipCode = zipcodes.FirstOrDefault(m => m.Address == entity.DealerZip);
        //            if (zipCode != null)
        //            {
        //                entity.GeoLocation = new GeoPoint(zipCode.Coordinates.lat, zipCode.Coordinates.lng);

        //            }
        //            else
        //            {
        //                var fetchedLatlong = _fetchLatLong.GetLatitudeAndLongitude(entity.DealerZip);

        //                if (fetchedLatlong != null)
        //                {
        //                    zipcodes.Add(new AddressWithLatLong
        //                    {
        //                        Address = entity.DealerAddress,
        //                        Coordinates = fetchedLatlong
        //                    });
        //                    entity.GeoLocation = new GeoPoint(fetchedLatlong.lat, fetchedLatlong.lng);
        //                }
        //            }
        //        }
        //    }
        //    return InsertBulk(entities);
        //}

        //Auto GetPurchasePropertyDetailsByMLSNumber(string mlsNumber);
    }
}
