using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YARG.Data.Models.BusinessObjects;
using YARG.Data.Services;
using YARG.Models;

namespace YARG.Data.Models.ViewModels
{ 
    [ViewComponent(Name = "YARGHeader")]
   
    public class YARGHeaderViewComponent : ViewComponent
    {
        private readonly RemoteHostDBService _remoteHostDBService;

        public YARGHeaderViewComponent(RemoteHostDBService remoteHostDBService)
        {
            _remoteHostDBService = remoteHostDBService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CurrentIrrigationCalcs currentIrrigationCalcs = await _remoteHostDBService.GetCurrentGrowInfo();

            PageHeader pageHeader = new();
            pageHeader.GrowName = currentIrrigationCalcs.GrowName;
            pageHeader.GrowWeek = currentIrrigationCalcs.GrowWeek;
            pageHeader.GrowDay = currentIrrigationCalcs.GrowDay;

            return View("YARGHeader", pageHeader);
        }
    }
}
