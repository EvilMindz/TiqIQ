using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace QuizApp.Svc
{
    [ServiceContract]    
    public interface IDataService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "data", BodyStyle = WebMessageBodyStyle.Bare)]
        Message GetData();
    }
}