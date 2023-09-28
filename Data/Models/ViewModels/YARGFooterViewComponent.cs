using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YARG.DAL;
using YARG.Data.Services;
using YARG.Data.Models.BusinessObjects;

namespace YARG.Data.Models.ViewModels
{
    [ViewComponent(Name = "YARGFooter")]
    public class YARGFooterViewComponent : ViewComponent
    {
        private readonly RemoteHostDBService _remoteHostDBService;

        public YARGFooterViewComponent(RemoteHostDBService remoteHostDBService)
        {
            _remoteHostDBService = remoteHostDBService;
        }
        public async Task <IViewComponentResult> InvokeAsync()
        {
            Footer footer = new();
            footer.Status = await _remoteHostDBService.GetAppStatus();

            return View("YARGFooter", footer);
        }
    }
}
