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
    public class FeedingChartTypeController : Controller
    {
        private readonly FeedingChartTypeDAL _feedingChartTypeDAL;

        public FeedingChartTypeController(IConfiguration configuration)
        {
            _feedingChartTypeDAL = new(configuration);
        }

        // GET: FeedingChartTypeController
        public async Task<ActionResult> Index()
        {
            return View(await _feedingChartTypeDAL.GetFeedingChartTypesAsync());
        }

        // GET: FeedingChartController/Details/5
        // GET: FeedingChartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeedingChartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ID,Name,Sorting")] FeedingChartType feedingChartType)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                feedingChartType.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                feedingChartType.CreatedBy = user;
                feedingChartType.CreateDate = DateTime.Now;
                feedingChartType.ChangedBy = user;
                feedingChartType.ChangeDate = DateTime.Now;
                feedingChartType.IsActive = true;

                await _feedingChartTypeDAL.AddFeedingChartTypeAsync(feedingChartType);

                return RedirectToAction(nameof(Index));
            }
            return View(feedingChartType);
        }

        // GET: FeedingChartTypeController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(await _feedingChartTypeDAL.GetFeedingChartTypeByIDAsync(id.Value));
        }

        // POST: FeedingChartTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,Name,Sorting,IsActive")] FeedingChartType feedingChartType)
        {
            if (id != feedingChartType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                feedingChartType.ChangedBy = user;
                feedingChartType.ChangeDate = DateTime.Now;

                await _feedingChartTypeDAL.SaveFeedingChartTypeAsync(feedingChartType);

                return RedirectToAction(nameof(Index));
            }
            return View(feedingChartType);
        }

        // GET: FeedingChartTypeController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _feedingChartTypeDAL.GetFeedingChartTypeByIDAsync(id.Value));
        }

        // POST: FeedingChartTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _feedingChartTypeDAL.DeleteFeedingChartTypeAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
