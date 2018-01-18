namespace Fetch.Core
{
    public class Input
    {
        public string Method { get; set; }
        public object Headers { get; set; }
        public string JsonHeaders { get; set; }
        public string Url { get; set; }
        public object Body { get; set; }
        public string JsonBody { get; set; }
        public bool BodyContainsFunc { get; set; }
    }
}