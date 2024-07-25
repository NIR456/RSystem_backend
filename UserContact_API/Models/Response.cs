namespace UserContact_API.Models
{
    public class Response
    {
        public bool ResponseStatus { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public object ResponseObject { get; set; } = new object();
    }
}
