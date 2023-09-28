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
    public class LocationController : Controller
    {
        private readonly LocationDAL _locationDAL;

        public LocationController(IConfiguration configuration)
        {
            _locationDAL = new(configuration);
        }

        // GET: LocationController
        public async Task<ActionResult> Index()
        {
            return View(await _locationDAL.GetLocationsAsync());
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
        public async Task<ActionResult> Create([Bind("ID,Name,Sorting,IsShowOnLandingPage")] Location location)
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

                await _locationDAL.AddLocationAsync(location);

                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: LocationController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(await _locationDAL.GetLocationByIDAsync(id.Value));
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,Name,Sorting,IsShowOnLandingPage,IsActive")] Location location)
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

                await _locationDAL.SaveLocationAsync(location);

                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: LocationController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _locationDAL.GetLocationByIDAsync(id.Value));
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _locationDAL.DeleteLocationAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
