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
    public class CropController : Controller
    {
        private readonly CropDAL _cropDAL;

        public CropController(IConfiguration configuration)
        {
            _cropDAL = new(configuration);
        }

        // GET: CropController
        public async Task<ActionResult> Index()
        {
            return View(await _cropDAL.GetCropsAsync());
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
        public async Task <ActionResult> Create([Bind("ID,Name")] Crop crop)
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

                await _cropDAL.AddCropAsync(crop);

                return RedirectToAction(nameof(Index));
            }
            return View(crop);
        }

        // GET: CropController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(await _cropDAL.GetCropByIDAsync(id.Value));
        }

        // POST: CropController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,Name,IsActive")] Crop crop)
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

                await _cropDAL.SaveCrop(crop);

                return RedirectToAction(nameof(Index));
            }
            return View(crop);
        }

        // GET: CropController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _cropDAL.GetCropByIDAsync(id.Value));
        }

        // POST: CropController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _cropDAL.DeleteCrop(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
