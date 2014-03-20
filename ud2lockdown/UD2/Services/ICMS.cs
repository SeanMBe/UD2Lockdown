using System.ServiceModel;
using System.ServiceModel.Web;

namespace UD2.Services
{
    [ServiceContract]
    public interface ICMS
    {
        [OperationContract]
        [WebGet]
        string Customer(string id);
    }
}