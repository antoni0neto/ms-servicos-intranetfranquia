using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DAL;

namespace Relatorios.wcfMobileApp
{
    [ServiceContract]
    public interface IVendaOnline
    {

        [OperationContract]
        [WebGet(UriTemplate = "helloApp",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        bool helloApp();

        [OperationContract]
        [WebGet(UriTemplate = "getUser/{user}/{password}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        int getUser(string user, string password);

        [OperationContract]
        [WebGet(UriTemplate = "getBranch/{usercode}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        IList<FILIAI1> getBranch(string usercode);

        [OperationContract]
        [WebGet(UriTemplate = "getBranchInformation/{_branch}/{_dataIni}/{_dataFim}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        SP_APP_OBTER_DESEMPENHOResult getBranchInformation(string _branch, string _dataIni, string _dataFim);

        [OperationContract]
        [WebGet(UriTemplate = "getBranchGoal/{_dataIni}/{_dataFim}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        IList<SP_APP_OBTER_DESEMPENHOResult> getBranchGoal(string _dataIni, string _dataFim);

        [OperationContract]
        [WebGet(UriTemplate = "getBranchSupervisor/{_dataIni}/{_dataFim}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        IList<SP_APP_OBTER_DESEMPENHOResult> getBranchSupervisor(string _dataIni, string _dataFim);

        [OperationContract]
        [WebGet(UriTemplate = "getBranchTotal/{_dataIni}/{_dataFim}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        SP_APP_OBTER_DESEMPENHOResult getBranchTotal(string _dataIni, string _dataFim);

        [OperationContract]
        [WebGet(UriTemplate = "getSalesManInformation/{_branch}/{_dataIni}/{_dataFim}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        IList<SP_Vendas_VendedorResult> getSalesManInformation(string _branch, string _dataIni, string _dataFim);

    }

}
