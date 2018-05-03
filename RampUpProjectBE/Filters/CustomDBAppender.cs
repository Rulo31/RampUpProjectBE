using log4net.Appender;
using RampUpProjectBE.Utils;
using System.Configuration;

namespace RampUpProjectBE.Filters {
    public class CustomDBAppender : AdoNetAppender {
        public new string ConnectionString {
            get { return base.ConnectionString; }
            set {
                string key = ConfigurationManager.AppSettings["EncryptionKey"];
                string rampDbUser = ConfigurationManager.AppSettings["RampDBUser"];
                string rampDbPassword = ConfigurationManager.AppSettings["RampDBPassword"];
                string rampConnectionString = string.Format(ConfigurationManager.ConnectionStrings["RampUpProjectEntities"].ToString(), Encryption.Decrypt(rampDbUser, key), Encryption.Decrypt(rampDbPassword, key));
                base.ConnectionString = rampConnectionString;
            }
        }
    }

}