

using MLHousePrice.Models.Services.DTOs;

namespace MLHousePrice.Models.Services
{
    public interface IAdvertisementService
    {
        Task<AdvertisementListDto> CreateAdvertisementAsync(CreateAdvertisementDto createAdvertisementDto);
        Task<PaginatedAdvertisementsDto> GetAdvertisementsAsync(int pageNumber, int pageSize);
        Task<AdvertisementDetailsDto> GetAdvertisementDetailsAsync(int id);
        Task<PredictedPriceDto> PredictPriceAsync(PredictPriceInputDto input);

        Task<IEnumerable<LocationReportDto>> GetLocationReportsAsync();
        Task<int> GetTotalAdvertisementsAsync();
    }

}
