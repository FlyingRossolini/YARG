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
    public class LimitTypeController : Controller
    {
        private readonly LimitTypeDAL _limitTypeDAL;

        public LimitTypeController(IConfiguration configuration)
        {
            _limitTypeDAL = new(configuration);
        }

        // GET: LimitTypeController
        public async Task<ActionResult> Index()
        {
            return View(await _limitTypeDAL.GetLimitTypesAsync());
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
        public async Task<ActionResult> Create([Bind("ID,Name,Sorting")] LimitType limitType)
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

                await _limitTypeDAL.AddLimitTypeAsync(limitType);

                return RedirectToAction(nameof(Index));
            }
            return View(limitType);
        }

        // GET: LimitTypeController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(await _limitTypeDAL.GetLimitTypeByIDAsync(id.Value));
        }

        // POST: LimitTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,Name,Sorting,IsActive")] LimitType limitType)
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

                await _limitTypeDAL.SaveLimitTypeAsync(limitType);

                return RedirectToAction(nameof(Index));
            }
            return View(limitType);
        }

        // GET: LimitTypeController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _limitTypeDAL.GetLimitTypeByIDAsync(id.Value));
        }

        // POST: LimitTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _limitTypeDAL.DeleteLimitTypeAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
