using YARG.DAL;
using YARG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace YARG.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHtmlLocalizer _localizer;
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly PotDAL _potDAL;
        private readonly GrowSeasonDAL _growSeasonDAL;

        public HomeController(IConfiguration configuration, IHtmlLocalizer<HomeController> localizer)
        {
            _localizer = localizer;

            _wateringScheduleDAL = new(configuration);
            _potDAL = new(configuration);
            _growSeasonDAL = new(configuration);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new();
            homeViewModel.WateringSchedules = await _wateringScheduleDAL.GetWateringSchedulesAsync();
            homeViewModel.Pots = await _potDAL.GetActivePotsAsync();
            homeViewModel.PotCount = await _potDAL.PotCountAsync();

            CurrentIrrigationCalcs currentIrrigationCalcs = await _growSeasonDAL.GetCurrentIrrigationCalcs();

            homeViewModel.Sunrise = currentIrrigationCalcs.Sunrise;
            homeViewModel.Sunset = currentIrrigationCalcs.Sunset;
            homeViewModel.FlgMorningDrink = currentIrrigationCalcs.IsMorningSip;
            homeViewModel.FlgEveningDrink = currentIrrigationCalcs.IsEveningSip;

            homeViewModel.GrowName = currentIrrigationCalcs.GrowName;
            homeViewModel.RecipeName = currentIrrigationCalcs.RecipeName;
            homeViewModel.CropName = currentIrrigationCalcs.CropName;
            homeViewModel.Day = currentIrrigationCalcs.GrowDay;
            homeViewModel.Week = currentIrrigationCalcs.GrowWeek;

            homeViewModel.GrowRoomTemp = Math.Round(currentIrrigationCalcs.GrowRoomTemp, 0);
            homeViewModel.GrowRoomHumidity = Math.Round(currentIrrigationCalcs.GrowRoomHumidity, 0);
            homeViewModel.ReservoirTemp = Math.Round(currentIrrigationCalcs.ReservoirTemp, 0);
            homeViewModel.ReservoirVolume = Math.Round(currentIrrigationCalcs.ReservoirVolume, 1);
            homeViewModel.StartDate = currentIrrigationCalcs.GrowStartDate;

            homeViewModel.DaylightHours = currentIrrigationCalcs.DaylightHoursPerDay;
            homeViewModel.FertEvents = currentIrrigationCalcs.IrrigationEventsPerDay;
            homeViewModel.MinsPerFert = currentIrrigationCalcs.SoakTime;
            homeViewModel.WeatherText = currentIrrigationCalcs.WeatherText;

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

            return LocalRedirect(returnUrl);

        }
    }
}
