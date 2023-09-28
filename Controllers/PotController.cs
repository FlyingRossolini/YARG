using System;
using Microsoft.AspNetCore.Mvc;
using YARG.Models;
using YARG.Common_Types;
using YARG.DAL;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace YARG.Controllers
{
    [Authorize]
    public class PotController : Controller
    {
        private readonly PotDAL _potDAL;
        private readonly WateringScheduleDAL _wateringScheduleDAL;

        public PotController(IConfiguration configuration)
        {
            _potDAL = new(configuration);
            _wateringScheduleDAL = new(configuration);
        }

        // GET: Pot
        public async Task<IActionResult> Index()
        {
            bool flgCanAddNew = false;

            if(await _potDAL.PotCountAsync() < 4)
            {
                flgCanAddNew = true;
            }

            ViewBag.flgCanAddNew = flgCanAddNew;

            return View(await _potDAL.GetPotsAsync());
        }

        // GET: Pot/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,EFDuration,EFAmount,EbbSpeed,FlowSpeed")] Pot pot)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                pot.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                pot.QueuePosition = await _potDAL.GetNextQueuePositionAsync();
                pot.CreatedBy = user;
                pot.CreateDate = DateTime.Now;
                pot.ChangedBy = user;
                pot.ChangeDate = DateTime.Now;
                pot.IsActive = true;

                await _potDAL.AddPotAsync(pot);

                await _wateringScheduleDAL.RebuildWateringSchedule();

                return RedirectToAction(nameof(Index));
            }
            return View(pot);
        }

        // GET: Pot/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _potDAL.GetPotByIDAsync(id.Value));
        }

        // POST: Pot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Name,EFDuration,EFAmount,EbbSpeed,FlowSpeed," +
            "IsActive")] Pot pot)
        {
            if (id != pot.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                pot.ChangedBy = user;
                pot.ChangeDate = DateTime.Now;

                if (await _potDAL.SavePotAsync(pot) == true)
                {
                    await _wateringScheduleDAL.RebuildWateringSchedule();
                };

                return RedirectToAction(nameof(Index), "Home");
            }
            return View(pot);
        }

        // GET: Pot/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _potDAL.GetPotByIDAsync(id.Value));
        }

        // POST: Pot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _potDAL.DeletePotAsync(id);

            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
