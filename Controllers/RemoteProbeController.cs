using YARG.Common_Types;
using YARG.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace YARG.Models
{
    [Authorize]
    public class RemoteProbeController : Controller
    {
        private readonly RemoteProbeDAL _remoteProbeDAL;
        private readonly LocationDAL _locationDAL;
        private readonly MeasurementTypeDAL _measurementTypeDAL;

        public RemoteProbeController(IConfiguration configuration)
        {
            _remoteProbeDAL = new(configuration);
            _locationDAL = new(configuration);
            _measurementTypeDAL = new(configuration);
        }

        // GET: RemoteProbeController
        public async Task<ActionResult> Index()
        {
            return View(await _remoteProbeDAL.GetRemoteProbesAsync());
        }

        // GET: RemoteProbeController/Details/5
        // GET: RemoteProbeController/Create
        public async Task<ActionResult> Create()
        {
            List<SelectListItem> ddLocation = new();

            var locationList = await _locationDAL.GetLocationsAsync();

            foreach (var location in locationList)
            {
                if(location.IsActive)
                {
                    ddLocation.Add(new SelectListItem { Value = location.ID.ToString(), Text = location.Name });
                }
            }

            ViewBag.ddLocation = ddLocation;

            List<SelectListItem> ddMeasurementType = new();

            var measurementTypeList = await _measurementTypeDAL.GetMeasurementTypesAsync();

            foreach (var measurementType in measurementTypeList)
            {
                ddMeasurementType.Add(new SelectListItem { Value = measurementType.ID.ToString(), Text = measurementType.Name });
            }

            ViewBag.ddMeasurementType = ddMeasurementType;

            return View();
        }

        // POST: RemoteProbeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ID,RemoteProbeAddress,LocationID,MeasurementTypeID")] RemoteProbe remoteProbe)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                remoteProbe.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                remoteProbe.CreatedBy = user;
                remoteProbe.CreateDate = DateTime.Now;
                remoteProbe.ChangedBy = user;
                remoteProbe.ChangeDate = DateTime.Now;
                remoteProbe.IsActive = true;

                await _remoteProbeDAL.AddRemoteProbeAsync(remoteProbe);

                return RedirectToAction(nameof(Index));
            }

            return View(remoteProbe);
        }

        // GET: RemoteProbeController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> ddLocation = new();
            var locationList = await _locationDAL.GetLocationsAsync();

            foreach (var location in locationList)
            {
                ddLocation.Add(new SelectListItem { Value = location.ID.ToString(), Text = location.Name });
            }

            ViewBag.ddLocation = ddLocation;

            List<SelectListItem> ddMeasurementType = new();
            var measurementTypeList = await _measurementTypeDAL.GetMeasurementTypesAsync();

            foreach (var measurementType in measurementTypeList)
            {
                if(measurementType.IsActive)
                {
                    ddMeasurementType.Add(new SelectListItem { Value = measurementType.ID.ToString(), Text = measurementType.Name });
                }
            }

            ViewBag.ddMeasurementType = ddMeasurementType;

            return View(await _remoteProbeDAL.GetRemoteProbeByIDAsync(id.Value));
        }

        // POST: RemoteProbeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,RemoteProbeAddress,LocationID,MeasurementTypeID,IsActive")] RemoteProbe remoteProbe)
        {
            if (id != remoteProbe.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                remoteProbe.ChangedBy = user;
                remoteProbe.ChangeDate = DateTime.Now;

                await _remoteProbeDAL.SaveRemoteProbeAsync(remoteProbe);

                return RedirectToAction(nameof(Index));
            }
            return View(remoteProbe);
        }

        // GET: RemoteProbeController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _remoteProbeDAL.GetRemoteProbeByIDAsync(id.Value));
        }

        // POST: RemoteProbeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _remoteProbeDAL.DeleteRemoteProbeAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
