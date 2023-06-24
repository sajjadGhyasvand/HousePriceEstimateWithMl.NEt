

using MLHousePrice.Models.Services.DTOs;

namespace MLHousePrice.Models.ViewModels
{
    public class AdvertisementReportViewModel
    {
        public IEnumerable<LocationReportDto> LocationReports { get; set; }
        public int TotalAdvertisements { get; set; }
    }

}
