namespace Fetch.Core
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object Value { get; set; }
    }
}