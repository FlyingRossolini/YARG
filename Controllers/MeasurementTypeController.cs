using GardenMVC.Common_Types;
using GardenMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace GardenMVC.Models
{
    public class MeasurementTypeController : Controller
    {
        private readonly MeasurementTypeDAL _measurementTypeDAL;

        public MeasurementTypeController(IConfiguration configuration)
        {
            _measurementTypeDAL = new(configuration);
        }

        // GET: MeasurementTypeController
        public ActionResult Index()
        {
            return View(_measurementTypeDAL.GetMeasurementTypes());
        }

        // GET: MeasurementTypeController/Details/5
        // GET: MeasurementTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeasurementTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name,Sorting")] MeasurementType measurementType)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                measurementType.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                measurementType.CreatedBy = user;
                measurementType.CreateDate = DateTime.Now;
                measurementType.ChangedBy = user;
                measurementType.ChangeDate = DateTime.Now;
                measurementType.IsActive = true;

                _measurementTypeDAL.AddMeasurementType(measurementType);

                return RedirectToAction(nameof(Index));
            }
            return View(measurementType);
        }

        // GET: MeasurementTypeController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(_measurementTypeDAL.GetMeasurementTypeByID(id.Value));
        }

        // POST: MeasurementTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,Sorting,IsActive")] MeasurementType measurementType)
        {
            if (id != measurementType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                measurementType.ChangedBy = user;
                measurementType.ChangeDate = DateTime.Now;

                _measurementTypeDAL.SaveMeasurementType(measurementType);

                return RedirectToAction(nameof(Index));
            }
            return View(measurementType);
        }

        // GET: MeasurementTypeController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_measurementTypeDAL.GetMeasurementTypeByID(id.Value));
        }

        // POST: MeasurementTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _measurementTypeDAL.DeleteMeasurementType(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
