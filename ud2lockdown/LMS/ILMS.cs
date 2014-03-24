using System.ServiceModel;

namespace LMS
{
    [ServiceContract]
    public interface ILMS
    {
        [OperationContract]
        string Address(string id);
    }
}