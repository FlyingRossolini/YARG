using YARG.Common_Types;
using YARG.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YARG.Models
{
    public class RecipeController : Controller
    {
        private readonly RecipeDAL _recipeDAL;
        private readonly LightCycleDAL _lightCycleDAL;
        private readonly LocationDAL _locationDAL;
        private readonly IConfiguration _config;

        public RecipeController(IConfiguration configuration)
        {
            _recipeDAL = new(configuration);
            _lightCycleDAL = new(configuration);
            _locationDAL = new(configuration);
            _config = configuration;
        }

        // GET: RecipeController
        public ActionResult Index()
        {
            List<SelectListItem> ddDaylight = new();
            var daylightList = (from daylight in _lightCycleDAL.GetLightCycles()
                                where daylight.IsActive == true
                                select new { daylight.ID, daylight.Name }).ToList();

            foreach (var item in daylightList)
            {
                ddDaylight.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddDaylight = ddDaylight;

            RecipeViewModel recipeViewModel = new();
            recipeViewModel.recipes = _recipeDAL.GetRecipes();
            recipeViewModel.locations = _locationDAL.GetLocationsForRecipe();
            recipeViewModel.recipeChemListViewModels = _recipeDAL.GetRecipeChemListViewModels();

            return View(recipeViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRecipeName(Recipe recipe)
        {
            string user = Environment.UserName;

            recipe.ChangedBy = user;
            recipe.ChangeDate = DateTime.Now;

            _recipeDAL.UpdateRecipeName(recipe);

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
        public JsonResult AddWeek(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
            recipeStep.CreatedBy = user;
            recipeStep.CreateDate = DateTime.Now;
            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            RecipeStep lstRecipeStep = _recipeDAL.GetLastRecipeStepByRecipeID(recipeStep.RecipeID);

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

            _recipeDAL.AddRecipeStep(recipeStep);

            #region Build Recipe Step Limits
            List<RecipeStepLimit> recipeStepLimits = new();

            RecipeStepLimit recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_TempDay,
                LimitTypeID = Constants.LimitType_LCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID, 
                    Constants.LocationType_Habitat, Constants.MeasurementType_TempDay, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_TempDay,
                LimitTypeID = Constants.LimitType_UCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_TempDay, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_TempNight,
                LimitTypeID = Constants.LimitType_LCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_TempNight, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_TempNight,
                LimitTypeID = Constants.LimitType_UCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_TempNight, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_Humidity,
                LimitTypeID = Constants.LimitType_LCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_Humidity, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Habitat,
                MeasurementTypeID = Constants.MeasurementType_Humidity,
                LimitTypeID = Constants.LimitType_UCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Habitat, Constants.MeasurementType_Humidity, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Reservoir,
                MeasurementTypeID = Constants.MeasurementType_pH,
                LimitTypeID = Constants.LimitType_LCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Reservoir, Constants.MeasurementType_pH, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Reservoir,
                MeasurementTypeID = Constants.MeasurementType_pH,
                LimitTypeID = Constants.LimitType_UCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Reservoir, Constants.MeasurementType_pH, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Reservoir,
                MeasurementTypeID = Constants.MeasurementType_PPM,
                LimitTypeID = Constants.LimitType_LCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Reservoir, Constants.MeasurementType_PPM, Constants.LimitType_LCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStepLimit = new()
            {
                ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                RecipeStepID = recipeStep.ID,
                LocationID = Constants.LocationType_Reservoir,
                MeasurementTypeID = Constants.MeasurementType_PPM,
                LimitTypeID = Constants.LimitType_UCL,
                Value = _recipeDAL.GetLastRecipeStepLimitValue(recipeStep.RecipeID,
                    Constants.LocationType_Reservoir, Constants.MeasurementType_PPM, Constants.LimitType_UCL),
                CreatedBy = user,
                CreateDate = DateTime.Now,
                ChangedBy = user,
                ChangeDate = DateTime.Now
            };

            _recipeDAL.AddRecipeStepLimit(recipeStepLimit);

            recipeStepLimits.Add(recipeStepLimit);

            recipeStep.RecipeStepLimits = recipeStepLimits;

            #endregion

            List<RecipeStepAmount> recipeStepAmounts = new();

            foreach(RecipeChemList recipeChemList in _recipeDAL.GetRecipeChemListByRecipeID(recipeStep.RecipeID))
            {
                RecipeStepAmount recipeStepAmount = new()
                {
                    ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString),
                    RecipeStepID = recipeStep.ID,
                    ChemicalID = recipeChemList.ChemicalID,
                    Amount = _recipeDAL.GetLastRecipeStepAmountValue(recipeStep.RecipeID,recipeChemList.ChemicalID),
                    CreatedBy = user,
                    CreateDate = DateTime.Now,
                    ChangedBy = user,
                    ChangeDate = DateTime.Now
                };

                _recipeDAL.AddRecipeStepAmount(recipeStepAmount);
                recipeStepAmounts.Add(recipeStepAmount);
            }

            recipeStep.RecipeStepAmounts = recipeStepAmounts;

            return Json(recipeStep);
        }

        [HttpGet]
        public JsonResult AddRecipe(Recipe recipe)
        {
            string user = Environment.UserName;

            recipe.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
            recipe.Name = _config.GetValue<string>("YARGStrings:NewRecipeNameDefault");
            recipe.CreatedBy = user;
            recipe.CreateDate = DateTime.Now;
            recipe.ChangedBy = user;
            recipe.ChangeDate = DateTime.Now;

            _recipeDAL.AddRecipe(recipe);

            return Json(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRecipeStepLimit(RecipeStepLimit recipeStepLimit)
        {
            string user = Environment.UserName;

            recipeStepLimit.ChangedBy = user;
            recipeStepLimit.ChangeDate = DateTime.Now;

            _recipeDAL.UpdateRecipeStepLimit(recipeStepLimit);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRecipeStepAmount(RecipeStepAmount recipeStepAmount)
        {
            string user = Environment.UserName;

            recipeStepAmount.ChangedBy = user;
            recipeStepAmount.ChangeDate = DateTime.Now;

            _recipeDAL.UpdateRecipeStepAmount(recipeStepAmount);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRecipeStepIrrigation(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            _recipeDAL.UpdateRecipeStepIrrigation(recipeStep);

            return Ok();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRecipeStepSoaktime(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            _recipeDAL.UpdateRecipeStepSoaktime(recipeStep);

            return Ok();

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRecipeStepMorningSip(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            _recipeDAL.UpdateRecipeStepMorningSip(recipeStep);

            return Ok();

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRecipeStepEveningSip(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            _recipeDAL.UpdateRecipeStepEveningSip(recipeStep);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRecipeStepLightCycle(RecipeStep recipeStep)
        {
            string user = Environment.UserName;

            recipeStep.ChangedBy = user;
            recipeStep.ChangeDate = DateTime.Now;

            _recipeDAL.UpdateRecipeStepLightCycle(recipeStep);

            return Ok();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRecipeStep(Guid StepID)
        {
            _recipeDAL.DeleteRecipeStepLimit(StepID);
            _recipeDAL.DeleteRecipeStepAmount(StepID);
            _recipeDAL.DeleteRecipeStep(StepID);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRecipe(Guid RecipeID)
        {
            _recipeDAL.DeleteRecipe(RecipeID);
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
