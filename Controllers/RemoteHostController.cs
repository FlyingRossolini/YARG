using YARG.Data.Models.ViewModels;
using YARG.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteHostController : ControllerBase
    {
        public RemoteHostService _remoteHostService;
        public RemoteHostController(RemoteHostService remoteHostService)
        {
            _remoteHostService = remoteHostService;
        }

        [HttpPost("acknowledge-mf-command")]
        public IActionResult AcknowledgeMixingFanSchedule([FromBody] RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            _remoteHostService.AcknowledgeMixingFanSchedule(remoteHostCommandViewModel);
            return Ok();
        }
        
        [HttpPost("complete-mf-command")]
        public IActionResult CompleteMixingFanSchedule([FromBody] RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            _remoteHostService.CompleteMixingFanSchedule(remoteHostCommandViewModel);
            return Ok();
        }

        [HttpPost("error-mf-command")]
        public IActionResult ErrorMixingFanSchedule([FromBody] RemoteHostErrorViewModel remoteHostErrorViewModel)
        {
            _remoteHostService.ErrorMixingFanSchedule(remoteHostErrorViewModel);
            return Ok();
        }

        [HttpPost("acknowledge-ws-command")]
        public IActionResult AcknowledgeWateringSchedule([FromBody] RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            _remoteHostService.AcknowledgeWateringSchedule(remoteHostCommandViewModel);
            return Ok();
        }

        [HttpPost("complete-ws-command")]
        public IActionResult CompleteWateringSchedule([FromBody] RemoteHostCommandViewModel remoteHostViewModel)
        {
            _remoteHostService.CompleteWateringSchedule(remoteHostViewModel);
            return Ok();
        }

        [HttpPost("error-ws-command")]
        public IActionResult ErrorWateringSchedule([FromBody] RemoteHostErrorViewModel remoteHostErrorViewModel)
        {
            _remoteHostService.ErrorWateringSchedule(remoteHostErrorViewModel);
            return Ok();
        }
    }
}
