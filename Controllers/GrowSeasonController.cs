using GardenMVC.Common_Types;
using GardenMVC.DAL;
using GardenMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GardenMVC.Controllers
{
    public class GrowSeasonController : Controller
    {
        private readonly GrowSeasonDAL _growSeasonDAL;
        private readonly CropDAL _cropDAL;
        private readonly LightCycleDAL _lightCycleDAL;
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        public GrowSeasonController()
        {
            _growSeasonDAL = new();
            _cropDAL = new();
            _wateringScheduleDAL = new();
        }
        // GET: GrowSeasonController
        public ActionResult Index()
        {
            return View(_growSeasonDAL.GetGrowSeasons());
        }

        // GET: GrowSeasonController/Details/5
        // GET: GrowSeasonController/Create
        public ActionResult Create()
        {
            List<SelectListItem> ddCrop = new();
            var cropList = (from crop in _cropDAL.GetCrops()
                            where crop.IsActive == true
                            orderby crop.Name
                            select new { crop.ID, crop.Name }).ToList();
            
            foreach (var item in cropList)
            {
                ddCrop.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddCrop = ddCrop;

            return View();
        }

        // POST: GrowSeasonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name,StartDate,EndDate,SunriseTime,CropID,FlgAddMorningSplash,FlgAddEveningSplash,EFEventsPerDay")] GrowSeason growSeason)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                growSeason.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                growSeason.CreatedBy = user;
                growSeason.CreateDate = DateTime.Now;
                growSeason.ChangedBy = user;
                growSeason.ChangeDate = DateTime.Now;
                growSeason.IsComplete = false;
                growSeason.IsActive = !_growSeasonDAL.FlgActiveGrowSeason();

                _growSeasonDAL.AddGrowSeason(growSeason);

                return RedirectToAction(nameof(Index));
            }
            return View(growSeason);
        }

        // GET: GrowSeasonController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> ddCrop = new();
            var cropList = (from crop in _cropDAL.GetCrops()
                            where crop.IsActive == true
                            orderby crop.Name
                            select new { crop.ID, crop.Name }).ToList();

            foreach (var item in cropList)
            {
                ddCrop.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddCrop = ddCrop;

            ViewBag.flgCanEditIsActive = _growSeasonDAL.IDActiveGrowSeason() ==  id.Value  ||
                (_growSeasonDAL.IDActiveGrowSeason() != id.Value && !_growSeasonDAL.FlgActiveGrowSeason()); // & id of active grow season != whoever is active

            return View(_growSeasonDAL.GetGrowSeasonByID(id.Value));
        }

        // POST: GrowSeasonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,StartDate,EndDate,SunriseTime,CropID,FlgAddMorningSplash,FlgAddEveningSplash,EFEventsPerDay,IsComplete,IsActive")] GrowSeason growSeason)
        {
            if (id != growSeason.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool flgRebuildWaterSchedule = false;

                GrowSeason gs = _growSeasonDAL.GetGrowSeasonByID(id);

                if(gs.StartDate != growSeason.StartDate ||
                    gs.SunriseTime != growSeason.SunriseTime ||
                    gs.FlgAddEveningSplash != growSeason.FlgAddEveningSplash ||
                    gs.FlgAddMorningSplash != growSeason.FlgAddMorningSplash ||
                    gs.EFEventsPerDay != growSeason.EFEventsPerDay ||
                    gs.IsComplete != growSeason.IsComplete ||
                    gs.IsActive != growSeason.IsActive)
                {
                    flgRebuildWaterSchedule = true;
                }
                    
                string user = Environment.UserName;

                growSeason.ChangedBy = user;
                growSeason.ChangeDate = DateTime.Now;

                _growSeasonDAL.SaveGrowSeason(growSeason);

                if(flgRebuildWaterSchedule)
                {
                    _wateringScheduleDAL.RebuildWateringSchedule();
                }

                return RedirectToAction(nameof(Index), "Home");
            }
            return View(growSeason);
        }

        // GET: GrowSeasonController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GrowSeason growSeason = new();
            return View(growSeason);
        }

        // POST: GrowSeasonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            //using (MySqlConnection sqlConnection = new MySqlConnection(_configuration.GetConnectionString("GardenConnection")))
            //{
            //    MySqlCommand sqlCmd = new MySqlCommand("spDeleteGrowSeason", sqlConnection);
            //    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            //    sqlCmd.Parameters.AddWithValue("thisid", id.ToString());

            //    sqlConnection.Open();
            //    sqlCmd.ExecuteNonQuery();
            //    sqlConnection.Close();
            //}

            return RedirectToAction(nameof(Index));
        }

    }
}
