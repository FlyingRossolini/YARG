using GardenMVC.Common_Types;
using GardenMVC.DAL;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GardenMVC.Models
{
    public class ChemicalTypeController : Controller
    {
        private readonly ChemicalTypeDAL _chemicalTypeDAL;

        public ChemicalTypeController()
        {
            _chemicalTypeDAL = new();
        }

        // GET: ChemicalTypeController
        public ActionResult Index()
        {
            return View(_chemicalTypeDAL.GetChemicalTypes());
        }

        // GET: ChemicalTypeController/Details/5
        // GET: ChemicalTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChemicalTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,Name,IsH2O2,IsPhUp,IsPhDown,Sorting")] ChemicalType chemicalType)
        {
            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                chemicalType.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                chemicalType.CreatedBy = user;
                chemicalType.CreateDate = DateTime.Now;
                chemicalType.ChangedBy = user;
                chemicalType.ChangeDate = DateTime.Now;
                chemicalType.IsActive = true;

                _chemicalTypeDAL.AddChemicalType(chemicalType);

                return RedirectToAction(nameof(Index));
            }
            return View(chemicalType);
        }

        // GET: ChemicalTypeController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(_chemicalTypeDAL.GetChemicalTypeByID(id.Value));
        }

        // POST: ChemicalTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("ID,Name,IsH2O2,IsPhUp,IsPhDown,Sorting,IsActive")] ChemicalType chemicalType)
        {
            if (id != chemicalType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string user = Environment.UserName;

                chemicalType.ChangedBy = user;
                chemicalType.ChangeDate = DateTime.Now;

                _chemicalTypeDAL.SaveChemicalType(chemicalType);

                return RedirectToAction(nameof(Index));
            }
            return View(chemicalType);
        }

        // GET: ChemicalTypeController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(_chemicalTypeDAL.GetChemicalTypeByID(id.Value));
        }

        // POST: ChemicalTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            _chemicalTypeDAL.DeleteChemicalType(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
