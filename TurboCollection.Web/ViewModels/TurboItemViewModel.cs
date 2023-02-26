namespace TurboCollection.Web.ViewModels
{
    public class TurboItemViewModel
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public int Number { get; set; }
        public string Picture { get; set; }
        public string? Name { get; set; }
        public int? Speed { get; set; }
        public int? EngineCapacity { get; set; }
        public int? HorsePower { get; set; }
        public int? Year { get; set; }
        public string? Tags { get; set; }
    }
}
