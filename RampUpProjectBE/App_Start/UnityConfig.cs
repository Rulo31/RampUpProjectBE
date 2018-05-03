using RampUpProjectBE.Services;
using RampUpProjectBE.Utils;
using System.Configuration;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace RampUpProjectBE
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            string key = ConfigurationManager.AppSettings["EncryptionKey"];
            string rampDbUser = ConfigurationManager.AppSettings["RampDBUser"];
            string rampDbPassword = ConfigurationManager.AppSettings["RampDBPassword"];
            string rampConnectionString = string.Format(ConfigurationManager.ConnectionStrings["RampUpProjectEntities"].ToString(), Encryption.Decrypt(rampDbUser, key), Encryption.Decrypt(rampDbPassword, key));

            container.RegisterType<UserService>(new InjectionConstructor(rampConnectionString, key));
            container.RegisterType<BusinessService>(new InjectionConstructor(rampConnectionString));
            container.RegisterType<BranchService>(new InjectionConstructor(rampConnectionString));
            container.RegisterType<ReservationService>(new InjectionConstructor(rampConnectionString));
            container.RegisterType<UserPaymentService>(new InjectionConstructor(rampConnectionString));
            container.RegisterType<PaymentMethodService>(new InjectionConstructor(rampConnectionString));
            container.RegisterType<FieldService>(new InjectionConstructor(rampConnectionString));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}