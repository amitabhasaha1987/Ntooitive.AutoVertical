using System;
using MongoDbRepository;
using MongoDbRepository.Implementation;
using MongoDbRepository.Implementation.APIFetch;
using MongoDbRepository.Implementation.DataReader;
using MongoDB.Driver;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;

using Repository.Interfaces;
using MongoDbRepository.Implementation.FileDownloader;
using MongoDbRepository.Implementation.FileUnzipper;
using MongoDbRepository.Implementation.Mail;
using Repository.Interfaces.Admin.Dealer;
using MongoDbRepository.Implementation.Dealer;
using Repository.Interfaces.Mail;
using Repository.Interfaces.Admin.Auto;
using MongoDbRepository.Implementation.Admin;
using MongoDbRepository.Implementation.CoordinatesImp;
using Repository.Interfaces.Downloader;
//using MongoDbRepository.Implementation.TextFileCoverter;

namespace Configuration
{
    public static class NinjectConfig
    {
        private static IKernel _kernel;
        /// <summary>
        /// To load specific module per need
        /// </summary>
        /// <typeparam name="T">Interface to get implementaion</typeparam>
        /// <returns>new object of implemented interface</returns>
        public static T Get<T>()
        {
            try
            {
                if (_kernel == null)
                {
                    _kernel = CreateKernel();
                }
                return _kernel.Get<T>();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Some exception occured while trying to get the implementation. Please check the error message : " + ex.Message);
                return default(T);
            }
        }
        /// <summary>
        /// To load all modules for schedulers
        /// </summary>
        public static void StartScheduler()
        {
            CreateKernel();
        }
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);

            RegisterServices(kernel);

            return kernel;
        }
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IMongoDatabase>().ToMethod(BuildMongoDatabase).InSingletonScope();
            kernel.Bind<IReader>().To<TSVReader>();
            kernel.Bind<IAutoVertical>().To<AutoVertical>();
            kernel.Bind<IFetchLatLong>().To<GetLatLongFromBing>();
            kernel.Bind<IDownloader>().To<FileDownloader>();
            kernel.Bind<IUnzipper>().To<FileUnzipper>();
            kernel.Bind<IMailBase>().To<MailgunBase>();
            kernel.Bind<IDealer>().To<DealerHandler>();
            kernel.Bind<IAuto>().To<AutoHandler>();
            kernel.Bind<IFetchAutoDetailsFromAPI>().To<FetchAutoDetailsFromAPIByVin>();
            kernel.Bind<ICoordinates>().To<CoordinatesImplimentation>();
            kernel.Bind<IFetchMarketValueFromAPIByVin>().To<GetMarketValueFromAPIByVin>();
            kernel.Bind<IFetchOwnershipCostFromAPIByVin>().To<GetOwnershipCostFromAPIByVin>();
            kernel.Bind<IFetcher>().To<AutoDataFetch>();
        }
        /// <summary>
        /// Create Singletone DB connetion
        /// </summary>
        /// <param name="arg">Registerd Context of the application</param>
        /// <returns>Returns Database Object</returns>
        private static IMongoDatabase BuildMongoDatabase(IContext arg)
        {
            return Factory.CreateMongoDatabase();
        }

    }
}
