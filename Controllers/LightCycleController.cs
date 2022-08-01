using GardenMVC.Common_Types;
using GardenMVC.DAL;
using GardenMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GardenMVC.Controllers
{
    public class LightCycleController : Controller
    {
        private readonly LightCycleDAL _lightCycleDAL;

        public LightCycleController()
        {
            _lightCycleDAL = new();
        }

        // GET: LightCycleController
        public ActionResult Index()
        {
            return View(_lightCycleDAL.GetLightCycles());
        }

        // GET: LightCycleController/Details/5
        // GET: LightCycleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LightCycleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name,DaylightHours")] LightCycle lightCycle)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                lightCycle.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                lightCycle.CreatedBy = user;
                lightCycle.CreateDate = DateTime.Now;
                lightCycle.ChangedBy = user;
                lightCycle.ChangeDate = DateTime.Now;
                lightCycle.IsActive = true;

                _lightCycleDAL.AddLightCycle(lightCycle);

                return RedirectToAction(nameof(Index));
            }
            return View(lightCycle);
        }

        // GET: LightCycleController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_lightCycleDAL.GetLightCycleByID(id.Value));
        }

        // POST: LightCycleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,,DaylightHours,IsActive")]LightCycle lightCycle)
        {
            if (id != lightCycle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                lightCycle.ChangedBy = user;
                lightCycle.ChangeDate = DateTime.Now;

                _lightCycleDAL.SaveLightCycle(lightCycle);

                return RedirectToAction(nameof(Index));
            }
            return View(lightCycle);
        }

        // GET: LightCycleController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_lightCycleDAL.GetLightCycleByID(id.Value));
        }

        // POST: LightCycleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _lightCycleDAL.DeleteLightCycle(id);

            return RedirectToAction(nameof(Index));
        }
    
        [HttpGet]
        public JsonResult GetLightCycleList()
        {
            List<LightCycle> lightCycles = new();

            foreach(LightCycle lightCycle in _lightCycleDAL.GetLightCycles())
            {
                lightCycles.Add(lightCycle);
            }

            return Json(lightCycles);
        }
    }
}
