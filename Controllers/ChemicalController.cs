using YARG.Common_Types;
using YARG.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace YARG.Models
{
    [Authorize]
    public class ChemicalController : Controller
    {
        private readonly ChemicalDAL _chemicalDAL;
        private readonly ChemicalTypeDAL _chemicalTypeDAL;

        public ChemicalController(IConfiguration configuration)
        {
            _chemicalDAL = new(configuration);
            _chemicalTypeDAL = new(configuration);
        }

        // GET: ChemicalController
        public async Task<ActionResult> Index()
        {
            return View(await _chemicalDAL.GetChemicalsAsync());
        }

        // GET: ChemicalController/Details/5
        // GET: ChemicalController/Create
        public async Task<ActionResult> Create()
        {
            List<SelectListItem> ddChemicalType = new();
            var chemTypeList = await _chemicalTypeDAL.GetChemicalTypesAsync();

            foreach (var chemical in chemTypeList)
            {
                if (chemical.IsActive)
                {
                    ddChemicalType.Add(new SelectListItem { Value = chemical.ID.ToString(), Text = chemical.Name });
                }
            }

            ViewBag.ddChemicalType = ddChemicalType;

            return View();
        }

        // POST: ChemicalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ID,Name,Manufacturer,ChemicalTypeID,PricePerL,InStockAmount," +
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

                await _chemicalDAL.AddChemicalAsync(chemical);

                return RedirectToAction(nameof(Index));
            }

            List<SelectListItem> ddChemicalType = new();
            var chemTypeList = await _chemicalTypeDAL.GetChemicalTypesAsync();

            foreach (var chem in chemTypeList)
            {
                if (chem.IsActive)
                {
                    ddChemicalType.Add(new SelectListItem { Value = chem.ID.ToString(), Text = chem.Name });
                }
            }

            ViewBag.ddChemicalType = ddChemicalType;

            return View(chemical);
        }

        // GET: ChemicalController/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> ddChemicalType = new();
            var chemTypeList = await _chemicalTypeDAL.GetChemicalTypesAsync();

            foreach (var chemical in chemTypeList)
            {
                if (chemical.IsActive)
                {
                    ddChemicalType.Add(new SelectListItem { Value = chemical.ID.ToString(), Text = chemical.Name });
                }
            }

            ViewBag.ddChemicalType = ddChemicalType;

            return View(await _chemicalDAL.GetChemicalByIDAsync(id.Value));
        }

        // POST: ChemicalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [Bind("ID,Name,Manufacturer,ChemicalTypeID,PricePerL," +
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

                await _chemicalDAL.SaveChemicalAsync(chemical);

                return RedirectToAction(nameof(Index));
            }

            List<SelectListItem> ddChemicalType = new();
            var chemTypeList = await _chemicalTypeDAL.GetChemicalTypesAsync();

            foreach (var chem in chemTypeList)
            {
                if (chem.IsActive)
                {
                    ddChemicalType.Add(new SelectListItem { Value = chem.ID.ToString(), Text = chem.Name });
                }
            }

            ViewBag.ddChemicalType = ddChemicalType;

            return View(chemical);
        }

        // GET: ChemicalController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _chemicalDAL.GetChemicalByIDAsync(id.Value));
        }

        // POST: ChemicalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _chemicalDAL.DeleteChemicalAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
