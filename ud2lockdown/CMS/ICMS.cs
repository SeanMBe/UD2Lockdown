using System.ServiceModel;

namespace CMS
{
    [ServiceContract]
    public interface ICMS
    {
        [OperationContract]
        string Customer(string id);
    }
}