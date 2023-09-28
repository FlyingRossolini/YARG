using YARG.Common_Types;
using YARG.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace YARG.Models
{
    [Authorize]
    public class MeasurementTypeController : Controller
    {
        private readonly MeasurementTypeDAL _measurementTypeDAL;

        public MeasurementTypeController(IConfiguration configuration)
        {
            _measurementTypeDAL = new(configuration);
        }

        // GET: MeasurementTypeController
        public async Task<ActionResult> Index()
        {
            return View(await _measurementTypeDAL.GetMeasurementTypesAsync());
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
        public async Task<ActionResult> Create([Bind("ID,Name,Sorting")] MeasurementType measurementType)
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

                await _measurementTypeDAL.AddMeasurementTypeAsync(measurementType);

                return RedirectToAction(nameof(Index));
            }
            return View(measurementType);
        }

        // GET: MeasurementTypeController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(await _measurementTypeDAL.GetMeasurementTypeByIDAsync(id.Value));
        }

        // POST: MeasurementTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Edit(Guid id, [Bind("ID,Name,Sorting,IsActive")] MeasurementType measurementType)
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

                await _measurementTypeDAL.SaveMeasurementTypeAsync(measurementType);

                return RedirectToAction(nameof(Index));
            }
            return View(measurementType);
        }

        // GET: MeasurementTypeController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _measurementTypeDAL.GetMeasurementTypeByIDAsync(id.Value));
        }

        // POST: MeasurementTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _measurementTypeDAL.DeleteMeasurementTypeAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
