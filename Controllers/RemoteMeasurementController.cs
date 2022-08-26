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
    public class RemoteMeasurementController : ControllerBase
    {
        public RemoteMeasurementService _remoteMeasurementService;
        public RemoteMeasurementController(RemoteMeasurementService remoteMeasurementService)
        {
            _remoteMeasurementService = remoteMeasurementService;
        }

        [HttpPost("add-measurement")]
        public async Task<IActionResult> AddRemoteMeasurement([FromBody]RemoteMeasurementViewModel measurement)
        {
            await _remoteMeasurementService.AddRemoteMeasurement(measurement);
            return Ok();
        }
    }
}
