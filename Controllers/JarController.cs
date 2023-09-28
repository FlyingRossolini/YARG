using YARG.Common_Types;
using YARG.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace YARG.Models
{
    [Authorize]
    public class JarController : Controller
    {
        private readonly JarDAL _jarDAL;
        private readonly ChemicalDAL _chemicalDAL;
        private readonly MixingFanScheduleDAL _mixingFanScheduleDAL;

        public JarController(IConfiguration configuration)
        {
            _jarDAL = new(configuration);
            _chemicalDAL = new(configuration);
            _mixingFanScheduleDAL = new(configuration);
        }

        // GET: JarController
        public async Task<ActionResult> Index()
        {
            return View(await _jarDAL.GetJarsAsync());
        }

        // GET: JarController/Details/5
        // GET: JarController/Create
        public async Task<ActionResult> Create()
        {
            List<SelectListItem> ddChemical = new();

            var chemList = await _chemicalDAL.GetChemicalsAsync();

            foreach (var chemical in chemList)
            {
                if (chemical.IsActive)
                {
                    ddChemical.Add(new SelectListItem { Value = chemical.ID.ToString(), Text = chemical.Name });
                }
            }

            ViewBag.ddChemical = ddChemical;

            return View();
        }

        // POST: JarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ID,MixFanPosition,ChemicalID,MixTimesPerDay," +
            "Duration,Capacity,CurrentAmount,MixFanOverSpeed")] Jar jar)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                jar.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                jar.CreatedBy = user;
                jar.CreateDate = DateTime.Now;
                jar.ChangedBy = user;
                jar.ChangeDate = DateTime.Now;
                jar.IsActive = true;

                await _jarDAL.AddJarAsync(jar);

                await _mixingFanScheduleDAL.RebuildMixingFanScheduleAsync(jar);

                return RedirectToAction(nameof(Index));
            }

            List<SelectListItem> ddChemical = new();
            var chemList = await _chemicalDAL.GetChemicalsAsync();

            foreach (var chemical in chemList)
            {
                if (chemical.IsActive)
                {
                    ddChemical.Add(new SelectListItem { Value = chemical.ID.ToString(), Text = chemical.Name });
                }
            }

            ViewBag.ddChemical = ddChemical;

            return View(jar);
        }

        // GET: JarController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> ddChemical = new();
            var chemList = await _chemicalDAL.GetChemicalsAsync();

            foreach (var chemical in chemList)
            {
                if (chemical.IsActive)
                {
                    ddChemical.Add(new SelectListItem { Value = chemical.ID.ToString(), Text = chemical.Name });
                }
            }

            ViewBag.ddChemical = ddChemical;

            return View(await _jarDAL.GetJarByIDAsync(id.Value));
        }

        // POST: JarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,MixFanPosition,ChemicalID,MixTimesPerDay," +
            "Duration,Capacity,CurrentAmount,MixFanOverSpeed,IsActive")] Jar jar)
        {
            if (id != jar.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                jar.ChangedBy = user;
                jar.ChangeDate = DateTime.Now;

                if (await _jarDAL.SaveJarAsync(jar) == true)
                {
                    await _mixingFanScheduleDAL.RebuildMixingFanScheduleAsync(jar);
                };

                return RedirectToAction(nameof(Index));
            }

            List<SelectListItem> ddChemical = new();
            var chemList = await _chemicalDAL.GetChemicalsAsync();

            foreach (var chemical in chemList)
            {
                if (chemical.IsActive)
                {
                    ddChemical.Add(new SelectListItem { Value = chemical.ID.ToString(), Text = chemical.Name });
                }
            }

            ViewBag.ddChemical = ddChemical;

            return View(jar);
        }

        // GET: JarController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _jarDAL.GetJarByIDAsync(id.Value));
        }

        // POST: JarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _jarDAL.DeleteJarAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
