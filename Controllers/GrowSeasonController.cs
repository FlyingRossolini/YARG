using YARG.Common_Types;
using YARG.DAL;
using YARG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace YARG.Controllers
{
    [Authorize]
    public class GrowSeasonController : Controller
    {
        private readonly GrowSeasonDAL _growSeasonDAL;
        private readonly CropDAL _cropDAL;
        private readonly WateringScheduleDAL _wateringScheduleDAL;

        public GrowSeasonController(IConfiguration configuration)
        {
            _growSeasonDAL = new(configuration);
            _cropDAL = new(configuration);
            _wateringScheduleDAL = new(configuration);
        }
        // GET: GrowSeasonController
        public async Task<ActionResult> Index()
        {
            return View(await _growSeasonDAL.GetGrowSeasonsAsync());
        }

        // GET: GrowSeasonController/Details/5
        // GET: GrowSeasonController/Create
        public async Task <ActionResult> Create()
        {
            List<SelectListItem> ddCrop = new();
            var cropList = await _cropDAL.GetCropsAsync();

            foreach (var crop in cropList)
            {
                if (crop.IsActive)
                {
                    ddCrop.Add(new SelectListItem { Value = crop.ID.ToString(), Text = crop.Name });
                }
            }

            ViewBag.ddCrop = ddCrop;

            return View();
        }

        // POST: GrowSeasonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ID,Name,StartDate,EndDate,SunriseTime,CropID,FlgAddMorningSplash,FlgAddEveningSplash,EFEventsPerDay")] GrowSeason growSeason)
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
                //growSeason.IsActive = !await _growSeasonDAL.FlgActiveGrowSeasonAsync();
                growSeason.IsActive = false;

                await _growSeasonDAL.AddGrowSeasonAsync(growSeason);

                return RedirectToAction(nameof(Index));
            }
            return View(growSeason);
        }

        // GET: GrowSeasonController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> ddCrop = new();
            var cropList = await _cropDAL.GetCropsAsync();

            foreach (var crop in cropList)
            {
                if (crop.IsActive)
                {
                    ddCrop.Add(new SelectListItem { Value = crop.ID.ToString(), Text = crop.Name });
                }
            }

            ViewBag.ddCrop = ddCrop;

            ViewBag.flgCanEditIsActive = await _growSeasonDAL.IDActiveGrowSeasonAsync() ==  id.Value  ||
                (await _growSeasonDAL.IDActiveGrowSeasonAsync() != id.Value && !await _growSeasonDAL.FlgActiveGrowSeasonAsync()); // & id of active grow season != whoever is active

            return View(await _growSeasonDAL.GetGrowSeasonByIDAsync(id.Value));
        }

        // POST: GrowSeasonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,Name,StartDate,EndDate,SunriseTime,CropID,FlgAddMorningSplash,FlgAddEveningSplash,EFEventsPerDay,IsComplete,IsActive")] GrowSeason growSeason)
        {
            if (id != growSeason.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool flgRebuildWaterSchedule = false;

                GrowSeason gs = await _growSeasonDAL.GetGrowSeasonByIDAsync(id);

                if(gs.StartDate != growSeason.StartDate ||
                    gs.SunriseTime != growSeason.SunriseTime ||
                    gs.IsComplete != growSeason.IsComplete ||
                    gs.IsActive != growSeason.IsActive)
                {
                    flgRebuildWaterSchedule = true;
                }
                    
                string user = Environment.UserName;

                growSeason.ChangedBy = user;
                growSeason.ChangeDate = DateTime.Now;

                await _growSeasonDAL.SaveGrowSeasonAsync(growSeason);

                if(flgRebuildWaterSchedule)
                {
                    await _wateringScheduleDAL.RebuildWateringSchedule();
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
