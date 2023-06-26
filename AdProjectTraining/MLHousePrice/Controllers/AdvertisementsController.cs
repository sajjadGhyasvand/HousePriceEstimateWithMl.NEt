using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ML;
using MLHousePrice.Models.Extensions;
using MLHousePrice.Models.ML;
using MLHousePrice.Models.Services;
using MLHousePrice.Models.Services.DTOs;
using MLHousePrice.Models.ViewModels;

namespace MLHousePrice.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementsController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            int pageSize = 20; // 5 rows with 4 ads per row

            var paginatedAdvertisements = await _advertisementService.GetAdvertisementsAsync(pageIndex, pageSize);
            var totalPages = (int)Math.Ceiling(paginatedAdvertisements.TotalCount / (double)pageSize);

            if (Request.IsAjaxRequest())
            {

                return PartialView("_AdvertisementsPartial", paginatedAdvertisements.Advertisements);
            }
            else
            {

                var viewModel = new AdvertisementsReportViewModel
                {
                    PaginatedAdvertisements = paginatedAdvertisements,
                    PageIndex = pageIndex,
                    TotalPages = totalPages
                };

                return View(viewModel);
            }
        }


        public async Task<IActionResult> Detail(int id)
        {
            var advertisement = await _advertisementService.GetAdvertisementDetailsAsync(id);

            return View(advertisement);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAdvertisementDto createAdvertisementDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createAdvertisementDto);
            }

            var advertisement = await _advertisementService.CreateAdvertisementAsync(createAdvertisementDto);

            return RedirectToAction(nameof(Detail), new { id = advertisement.Id });
        }

        public async Task<IActionResult> AdvertisementReport()
        {
            var locationReports = await _advertisementService.GetLocationReportsAsync();
            var totalAdvertisements = await _advertisementService.GetTotalAdvertisementsAsync();

            var viewModel = new AdvertisementReportViewModel
            {
                LocationReports = locationReports,
                TotalAdvertisements = totalAdvertisements
            };

            return View(viewModel);
        }


        public async Task<IActionResult> Prediction()
        {
            ViewBag.Locations = new List<SelectListItem>
            {
        new SelectListItem { Value = "منطقه ۱", Text = "منطقه ۱" },
        new SelectListItem { Value = "منطقه ۲", Text = "منطقه ۲" },
        new SelectListItem { Value = "منطقه ۳", Text = "منطقه ۳" },
        new SelectListItem { Value = "منطقه ۴", Text = "منطقه ۴" },
        new SelectListItem { Value = "منطقه ۵", Text = "منطقه ۵" },
        new SelectListItem { Value = "منطقه ۶", Text = "منطقه ۶" }
            };
            return View();
        }

          [HttpPost]
          public async Task<IActionResult> Prediction(PredictionViewModel model)
          {
              if (ModelState.IsValid)
              {
                  // Use Project Of Predict

                  MLContext mlContext = new MLContext();
                  DataViewSchema modelSchema;
                  ITransformer trainedModel =
                      mlContext.Model.Load("C:\\Users\\S.Ghiasvand\\source\\repos\\HousePriceEstimateWithMl.NEt_2\\AdProjectTraining\\MLHousePrice\\model.zip"
                      , out modelSchema);

                  PredictionEngineWrapper predictionEngine = new PredictionEngineWrapper(trainedModel, mlContext);

                  var prediction = predictionEngine.Predict(new MLInputData
                  {
                      Area= model.Area,
                      BuildYear= model.BuildYear,
                      Rooms= model.Rooms,
                      Floor= model.Floor,
                      Elevator= model.Elevator,
                      LocationName= model.LocationName,
                      Parking= model.Parking,
                      Storage= model.Storage,
                      TotalPrice = model.TotalPrice,
                  });

                  // Predicted Price
                  string predictedPrice = prediction.Price.ToString("n0");

                  // Return as JSON
                  return Json(new { Price = predictedPrice });
              }

              return BadRequest();
          }
    }
}
