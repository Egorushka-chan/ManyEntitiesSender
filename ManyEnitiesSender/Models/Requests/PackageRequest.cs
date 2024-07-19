namespace ManyEntitiesSender.BLL.Models.Requests
{
    public class PackageRequest
    {
        public string Table { get; set; } = "Body";
        public string? Filter { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
