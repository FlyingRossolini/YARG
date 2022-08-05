using GardenMVC.Common_Types;
using GardenMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace GardenMVC.Models
{
    public class CropController : Controller
    {
        private readonly CropDAL _cropDAL;

        public CropController(IConfiguration configuration)
        {
            _cropDAL = new(configuration);
        }

        // GET: CropController
        public ActionResult Index()
        {
            return View(_cropDAL.GetCrops());
        }

        // GET: CropController/Details/5
        // GET: CropController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CropController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name")] Crop crop)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                crop.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                crop.CreatedBy = user;
                crop.CreateDate = DateTime.Now;
                crop.ChangedBy = user;
                crop.ChangeDate = DateTime.Now;
                crop.IsActive = true;

                _cropDAL.AddCrop(crop);

                return RedirectToAction(nameof(Index));
            }
            return View(crop);
        }

        // GET: CropController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(_cropDAL.GetCropByID(id.Value));
        }

        // POST: CropController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,IsActive")] Crop crop)
        {
            if (id != crop.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                crop.ChangedBy = user;
                crop.ChangeDate = DateTime.Now;

                _cropDAL.SaveCrop(crop);

                return RedirectToAction(nameof(Index));
            }
            return View(crop);
        }

        // GET: CropController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_cropDAL.GetCropByID(id.Value));
        }

        // POST: CropController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _cropDAL.DeleteCrop(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
