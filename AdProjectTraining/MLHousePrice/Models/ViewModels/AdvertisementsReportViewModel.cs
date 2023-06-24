

using MLHousePrice.Models.Services.DTOs;

namespace MLHousePrice.Models.ViewModels
{
    public class AdvertisementsReportViewModel
    {
        public PaginatedAdvertisementsDto PaginatedAdvertisements { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }

}
