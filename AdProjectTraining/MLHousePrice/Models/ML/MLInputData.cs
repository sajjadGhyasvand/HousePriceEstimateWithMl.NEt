using Microsoft.ML.Data;

namespace MLHousePrice.Models.ML
{
    public class MLInputData
    {
        public Single Area;
        public Single BuildYear;
        public Single Rooms;
        public Single TotalPrice;
        public string LocationName;
        public Single Floor;
        public bool Elevator;
        public bool Parking;
        public bool Storage;
        public string Description;
        public string Title;
    }
    public class OutPutData
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}
