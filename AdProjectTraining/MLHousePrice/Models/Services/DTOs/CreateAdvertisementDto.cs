using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MLHousePrice.Models.Services.DTOs
{
 
        public class CreateAdvertisementDto
        {
            [DisplayName("متراژ آپارتمان")]
            [Required(ErrorMessage = "متراژ آپارتمان را وارد کنید.")]
            [Range(1, 400, ErrorMessage = "متراژ باید بین 1 تا 400 باشد.")]
            public int Area { get; set; }

            [DisplayName("سال ساخت آپارتمان")]
            [Required(ErrorMessage = "سال ساخت آپارتمان را وارد کنید.")]
            [Range(1, 1405, ErrorMessage = "سال ساخت باید بین 1 تا 1405 باشد.")]
            public int BuildYear { get; set; }
            [DisplayName("تعداد اتاق‌ها")]
            [Required(ErrorMessage = "تعداد اتاق‌ها را وارد کنید.")]
            [Range(1, 5, ErrorMessage = "تعداد اتاق‌ها باید بین 1 تا 5 باشد.")]
            public int Rooms { get; set; }

            [DisplayName("قیمت کل آپارتمان به تومان")]
            [Required(ErrorMessage = "قیمت کل آپارتمان را وارد کنید.")]
            public long TotalPrice { get; set; }

            [DisplayName("طبقه‌ای که آپارتمان در آن قرار دارد")]
            [Required(ErrorMessage = "طبقه را وارد کنید.")]
            [Range(1, 30, ErrorMessage = "طبقه باید بین 1 تا 30 باشد.")]
            public int Floor { get; set; }

            [DisplayName("وجود یا عدم وجود آسانسور")]
            [Required(ErrorMessage = "وجود یا عدم وجود آسانسور را مشخص کنید.")]
            public bool Elevator { get; set; }
            [DisplayName("وجود یا عدم وجود پارکینگ")]
            [Required(ErrorMessage = "وجود یا عدم وجود پارکینگ را مشخص کنید.")]
            public bool Parking { get; set; }
            [DisplayName("وجود یا عدم وجود انباری")]
            [Required(ErrorMessage = "وجود یا عدم وجود انباری را مشخص کنید.")]
            public bool Storage { get; set; }
            [DisplayName("توضیحات مربوط به آگهی ")]
            [Required(ErrorMessage = "توضیحات مربوط به آگهی را وارد کنید.")]
            public string Description { get; set; }

            [DisplayName("عنوان آگهی")]
            [Required(ErrorMessage = "عنوان آگهی را وارد کنید.")]
            public string Title { get; set; }
            [DisplayName("شناسه محل")]
            [Required(ErrorMessage = "شناسه محل را وارد کنید.")]
            public int LocationId { get; set; }
        }

        public class AdvertisementListDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string LocationName { get; set; }
            public long TotalPrice { get; set; }
        }

        public class PaginatedAdvertisementsDto
        {
            public IEnumerable<AdvertisementListDto> Advertisements { get; set; }
            public int TotalCount { get; set; }
        }

        public class AdvertisementDetailsDto : CreateAdvertisementDto
        {
            public int Id { get; set; }
            public long TotalPrice { get; set; }
        }

        public class PredictPriceInputDto : CreateAdvertisementDto
        {
        }

        public class PredictedPriceDto
        {
            public long EstimatedPrice { get; set; }
        }
        public class LocationReportDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AdvertisementCount { get; set; }
            public long? MaxPrice { get; set; }
            public long? MinPrice { get; set; }
            public long? AvgPrice { get; set; }
            public double? AvgBuildYear { get; set; }
            public double? AvgArea { get; set; }
        }
    }

