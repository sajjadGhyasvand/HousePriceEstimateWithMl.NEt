
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MLHousePrice.Models.Context;
using MLHousePrice.Models.Entities;
using MLHousePrice.Models.Services.DTOs;
using System.Globalization;

namespace MLHousePrice.Models.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;

        public AdvertisementService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AdvertisementListDto> CreateAdvertisementAsync(CreateAdvertisementDto createAdvertisementDto)
        {
            var advertisement = _mapper.Map<Advertisement>(createAdvertisementDto);
            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();

            return _mapper.Map<AdvertisementListDto>(advertisement);
        }

        public async Task<PaginatedAdvertisementsDto> GetAdvertisementsAsync(int pageNumber, int pageSize)
        {
            var advertisements = await _context.Advertisements
                .Include(a => a.Location)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.Advertisements.CountAsync();

            var paginatedAdvertisements = new PaginatedAdvertisementsDto
            {
                Advertisements = _mapper.Map<IEnumerable<AdvertisementListDto>>(advertisements),
                TotalCount = totalCount
            };
            return paginatedAdvertisements;
        }
        public async Task<AdvertisementDetailsDto> GetAdvertisementDetailsAsync(int id)
        {
        
            var advertisement = await _context.Advertisements.Include(a => a.Location).FirstOrDefaultAsync(a => a.Id == id);
            if (advertisement == null)
            {
                return null;
            }

            return _mapper.Map<AdvertisementDetailsDto>(advertisement);
        }

        public Task<PredictedPriceDto> PredictPriceAsync(PredictPriceInputDto input)
        {
            // اینجا باید منطق پیش‌بینی قیمت را اضافه کنید
            // به عنوان مثال:
            long predictedPrice = 0;
            return Task.FromResult(new PredictedPriceDto { EstimatedPrice = predictedPrice });
        }

        public async Task<IEnumerable<LocationReportDto>> GetLocationReportsAsync()
        {
            var persianCalendar = new PersianCalendar();
            int currentYear = persianCalendar.GetYear(DateTime.Now);
            return await _context.Locations
                .Select(location => new LocationReportDto
                {
                    Id = location.Id,
                    Name = location.Name,
                    AdvertisementCount = location.Advertisements.Count,
                    MaxPrice = location.Advertisements.Max(a => (long?)a.TotalPrice),
                    MinPrice = location.Advertisements.Min(a => (long?)a.TotalPrice),
                    AvgPrice = (long)location.Advertisements.Average(a => (double?)a.TotalPrice),
                    AvgBuildYear = location.Advertisements.Average(advertisement => (int?)advertisement.BuildYear) != null ? currentYear - Math.Round(location.Advertisements.Average(advertisement => (double?)advertisement.BuildYear).Value) : (double?)null,
                    AvgArea = location.Advertisements.Average(advertisement => (int?)advertisement.Area) != null ? Math.Round(location.Advertisements.Average(advertisement => (double?)advertisement.Area).Value) : (double?)null,
                })
                .ToListAsync();
        }

        public async Task<int> GetTotalAdvertisementsAsync()
        {
            return await _context.Advertisements.CountAsync();
        }
    }
}



