namespace LMS
{
    public class Lms : ILMS
    {
        public string Address(string id)
        {
            return "AddressId=7,StreetNumber=1234";
        }
                                                 
        public static string Uri { get { return "http://localhost:7979/service.svc/lms"; } }
    }
}