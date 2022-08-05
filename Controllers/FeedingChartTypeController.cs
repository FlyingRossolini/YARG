using GardenMVC.Common_Types;
using GardenMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace GardenMVC.Models
{
    public class FeedingChartTypeController : Controller
    {
        private readonly FeedingChartTypeDAL _feedingChartTypeDAL;

        public FeedingChartTypeController(IConfiguration configuration)
        {
            _feedingChartTypeDAL = new(configuration);
        }

        // GET: FeedingChartTypeController
        public ActionResult Index()
        {
            return View(_feedingChartTypeDAL.GetFeedingChartTypes());
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
        public ActionResult Create([Bind("ID,Name,Sorting")] FeedingChartType feedingChartType)
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

                _feedingChartTypeDAL.AddFeedingChartType(feedingChartType);

                return RedirectToAction(nameof(Index));
            }
            return View(feedingChartType);
        }

        // GET: FeedingChartTypeController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(_feedingChartTypeDAL.GetFeedingChartTypeByID(id.Value));
        }

        // POST: FeedingChartTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,Sorting,IsActive")] FeedingChartType feedingChartType)
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

                _feedingChartTypeDAL.SaveFeedingChartType(feedingChartType);

                return RedirectToAction(nameof(Index));
            }
            return View(feedingChartType);
        }

        // GET: FeedingChartTypeController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_feedingChartTypeDAL.GetFeedingChartTypeByID(id.Value));
        }

        // POST: FeedingChartTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _feedingChartTypeDAL.DeleteFeedingChartType(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
