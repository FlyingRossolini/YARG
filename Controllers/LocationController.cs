using YARG.Common_Types;
using YARG.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace YARG.Models
{
    public class LocationController : Controller
    {
        private readonly LocationDAL _locationDAL;

        public LocationController(IConfiguration configuration)
        {
            _locationDAL = new(configuration);
        }

        // GET: LocationController
        public ActionResult Index()
        {
            return View(_locationDAL.GetLocations());
        }

        // GET: LocationController/Details/5
        // GET: LocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name,Sorting,IsShowOnLandingPage")] Location location)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                location.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                location.CreatedBy = user;
                location.CreateDate = DateTime.Now;
                location.ChangedBy = user;
                location.ChangeDate = DateTime.Now;
                location.IsActive = true;

                _locationDAL.AddLocation(location);

                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(_locationDAL.GetLocationByID(id.Value));
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,Sorting,IsShowOnLandingPage,IsActive")] Location location)
        {
            if (id != location.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                location.ChangedBy = user;
                location.ChangeDate = DateTime.Now;

                _locationDAL.SaveLocation(location);

                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: LocationController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_locationDAL.GetLocationByID(id.Value));
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _locationDAL.DeleteLocation(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
