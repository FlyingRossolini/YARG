using YARG.Common_Types;
using YARG.DAL;
using YARG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace YARG.Controllers
{
    [Authorize]
    public class LightCycleController : Controller
    {
        private readonly LightCycleDAL _lightCycleDAL;

        public LightCycleController(IConfiguration configuration)
        {
            _lightCycleDAL = new(configuration);
        }

        // GET: LightCycleController
        public async Task<ActionResult> Index()
        {
            return View(await _lightCycleDAL.GetLightCyclesAsync());
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
        public async Task<ActionResult> Create([Bind("ID,Name,DaylightHours")] LightCycle lightCycle)
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

                await _lightCycleDAL.AddLightCycleAsync(lightCycle);

                return RedirectToAction(nameof(Index));
            }
            return View(lightCycle);
        }

        // GET: LightCycleController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _lightCycleDAL.GetLightCycleByIDAsync(id.Value));
        }

        // POST: LightCycleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,Name,,DaylightHours,IsActive")]LightCycle lightCycle)
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

                await _lightCycleDAL.SaveLightCycleAsync(lightCycle);

                return RedirectToAction(nameof(Index));
            }
            return View(lightCycle);
        }

        // GET: LightCycleController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _lightCycleDAL.GetLightCycleByIDAsync(id.Value));
        }

        // POST: LightCycleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _lightCycleDAL.DeleteLightCycleAsync(id);

            return RedirectToAction(nameof(Index));
        }
    
        [HttpGet]
        public async Task<JsonResult> GetLightCycleList()
        {
            List<LightCycle> lightCycles = new();

            foreach(LightCycle lightCycle in await _lightCycleDAL.GetLightCyclesAsync())
            {
                lightCycles.Add(lightCycle);
            }

            return Json(lightCycles);
        }
    }
}
