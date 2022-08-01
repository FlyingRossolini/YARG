using GardenMVC.Data.Models.ViewModels;
using GardenMVC.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenMVC.Controllers
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
        public IActionResult AddRemoteMeasurement([FromBody]RemoteMeasurementViewModel measurement)
        {
            _remoteMeasurementService.AddRemoteMeasurement(measurement);
            return Ok();
        }
    }
}
