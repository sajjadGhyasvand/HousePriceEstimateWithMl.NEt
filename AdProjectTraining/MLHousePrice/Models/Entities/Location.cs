namespace MLHousePrice.Models.Entities
{
    public class Location
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}
