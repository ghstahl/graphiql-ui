namespace Programs.Models
{
    public class IsInstalledQuery
    {
        public string DisplayName { get; set; }
    }
    public class IsInstalledOutput
    {
        public string DisplayName { get; set; }
        public bool IsInstalled { get; set; }
    }
}