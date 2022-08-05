using GardenMVC.Common_Types;
using GardenMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace GardenMVC.Models
{
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
        public ActionResult Index()
        {
            return View(_jarDAL.GetJars());
        }

        // GET: JarController/Details/5
        // GET: JarController/Create
        public ActionResult Create()
        {
            List<SelectListItem> ddChemical = new();
            var chemicalList = (from chemical in _chemicalDAL.GetChemicals()
                            where chemical.IsActive == true                
                            select new { chemical.ID, chemical.Name }).ToList();

            foreach (var item in chemicalList)
            {
                ddChemical.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddChemical = ddChemical;

            return View();
        }

        // POST: JarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,MixFanPosition,ChemicalID,MixTimesPerDay," +
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

                _jarDAL.AddJar(jar);

                _mixingFanScheduleDAL.RebuildMixingFanSchedule(jar);

                return RedirectToAction(nameof(Index));
            }
            List<SelectListItem> ddChemical = new();
            var chemicalList = (from chemical in _chemicalDAL.GetChemicals()
                                where chemical.IsActive == true
                                orderby chemical.Name
                                select new { chemical.ID, chemical.Name }).ToList();

            foreach (var item in chemicalList)
            {
                ddChemical.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddChemical = ddChemical;

            return View(jar);
        }

        // GET: JarController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> ddChemical = new();
            var chemicalList = (from chemical in _chemicalDAL.GetChemicals()
                                where chemical.IsActive == true
                                orderby chemical.Name
                                select new { chemical.ID, chemical.Name }).ToList();

            foreach (var item in chemicalList)
            {
                ddChemical.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddChemical = ddChemical;

            return View(_jarDAL.GetJarByID(id.Value));
        }

        // POST: JarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,MixFanPosition,ChemicalID,MixTimesPerDay," +
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

                if (_jarDAL.SaveJar(jar) == true)
                {
                    _mixingFanScheduleDAL.RebuildMixingFanSchedule(jar);
                };

                return RedirectToAction(nameof(Index));
            }

            List<SelectListItem> ddChemical = new();
            var chemicalList = (from chemical in _chemicalDAL.GetChemicals()
                                where chemical.IsActive == true
                                orderby chemical.Name
                                select new { chemical.ID, chemical.Name }).ToList();

            foreach (var item in chemicalList)
            {
                ddChemical.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddChemical = ddChemical;

            return View(jar);
        }

        // GET: JarController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_jarDAL.GetJarByID(id.Value));
        }

        // POST: JarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _jarDAL.DeleteJar(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
