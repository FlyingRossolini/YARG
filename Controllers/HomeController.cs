﻿using YARG.DAL;
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
            homeViewModel.wateringSchedules = await _wateringScheduleDAL.GetWateringSchedulesAsync();
            homeViewModel.pots = await _potDAL.GetActivePotsAsync();
            homeViewModel.potCount = await _potDAL.PotCountAsync();

            homeViewModel.sunrise = (await _growSeasonDAL.GetGrowSeasonByIDAsync(await _growSeasonDAL.IDActiveGrowSeasonAsync())).SunriseTime;
            homeViewModel.sunset = (await _growSeasonDAL.GetGrowSeasonByIDAsync(await _growSeasonDAL.IDActiveGrowSeasonAsync())).SunsetTime;

            homeViewModel.flgMorningDrink = (await _growSeasonDAL.GetGrowSeasonByIDAsync(await _growSeasonDAL.IDActiveGrowSeasonAsync())).FlgAddMorningSplash;
            homeViewModel.flgEveningDrink = (await _growSeasonDAL.GetGrowSeasonByIDAsync(await _growSeasonDAL.IDActiveGrowSeasonAsync())).FlgAddEveningSplash;

            homeViewModel.growName = (await _growSeasonDAL.GetGrowSeasonByIDAsync(await _growSeasonDAL.IDActiveGrowSeasonAsync())).Name;

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

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

            return LocalRedirect(returnUrl);

        }
    }
}
