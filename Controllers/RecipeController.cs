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
    public class RecipeController : Controller
    {

        private readonly RecipeDAL _recipeDAL;
        private readonly LightCycleDAL _lightCycleDAL;
        private readonly LocationDAL _locationDAL;
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly IConfiguration _config;

        public RecipeController(IConfiguration configuration)
        {
            _recipeDAL = new(configuration);
            _lightCycleDAL = new(configuration);
            _locationDAL = new(configuration);
            _wateringScheduleDAL = new(configuration);
            _config = configuration;
        }

        // GET: RecipeController
 //       [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            List<SelectListItem> ddDaylight = new();
            var daylightList = (from daylight in await _lightCycleDAL.GetLightCyclesAsync()
                                where daylight.IsActive == true
                                select new { daylight.ID, daylight.Name }).ToList();

            foreach (var item in daylightList)
            {
                ddDaylight.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddDaylight = ddDaylight;

            RecipeViewModel recipeViewModel = new();
            recipeViewModel.recipes = await _recipeDAL.GetRecipesAsync();
            recipeViewModel.locations = await _locationDAL.GetLocationsForRecipeAsync();
            recipeViewModel.recipeChemListViewModels = await _recipeDAL.GetRecipeChemListViewModelsAsync();

            return View(recipeViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipeName(Recipe recipe)
        {
            string user = Environment.UserName;

            recipe.ChangedBy = user;
            recipe.ChangeDate = DateTime.Now;

            await _recipeDAL.UpdateRecipeNameAsync(recipe);

            return new EmptyResult();
        }

        //[HttpGet]
        //public JsonResult GetRecipeChemListByRecipeID(Guid recipeID)
        //{
        //    List<RecipeChemList> recipeChems = new();

        //    foreach (RecipeChemList recipeChemList in _recipeDAL.GetRecipeChemListByRecipeID(recipeID))
        //    {
        //        recipeChems.Add(recipeChemList);
        //    }

        //    return Json(recipeChems);

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddWeek(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
            recipeStep.CreatedBy = user;
            recipeStep.CreateDate = DateTime.Now;
            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            RecipeStep lstRecipeStep = await _recipeDAL.GetLastRecipeStepByRecipeIDAsync(recipeStep.RecipeID);

            if(lstRecipeStep.WeekNumber != 0)
            {
                recipeStep.WeekNumber = Convert.ToInt16(lstRecipeStep.WeekNumber + 1);
                recipeStep.LightCycleID = lstRecipeStep.LightCycleID;
                recipeStep.IrrigationEventsPerDay = lstRecipeStep.IrrigationEventsPerDay;
                recipeStep.SoakTime = lstRecipeStep.SoakTime;
                recipeStep.IsMorningSip = lstRecipeStep.IsMorningSip;
                recipeStep.IsEveningSip = lstRecipeStep.IsEveningSip;
            }
            else
            {
                recipeStep.WeekNumber = 1;
                recipeStep.LightCycleID = Guid.Parse("3a02ab69-ecc3-12aa-0c5e-cd55b28ffcb4");
                recipeStep.IrrigationEventsPerDay = 1;
                recipeStep.SoakTime = 1;
                recipeStep.IsMorningSip = true;
                recipeStep.IsEveningSip = true;

            }

            await _recipeDAL.AddRecipeStepAsync(recipeStep);

            #region Build Recipe Step Limits
            List<RecipeStepLimit> recipeStepLimits = new();

            RecipeStepLimit recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_TempDay,
                LimitTypeID = Constants.LimitType_LCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID, 
                    Constants.LocationType_Habitat, Constants.MeasurementType_TempDay, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_TempDay,
                LimitTypeID = Constants.LimitType_UCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_TempDay, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_TempNight,
                LimitTypeID = Constants.LimitType_LCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_TempNight, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_TempNight,
                LimitTypeID = Constants.LimitType_UCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_TempNight, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_Humidity,
                LimitTypeID = Constants.LimitType_LCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_Humidity, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_Humidity,
                LimitTypeID = Constants.LimitType_UCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_Humidity, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Reservoir,
                MeasurementTypeID = Constants.MeasurementType_pH,
                LimitTypeID = Constants.LimitType_LCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Reservoir, Constants.MeasurementType_pH, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Reservoir,
                MeasurementTypeID = Constants.MeasurementType_pH,
                LimitTypeID = Constants.LimitType_UCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Reservoir, Constants.MeasurementType_pH, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Reservoir,
                MeasurementTypeID = Constants.MeasurementType_PPM,
                LimitTypeID = Constants.LimitType_LCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Reservoir, Constants.MeasurementType_PPM, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Reservoir,
                MeasurementTypeID = Constants.MeasurementType_PPM,
                LimitTypeID = Constants.LimitType_UCL,
                Value = await _recipeDAL.GetLastRecipeStepLimitValueAsync(recipeStep.RecipeID,
                    Constants.LocationType_Reservoir, Constants.MeasurementType_PPM, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            await _recipeDAL.AddRecipeStepLimitAsync(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStep.RecipeStepLimits = recipeStepLimits;

            #endregion

            List<RecipeStepAmount> recipeStepAmounts = new();

            foreach(RecipeChemList recipeChemList in await _recipeDAL.GetRecipeChemListByRecipeIDAsync(recipeStep.RecipeID))
            {
                RecipeStepAmount recipeStepAmount = new()
                {
                    ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                    RecipeStepID = recipeStep.ID,
                    ChemicalID = recipeChemList.ChemicalID,
                    Amount = await _recipeDAL.GetLastRecipeStepAmountValueAsync(recipeStep.RecipeID,recipeChemList.ChemicalID),
                    CreatedBy = user,
                    CreateDate = DateTime.Now,
                    ChangedBy = user,
                    ChangeDate = DateTime.Now
                };

                await _recipeDAL.AddRecipeStepAmountAsync(recipeStepAmount);
                recipeStepAmounts.Add(recipeStepAmount);
            }

            recipeStep.RecipeStepAmounts = recipeStepAmounts;

            return Json(recipeStep);
        }

        [HttpGet]
        public async Task<JsonResult> AddRecipe(Recipe recipe)
        {
            string user = Environment.UserName;

            recipe.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
            recipe.Name = _config.GetValue<string>("YARGStrings:NewRecipeNameDefault");
            recipe.CreatedBy = user;
            recipe.CreateDate = DateTime.Now;
            recipe.ChangedBy = user;
            recipe.ChangeDate = DateTime.Now;

            await _recipeDAL.AddRecipeAsync(recipe);

            return Json(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipeStepLimit(RecipeStepLimit recipeStepLimit)
        {
            string user = Environment.UserName;

            recipeStepLimit.ChangedBy = user;
            recipeStepLimit.ChangeDate = DateTime.Now;

            await _recipeDAL.UpdateRecipeStepLimitAsync(recipeStepLimit);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipeStepAmount(RecipeStepAmount recipeStepAmount)
        {
            string user = Environment.UserName;

            recipeStepAmount.ChangedBy = user;
            recipeStepAmount.ChangeDate = DateTime.Now;

            await _recipeDAL.UpdateRecipeStepAmountAsync(recipeStepAmount);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipeStepIrrigation(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            await _recipeDAL.UpdateRecipeStepIrrigationAsync(recipeStep);

            CurrentIrrigationCalcs currentIrrigationCalcs = new();

            if(currentIrrigationCalcs.RecipeID == recipeStep.RecipeID & currentIrrigationCalcs.GrowWeek == recipeStep.WeekNumber)
            {
                await _wateringScheduleDAL.RebuildWateringSchedule();
            }

            return Ok();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipeStepSoaktime(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            await _recipeDAL.UpdateRecipeStepSoaktimeAsync(recipeStep);

            return Ok();

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipeStepMorningSip(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            await _recipeDAL.UpdateRecipeStepMorningSipAsync(recipeStep);

            return Ok();

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipeStepEveningSip(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            await _recipeDAL.UpdateRecipeStepEveningSipAsync(recipeStep);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipeStepLightCycle(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            await _recipeDAL.UpdateRecipeStepLightCycleAsync(recipeStep);

            return Ok();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipeStep(Guid StepID)
        {
            await _recipeDAL.DeleteRecipeStepLimitAsync(StepID);
            await _recipeDAL.DeleteRecipeStepAmountAsync(StepID);
            await _recipeDAL.DeleteRecipeStepAsync(StepID);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipe(Guid RecipeID)
        {
            await _recipeDAL.DeleteRecipeAsync(RecipeID);
            return Ok();
        }

        // GET: RecipeController/Details/5
        // GET: RecipeLimitTypeController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: RecipeController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind("Name")] Recipe recipe)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string user = Environment.UserName;

        //        limitType.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
        //        limitType.CreatedBy = user;
        //        limitType.CreateDate = DateTime.Now;
        //        limitType.ChangedBy = user;
        //        limitType.ChangeDate = DateTime.Now;
        //        limitType.IsActive = true;

        //        _limitTypeDAL.AddLimitType(limitType);

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(limitType);
        //}

        //// GET: LimitTypeController/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(_limitTypeDAL.GetLimitTypeByID(id.Value));
        //}

        //// POST: LimitTypeController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Guid id, [Bind("ID,Name,Sorting,IsActive")] LimitType limitType)
        //{
        //    if (id != limitType.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        string user = Environment.UserName;

        //        limitType.ChangedBy = user;
        //        limitType.ChangeDate = DateTime.Now;

        //        _limitTypeDAL.SaveLimitType(limitType);

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(limitType);
        //}

        //// GET: LimitTypeController/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(_limitTypeDAL.GetLimitTypeByID(id.Value));
        //}

        //// POST: LimitTypeController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(Guid id)
        //{
        //    _limitTypeDAL.DeleteLimitType(id);

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
