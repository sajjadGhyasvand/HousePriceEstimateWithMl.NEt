namespace MLHousePrice.Models.Entities
{
    public class Advertisement
    {
        public int Id { get; set; }
        public int Area { get; set; }
        public int BuildYear { get; set; }
        public int Rooms { get; set; }
        public long TotalPrice { get; set; }
        public int Floor { get; set; }
        public bool Elevator { get; set; }
        public bool Parking { get; set; }
        public bool Storage { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
