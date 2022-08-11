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

namespace YARG.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly PotDAL _potDAL;
        private readonly GrowSeasonDAL _growSeasonDAL;

        public HomeController(IConfiguration configuration)
        {
            _wateringScheduleDAL = new(configuration);
            _potDAL = new(configuration);
            _growSeasonDAL = new(configuration);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new();
            homeViewModel.wateringSchedules = _wateringScheduleDAL.GetWateringSchedules();
            homeViewModel.pots = _potDAL.GetActivePots();
            homeViewModel.potCount = _potDAL.PotCount();

            
            homeViewModel.sunrise = _growSeasonDAL.GetGrowSeasonByID(_growSeasonDAL.IDActiveGrowSeason()).SunriseTime;
            homeViewModel.sunset = _growSeasonDAL.GetGrowSeasonByID(_growSeasonDAL.IDActiveGrowSeason()).SunsetTime;

            homeViewModel.flgMorningDrink = _growSeasonDAL.GetGrowSeasonByID(_growSeasonDAL.IDActiveGrowSeason()).FlgAddMorningSplash;
            homeViewModel.flgEveningDrink = _growSeasonDAL.GetGrowSeasonByID(_growSeasonDAL.IDActiveGrowSeason()).FlgAddEveningSplash;

            homeViewModel.growName = _growSeasonDAL.GetGrowSeasonByID(_growSeasonDAL.IDActiveGrowSeason()).Name;

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
