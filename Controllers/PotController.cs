using System;
using Microsoft.AspNetCore.Mvc;
using YARG.Models;
using YARG.Common_Types;
using YARG.DAL;
using Microsoft.Extensions.Configuration;

namespace YARG.Controllers
{
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
        public IActionResult Index()
        {
            bool flgCanAddNew = false;

            if(_potDAL.PotCount() < 4)
            {
                flgCanAddNew = true;
            }

            ViewBag.flgCanAddNew = flgCanAddNew;

            return View(_potDAL.GetPots());
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
        public IActionResult Create([Bind("ID,Name,EFDuration,EFAmount,EbbSpeed,FlowSpeed")] Pot pot)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                pot.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                pot.QueuePosition = _potDAL.GetNextQueuePosition();
                pot.CreatedBy = user;
                pot.CreateDate = DateTime.Now;
                pot.ChangedBy = user;
                pot.ChangeDate = DateTime.Now;
                pot.IsActive = true;

                _potDAL.AddPot(pot);

                _wateringScheduleDAL.RebuildWateringSchedule();

                return RedirectToAction(nameof(Index));
            }
            return View(pot);
        }

        // GET: Pot/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_potDAL.GetPotByID(id.Value));
        }

        // POST: Pot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("ID,Name,EFDuration,EFAmount,EbbSpeed,FlowSpeed," +
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

                if (_potDAL.SavePot(pot) == true)
                {
                    _wateringScheduleDAL.RebuildWateringSchedule();
                };

                return RedirectToAction(nameof(Index), "Home");
            }
            return View(pot);
        }

        // GET: Pot/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_potDAL.GetPotByID(id.Value));
        }

        // POST: Pot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _potDAL.DeletePot(id);

            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
