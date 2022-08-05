using GardenMVC.Common_Types;
using GardenMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace GardenMVC.Models
{
    public class LimitTypeController : Controller
    {
        private readonly LimitTypeDAL _limitTypeDAL;

        public LimitTypeController(IConfiguration configuration)
        {
            _limitTypeDAL = new(configuration);
        }

        // GET: LimitTypeController
        public ActionResult Index()
        {
            return View(_limitTypeDAL.GetLimitTypes());
        }

        // GET: LimitTypeController/Details/5
        // GET: LimitTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LimitTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name,Sorting")] LimitType limitType)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                limitType.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                limitType.CreatedBy = user;
                limitType.CreateDate = DateTime.Now;
                limitType.ChangedBy = user;
                limitType.ChangeDate = DateTime.Now;
                limitType.IsActive = true;

                _limitTypeDAL.AddLimitType(limitType);

                return RedirectToAction(nameof(Index));
            }
            return View(limitType);
        }

        // GET: LimitTypeController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(_limitTypeDAL.GetLimitTypeByID(id.Value));
        }

        // POST: LimitTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,Sorting,IsActive")] LimitType limitType)
        {
            if (id != limitType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                limitType.ChangedBy = user;
                limitType.ChangeDate = DateTime.Now;

                _limitTypeDAL.SaveLimitType(limitType);

                return RedirectToAction(nameof(Index));
            }
            return View(limitType);
        }

        // GET: LimitTypeController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_limitTypeDAL.GetLimitTypeByID(id.Value));
        }

        // POST: LimitTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _limitTypeDAL.DeleteLimitType(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
