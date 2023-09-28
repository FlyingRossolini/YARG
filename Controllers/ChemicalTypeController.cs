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
    public class ChemicalTypeController : Controller
    {
        private readonly ChemicalTypeDAL _chemicalTypeDAL;

        public ChemicalTypeController(IConfiguration configuration)
        {
            _chemicalTypeDAL = new(configuration);
        }

        // GET: ChemicalTypeController
        public async Task<ActionResult> Index()
        {
            return View(await _chemicalTypeDAL.GetChemicalTypesAsync());
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
        public async Task<ActionResult> Create([Bind("ID,Name,IsH2O2,IsPhUp,IsPhDown,Sorting")] ChemicalType chemicalType)
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

                await _chemicalTypeDAL.AddChemicalTypeAsync(chemicalType);

                return RedirectToAction(nameof(Index));
            }
            return View(chemicalType);
        }

        // GET: ChemicalTypeController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(await _chemicalTypeDAL.GetChemicalTypeByIDAsync(id.Value));
        }

        // POST: ChemicalTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,Name,IsH2O2,IsPhUp,IsPhDown,Sorting,IsActive")] ChemicalType chemicalType)
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

                await _chemicalTypeDAL.SaveChemicalTypeAsync(chemicalType);

                return RedirectToAction(nameof(Index));
            }
            return View(chemicalType);
        }

        // GET: ChemicalTypeController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _chemicalTypeDAL.GetChemicalTypeByIDAsync(id.Value));
        }

        // POST: ChemicalTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _chemicalTypeDAL.DeleteChemicalType(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
