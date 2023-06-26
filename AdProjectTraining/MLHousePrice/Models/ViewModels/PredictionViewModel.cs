using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MLHousePrice.Models.ViewModels
{
    public class PredictionViewModel
    {
        [Required(ErrorMessage = " مساحت الزامیست.")]
        [Range(35, 310, ErrorMessage = "مساحت باید بین 35 و 310 متر مربع باشد.")]
        [DisplayName("مساحت")]
        public int Area { get; set; }
        [Required(ErrorMessage = " مساحت الزامیست.")]
        [DisplayName("قیمت کل")]
        public int TotalPrice { get; set; }

        [Required(ErrorMessage = " سال ساخت الزامیست.")]
        [Range(1370, 1410, ErrorMessage = "سال ساخت باید بین 1370 و 1410 باشد.")]
        [DisplayName("سال ساخت")]
        public int BuildYear { get; set; }

        [Required(ErrorMessage = " تعداد اتاق‌ها الزامیست.")]
        [Range(1, 4, ErrorMessage = "تعداد اتاق‌ها باید بین 1 و 4 باشد.")]
        [DisplayName("تعداد اتاق‌ها")]
        public int Rooms { get; set; }

        [Required(ErrorMessage = " طبقه الزامیست.")]
        [Range(1, 30, ErrorMessage = "طبقه باید بین 1 و 30 باشد.")]
        [DisplayName("طبقه")]
        public int Floor { get; set; }

        [DisplayName("آسانسور")]
        public bool Elevator { get; set; }

        [DisplayName("پارکینگ")]
        public bool Parking { get; set; }

        [DisplayName("انباری")]
        public bool Storage { get; set; }

        [Required(ErrorMessage = " محله الزامیست.")]
        [DisplayName("محله")]
        public string LocationName { get; set; }
    }

}
