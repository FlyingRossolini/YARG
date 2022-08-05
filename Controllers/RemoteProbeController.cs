using GardenMVC.Common_Types;
using GardenMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GardenMVC.Models
{
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
        public ActionResult Index()
        {
            return View(_remoteProbeDAL.GetRemoteProbes());
        }

        // GET: RemoteProbeController/Details/5
        // GET: RemoteProbeController/Create
        public ActionResult Create()
        {
            List<SelectListItem> ddLocation = new();
            var locationList = (from location in _locationDAL.GetLocations()
                            where location.IsActive == true
                            orderby location.Name
                            select new { location.ID, location.Name }).ToList();

            foreach (var item in locationList)
            {
                ddLocation.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddLocation = ddLocation;

            List<SelectListItem> ddMeasurementType = new();
            var measurementTypeList = (from measurementType in _measurementTypeDAL.GetMeasurementTypes()
                                where measurementType.IsActive == true
                                orderby measurementType.Name
                                select new { measurementType.ID, measurementType.Name }).ToList();

            foreach (var item in measurementTypeList)
            {
                ddMeasurementType.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddMeasurementType = ddMeasurementType;

            return View();
        }

        // POST: RemoteProbeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,RemoteProbeAddress,LocationID,MeasurementTypeID")] RemoteProbe remoteProbe)
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

                _remoteProbeDAL.AddRemoteProbe(remoteProbe);

                return RedirectToAction(nameof(Index));
            }

            return View(remoteProbe);
        }

        // GET: RemoteProbeController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> ddLocation = new();
            var locationList = (from location in _locationDAL.GetLocations()
                                where location.IsActive == true
                                orderby location.Name
                                select new { location.ID, location.Name }).ToList();

            foreach (var item in locationList)
            {
                ddLocation.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddLocation = ddLocation;

            List<SelectListItem> ddMeasurementType = new();
            var measurementTypeList = (from measurementType in _measurementTypeDAL.GetMeasurementTypes()
                                       where measurementType.IsActive == true
                                       orderby measurementType.Name
                                       select new { measurementType.ID, measurementType.Name }).ToList();

            foreach (var item in measurementTypeList)
            {
                ddMeasurementType.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddMeasurementType = ddMeasurementType;

            return View(_remoteProbeDAL.GetRemoteProbeByID(id.Value));
        }

        // POST: RemoteProbeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,RemoteProbeAddress,LocationID,MeasurementTypeID,IsActive")] RemoteProbe remoteProbe)
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

                _remoteProbeDAL.SaveRemoteProbe(remoteProbe);

                return RedirectToAction(nameof(Index));
            }
            return View(remoteProbe);
        }

        // GET: RemoteProbeController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_remoteProbeDAL.GetRemoteProbeByID(id.Value));
        }

        // POST: RemoteProbeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _remoteProbeDAL.DeleteRemoteProbe(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
