using System.ServiceModel;
using System.ServiceModel.Web;

namespace UD2.Services
{
    [ServiceContract]
    public interface ILMS
    {
        [OperationContract]
        [WebGet]
        string Address(string id);
    }
}