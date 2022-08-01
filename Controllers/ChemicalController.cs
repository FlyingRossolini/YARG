using GardenMVC.Common_Types;
using GardenMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GardenMVC.Models
{
    public class ChemicalController : Controller
    {
        private readonly ChemicalDAL _chemicalDAL;
        private readonly ChemicalTypeDAL _chemicalTypeDAL;

        public ChemicalController()
        {
            _chemicalDAL = new();
            _chemicalTypeDAL = new();
        }

        // GET: ChemicalController
        public ActionResult Index()
        {
            return View(_chemicalDAL.GetChemicals());
        }

        // GET: ChemicalController/Details/5
        // GET: ChemicalController/Create
        public ActionResult Create()
        {
            List<SelectListItem> ddChemicalType = new();
            var chemicalList = (from c in _chemicalTypeDAL.GetChemicalTypes()
                            where c.IsActive == true
                            orderby c.Sorting
                            
                            select new { c.ID, c.Name }).ToList();

            foreach (var item in chemicalList)
            {
                ddChemicalType.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddChemicalType = ddChemicalType;

            return View();
        }

        // POST: ChemicalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name,Manufacturer,ChemicalTypeID,PricePerL,InStockAmount," +
            "MinReorderPoint")] Chemical chemical)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                chemical.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                chemical.CreatedBy = user;
                chemical.CreateDate = DateTime.Now;
                chemical.ChangedBy = user;
                chemical.ChangeDate = DateTime.Now;
                chemical.IsActive = true;

                _chemicalDAL.AddChemical(chemical);

                return RedirectToAction(nameof(Index));
            }

            List<SelectListItem> ddChemicalType = new();
            var chemicalList = (from c in _chemicalTypeDAL.GetChemicalTypes()
                                where c.IsActive == true
                                orderby c.Sorting
                                
                                select new { c.ID, c.Name }).ToList();

            foreach (var item in chemicalList)
            {
                ddChemicalType.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddChemicalType = ddChemicalType;

            return View(chemical);
        }

        // GET: ChemicalController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> ddChemicalType = new();
            var chemicalList = (from c in _chemicalTypeDAL.GetChemicalTypes()
                                where c.IsActive == true
                                orderby c.Sorting

                                select new { c.ID, c.Name }).ToList();

            foreach (var item in chemicalList)
            {
                ddChemicalType.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddChemicalType = ddChemicalType;

            return View(_chemicalDAL.GetChemicalByID(id.Value));
        }

        // POST: ChemicalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,Manufacturer,ChemicalTypeID,PricePerL," +
            "InStockAmount,MinReorderPoint,IsActive")] Chemical chemical)
        {
            if (id != chemical.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                chemical.ChangedBy = user;
                chemical.ChangeDate = DateTime.Now;

                _chemicalDAL.SaveChemical(chemical);

                return RedirectToAction(nameof(Index));
            }

            List<SelectListItem> ddChemicalType = new();
            var chemicalList = (from c in _chemicalTypeDAL.GetChemicalTypes()
                                where c.IsActive == true
                                orderby c.Sorting

                                select new { c.ID, c.Name }).ToList();

            foreach (var item in chemicalList)
            {
                ddChemicalType.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }

            ViewBag.ddChemicalType = ddChemicalType;

            return View(chemical);
        }

        // GET: ChemicalController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_chemicalDAL.GetChemicalByID(id.Value));
        }

        // POST: ChemicalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _chemicalDAL.DeleteChemical(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
