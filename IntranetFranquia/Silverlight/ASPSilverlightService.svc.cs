using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.ServiceModel.Activation;
using DAL;

namespace Relatorios
{
    // NOTE: If you change the class name "SessionService" here, you must also update the reference to "SessionService" in Web.config.
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ASPSilverlightService
    {
        public void DoWork()
        {
        }

        #region ISessionService Members

        [OperationContract]
        public object GetSessionValueForKey(string sessionKey)
        {
            return System.Web.HttpContext.Current.Session[sessionKey];
        }

        [OperationContract]
        public void SetSessionValueForKey(string sessionKey, object sessionValue)
        {
            System.Web.HttpContext.Current.Session[sessionKey] = sessionValue;
        }

        #endregion
    }
}
