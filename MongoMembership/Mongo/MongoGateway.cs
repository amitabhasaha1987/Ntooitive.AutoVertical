using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoMembership.Utils;

namespace MongoMembership.Mongo
{
    internal class MongoGateway : IMongoGateway
    {
        private readonly IMongoDatabase dataBase;
        private IMongoCollection<User> UsersCollection
        {
            get { return dataBase.GetCollection<User>(typeof(User).Name); }
        }
        private IMongoCollection<Role> RolesCollection
        {
            get { return dataBase.GetCollection<Role>(typeof(Role).Name); }
        }

        static MongoGateway()
        {
            RegisterClassMapping();
        }

        public MongoGateway(string mongoConnectionString)
        {
            var mongoUrl = new MongoUrl(mongoConnectionString);
            var client = new MongoClient(mongoConnectionString);
            //var server = client.GetServer();
            dataBase = client.GetDatabase(mongoUrl.DatabaseName);
            CreateIndex();
        }

        public void DropUsers()
        {
            UsersCollection.Database.DropCollectionAsync(typeof(User).Name);
        }

        public void DropRoles()
        {
            RolesCollection.Database.DropCollectionAsync(typeof(User).Name);
        }

        #region User
        public void CreateUser(User user)
        {
            if (user.Username != null) user.UsernameLowercase = user.Username.ToLowerInvariant();
            if (user.Email != null) user.EmailLowercase = user.Email.ToLowerInvariant();

            UsersCollection.InsertOneAsync(user);
        }

        public void UpdateUser(User user)
        {
            try
            {
                var res = UsersCollection.UpdateOneAsync(Builders<User>.Filter.Eq(m => m.Id, user.Id),Builders<User>.Update
                                                            .Set(m => m.ApplicationName, user.ApplicationName)
                                                            .Set(m => m.Username, user.Username)
                                                            .Set(m => m.UsernameLowercase, user.UsernameLowercase)
                                                            .Set(m => m.Email, user.Email)
                                                            .Set(m => m.EmailLowercase, user.EmailLowercase)
                                                            .Set(m => m.Comment, user.Comment)
                                                            .Set(m => m.Password, user.Password)
                                                            .Set(m => m.PasswordSalt, user.PasswordSalt)
                                                            .Set(m => m.PasswordQuestion, user.PasswordQuestion)
                                                            .Set(m => m.PasswordAnswer, user.PasswordAnswer)
                                                            .Set(m => m.IsApproved, user.IsApproved)
                                                            .Set(m => m.IsDeleted, user.IsDeleted)
                                                            .Set(m => m.IsDeleted, user.IsLockedOut)
                                                            .Set(m => m.IsDeleted, user.IsAnonymous)
                                                            .Set(m => m.LastActivityDate, user.LastActivityDate)
                                                            .Set(m => m.LastLoginDate, user.LastLoginDate)
                                                            .Set(m => m.LastPasswordChangedDate, user.LastPasswordChangedDate)
                                                            .Set(m => m.CreateDate, user.CreateDate)
                                                            .Set(m => m.LastLockedOutDate, user.LastLockedOutDate)
                                                            .Set(m => m.LastUpdatedDate, user.LastUpdatedDate)
                                                            .Set(m => m.FailedPasswordAttemptCount, user.FailedPasswordAttemptCount)
                                                            .Set(m => m.FailedPasswordAttemptWindowStart, user.FailedPasswordAttemptWindowStart)
                                                            .Set(m => m.FailedPasswordAnswerAttemptCount, user.FailedPasswordAnswerAttemptCount)
                                                            .Set(m => m.FailedPasswordAnswerAttemptWindowStart, user.FailedPasswordAnswerAttemptWindowStart)
                                                            .Set(m => m.Roles, user.Roles)
                                                            .Set(m => m.Values, user.Values)


                                                         ).Result;

                Console.WriteLine(res.ModifiedCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveUser(User user)
        {
            user.IsDeleted = true;
            UpdateUser(user);
        }

        public User GetById(string id)
        {
            if (id.IsNullOrWhiteSpace() || UsersCollection.CountAsync(new BsonDocument()).Result == 0)
                return null;
            using (var cursor = UsersCollection.FindAsync(m => m.Id == id).Result)
            {
                return cursor.MoveNextAsync().Result ? cursor.Current.FirstOrDefault() : null;
            }

        }

        public User GetByUserName(string applicationName, string username)
        {
            if (username.IsNullOrWhiteSpace() || UsersCollection.CountAsync(new BsonDocument()).Result == 0)
                return null;

            using (var cursor = UsersCollection.FindAsync(user
                => user.ApplicationName == applicationName
                   && user.UsernameLowercase == username.ToLowerInvariant()
                   && user.IsDeleted == false).Result)
            {
                return cursor.MoveNextAsync().Result ? cursor.Current.FirstOrDefault() : null;

            }
        }

        public User GetByEmail(string applicationName, string email)
        {
            if (email.IsNullOrWhiteSpace() || UsersCollection.CountAsync(new BsonDocument()).Result == 0)
                return null;

            using (var cursor = UsersCollection.FindAsync(user
                        => user.ApplicationName == applicationName
                        && user.EmailLowercase == email.ToLowerInvariant()
                        && user.IsDeleted == false).Result)
            {
                return cursor.MoveNextAsync().Result ? cursor.Current.FirstOrDefault() : null;
            }
        }

        public IEnumerable<User> GetAllByEmail(string applicationName, string email, int pageIndex, int pageSize, out int totalRecords)
        {
            if (email.IsNullOrWhiteSpace() || UsersCollection.CountAsync(new BsonDocument()).Result == 0)
            {
                totalRecords = 0;
                return Enumerable.Empty<User>();
            }
            using (var cursor = UsersCollection.FindAsync(user
                            => user.ApplicationName == applicationName
                            && user.EmailLowercase.Contains(email.ToLowerInvariant())
                            && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }

        }

        public IEnumerable<User> GetAllByUserName(string applicationName, string username, int pageIndex, int pageSize, out int totalRecords)
        {
            if (username.IsNullOrWhiteSpace() || UsersCollection.CountAsync(new BsonDocument()).Result == 0)
            {
                totalRecords = 0;
                return Enumerable.Empty<User>();
            }

            using (var cursor = UsersCollection.FindAsync(user
                            => user.ApplicationName == applicationName
                            && user.UsernameLowercase.Contains(username.ToLowerInvariant())
                            && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }

        }

        public IEnumerable<User> GetAllAnonymByUserName(string applicationName, string username, int pageIndex, int pageSize, out int totalRecords)
        {
            if (username.IsNullOrWhiteSpace() || UsersCollection.CountAsync(new BsonDocument()).Result == 0)
            {
                totalRecords = 0;
                return Enumerable.Empty<User>();
            }
            using (var cursor = UsersCollection.FindAsync(user
                    => user.ApplicationName == applicationName
                    && user.UsernameLowercase.Contains(username.ToLowerInvariant())
                    && user.IsAnonymous
                    && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }
        }

        public IEnumerable<User> GetAll(string applicationName, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = (int)UsersCollection.CountAsync(new BsonDocument()).Result;

            if (totalRecords == 0)
                return Enumerable.Empty<User>();

            using (var cursor = UsersCollection.FindAsync(user
                        => user.ApplicationName == applicationName
                        && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }
        }

        public IEnumerable<User> GetAllAnonym(string applicationName, int pageIndex, int pageSize, out int totalRecords)
        {
            if (UsersCollection.CountAsync(new BsonDocument()).Result == 0)
            {
                totalRecords = 0;
                return Enumerable.Empty<User>();
            }
            using (var cursor = UsersCollection.FindAsync(user
                    => user.ApplicationName == applicationName
                    && user.IsAnonymous
                    && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }
        }

        public IEnumerable<User> GetAllInactiveSince(string applicationName, DateTime inactiveDate, int pageIndex, int pageSize, out int totalRecords)
        {
            if (UsersCollection.CountAsync(new BsonDocument()).Result == 0)
            {
                totalRecords = 0;
                return Enumerable.Empty<User>();
            }


            using (var cursor = UsersCollection.FindAsync(user
                            => user.ApplicationName == applicationName
                            && user.LastActivityDate <= inactiveDate
                            && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }
        }

        public IEnumerable<User> GetAllInactiveAnonymSince(string applicationName, DateTime inactiveDate, int pageIndex, int pageSize, out int totalRecords)
        {
            if (UsersCollection.CountAsync(new BsonDocument()).Result == 0)
            {
                totalRecords = 0;
                return Enumerable.Empty<User>();
            }
            using (var cursor = UsersCollection.FindAsync(user
                            => user.ApplicationName == applicationName
                            && user.LastActivityDate <= inactiveDate
                            && user.IsAnonymous
                            && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }
        }

        public IEnumerable<User> GetInactiveSinceByUserName(string applicationName, string username, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            if (UsersCollection.CountAsync(new BsonDocument()).Result == 0)
            {
                totalRecords = 0;
                return Enumerable.Empty<User>();
            }
            using (var cursor = UsersCollection.FindAsync(user
                            => user.ApplicationName == applicationName
                            && user.UsernameLowercase.Contains(username.ToLowerInvariant())
                            && user.LastActivityDate <= userInactiveSinceDate
                            && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }
        }

        public IEnumerable<User> GetInactiveAnonymSinceByUserName(string applicationName, string username, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            if (UsersCollection.CountAsync(new BsonDocument()).Result == 0)
            {
                totalRecords = 0;
                return Enumerable.Empty<User>();
            }

            using (var cursor = UsersCollection.FindAsync(user
                            => user.ApplicationName == applicationName
                            && user.UsernameLowercase.Contains(username.ToLowerInvariant())
                            && user.LastActivityDate <= userInactiveSinceDate
                            && user.IsAnonymous
                            && user.IsDeleted == false).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    totalRecords = cursor.Current.Count();

                    return cursor.Current.Skip(pageIndex * pageSize).Take(pageSize);

                }
                else
                {
                    totalRecords = 0;
                    return Enumerable.Empty<User>();
                }
            }
        }

        public int GetUserForPeriodOfTime(string applicationName, TimeSpan timeSpan)
        {
            return (int)UsersCollection.CountAsync(user
                        => user.ApplicationName == applicationName
                        && user.LastActivityDate > DateTime.UtcNow.Subtract(timeSpan)).Result;
        }
        #endregion

        #region Role
        public void CreateRole(Role role)
        {
            if (role.RoleName != null) role.RoleNameLowercased = role.RoleName.ToLowerInvariant();

            RolesCollection.InsertOneAsync(role);
        }

        public void RemoveRole(string applicationName, string roleName)
        {

            RolesCollection.DeleteOneAsync(Builders<Role>.Filter.And(
                Builders<Role>.Filter.Eq(Util.GetElementNameFor<Role>(_ => _.ApplicationName), applicationName),
                Builders<Role>.Filter.Eq(Util.GetElementNameFor<Role>(_ => _.RoleNameLowercased), applicationName)
                ));
        }

        public string[] GetAllRoles(string applicationName)
        {
            using (var cursor = RolesCollection.FindAsync(role => role.ApplicationName == applicationName).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    return cursor.Current
                    .Select(role => role.RoleName)
                    .ToArray();
                }
                else
                {
                    return new List<string>().ToArray();
                }
            }

        }

        public string[] GetRolesForUser(string applicationName, string username)
        {
            if (username.IsNullOrWhiteSpace())
                return null;

            User user = GetByUserName(applicationName, username);

            if (user == null || user.Roles == null)
                return null;

            return user.Roles.ToArray();
        }

        public string[] GetUsersInRole(string applicationName, string roleName)
        {
            if (roleName.IsNullOrWhiteSpace())
                return null;

            using (var cursor = UsersCollection.FindAsync(user
                        => user.ApplicationName == applicationName
                        && (user.Roles.Contains(roleName.ToLowerInvariant()) || user.Roles.Contains(roleName))).Result)
            {
                if (cursor.MoveNextAsync().Result)
                {
                    return cursor.Current
                    .Select(user => user.Username)
                    .ToArray();
                }
                else
                {
                    return new List<string>().ToArray();
                }
            }
        }

        public bool IsUserInRole(string applicationName, string username, string roleName)
        {
            if (username.IsNullOrWhiteSpace() || roleName.IsNullOrWhiteSpace())
                return false;

            return UsersCollection.CountAsync(user
                        => user.ApplicationName == applicationName
                        && user.UsernameLowercase == username.ToLowerInvariant()
                        && (user.Roles.Contains(roleName.ToLowerInvariant()) || user.Roles.Contains(roleName))).Result > 0;
        }

        public bool IsRoleExists(string applicationName, string roleName)
        {
            if (roleName.IsNullOrWhiteSpace())
                return false;

            return RolesCollection.CountAsync(role
                        => role.ApplicationName == applicationName
                        && role.RoleNameLowercased == roleName.ToLowerInvariant()).Result > 0;
        }
        #endregion

        #region Private Methods
        private static void RegisterClassMapping()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                // Initialize Mongo Mappings
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.SetIsRootClass(true);
                    cm.MapIdField(c => c.Id);
                    cm.MapProperty(c => c.ApplicationName).SetElementName("ApplicationName");
                    cm.MapProperty(c => c.Username).SetElementName("Username");
                    cm.MapProperty(c => c.UsernameLowercase).SetElementName("UsernameLowercase");
                    cm.MapProperty(c => c.Comment).SetElementName("Comment");
                    cm.MapProperty(c => c.CreateDate).SetElementName("CreateDate");
                    cm.MapProperty(c => c.Email).SetElementName("Email");
                    cm.MapProperty(c => c.EmailLowercase).SetElementName("EmailLowercase");
                    cm.MapProperty(c => c.FailedPasswordAnswerAttemptCount).SetElementName("FailedPasswordAnswerAttemptCount");
                    cm.MapProperty(c => c.FailedPasswordAttemptCount).SetElementName("FailedPasswordAttemptCount");
                    cm.MapProperty(c => c.FailedPasswordAnswerAttemptWindowStart).SetElementName("FailedPasswordAnswerAttemptWindowStart");
                    cm.MapProperty(c => c.FailedPasswordAttemptWindowStart).SetElementName("FailedPasswordAttemptWindowStart");
                    cm.MapProperty(c => c.IsApproved).SetElementName("IsApproved");
                    cm.MapProperty(c => c.IsDeleted).SetElementName("IsDeleted");
                    cm.MapProperty(c => c.IsLockedOut).SetElementName("IsLockedOut");
                    cm.MapProperty(c => c.LastActivityDate).SetElementName("LastActivityDate");
                    cm.MapProperty(c => c.LastLockedOutDate).SetElementName("LastLockedOutDate");
                    cm.MapProperty(c => c.LastLoginDate).SetElementName("LastLoginDate");
                    cm.MapProperty(c => c.LastPasswordChangedDate).SetElementName("LastPasswordChangedDate");
                    cm.MapProperty(c => c.Password).SetElementName("Password");
                    cm.MapProperty(c => c.PasswordAnswer).SetElementName("PasswordAnswer");
                    cm.MapProperty(c => c.PasswordQuestion).SetElementName("PasswordQuestion");
                    cm.MapProperty(c => c.PasswordSalt).SetElementName("PasswordSalt");
                    cm.MapProperty(c => c.Roles).SetElementName("Roles").SetIgnoreIfNull(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Role)))
            {
                BsonClassMap.RegisterClassMap<Role>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.SetIsRootClass(true);
                    cm.MapProperty(c => c.ApplicationName).SetElementName("ApplicationName");
                    cm.MapProperty(c => c.RoleName).SetElementName("RoleName");
                    cm.MapProperty(c => c.RoleNameLowercased).SetElementName("RoleNameLowercased");
                });
            }
        }

        private void CreateIndex()
        {
            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)));


            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.EmailLowercase))));


            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.UsernameLowercase))));

            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.Roles))));

            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.UsernameLowercase)),
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.Roles))));

            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.IsAnonymous))));

            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
                 Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
                 Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.LastActivityDate)),
                 Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.IsAnonymous))));

            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.LastActivityDate)),
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.UsernameLowercase)),
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.IsAnonymous))));


            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.UsernameLowercase)),
               Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.IsAnonymous))));

            UsersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Combine(
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.ApplicationName)),
                Builders<User>.IndexKeys.Ascending(Util.GetElementNameFor<User>(_ => _.LastActivityDate))));

            RolesCollection.Indexes.CreateOneAsync(Util.GetElementNameFor<Role>(_ => _.ApplicationName));

            RolesCollection.Indexes.CreateOneAsync(Builders<Role>.IndexKeys.Combine(
                Builders<Role>.IndexKeys.Ascending(Util.GetElementNameFor<Role>(_ => _.ApplicationName)),
                Builders<Role>.IndexKeys.Ascending(Util.GetElementNameFor<Role>(_ => _.RoleNameLowercased))));

        }
        #endregion
    }
}