namespace RLabs.Cielo.SDK.Model.Response
{
    public class BaseResponse
    {
        public int Status { get; set; }
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public string InternalCode { get; set; }
        public string InternalMessage { get; set; }
    }
}