using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using lab4.Data;
using lab4.Models;
using lab4.ViewModels;
using lab4.Filters;
using Newtonsoft.Json;

namespace lab4.Controllers
{
    [CatchExceptionFilter]
    public class MedicineController : Controller
    {
        private int pageSize = 5;
        private Context db;
        private Medicine _medicine = new Medicine
        {
            MedicineName = "",
            MedicineIndications = "",
            MedicineContraindications = "",
            MedicineManufacturer = "",
            MedicinePackaging = "",
            MedicineDosage = ""
        };

        public MedicineController(Context medicineContext) {
            db = medicineContext;
        }

        [HttpGet]
        public IActionResult Index(SortState sortOrder = SortState.No, int index = 0)
        {
            Medicine sessionMedicines = HttpContext.Session.GetObject<Medicine>("Medicines");
            string sessionSortState = HttpContext.Session.GetString("SortState");
            int? page = HttpContext.Session.GetInt32("Page");
            if (page == null)
            {
                page = 0;
                HttpContext.Session.SetInt32("Page", 0);
            }
            else
            {
                if (!(page < 1 && index < 0))
                    page += index;
                HttpContext.Session.SetInt32("Page", (int)page);
            }

            if (sessionMedicines != null)
            {
                _medicine = sessionMedicines;
            }

            if (sessionSortState != null)
                if (sortOrder == SortState.No)
                    sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState);

            ViewData["NameSort"] = sortOrder == SortState.NameDesc ? SortState.NameAsc : SortState.NameDesc;

            HttpContext.Session.SetString("SortState", sortOrder.ToString());
            IQueryable<Medicine> medicine = Sort(db.Medicines, sortOrder,
                _medicine.MedicineName, (int)page);
            MedicinesViewModel medicinesView = new MedicinesViewModel
            {
                MedicineViewModel = _medicine,
                PageViewModel = medicine,
                PageNumber = (int)page
            };

            return View(medicinesView);
        }

        [HttpPost]
        public IActionResult Index(Medicine medicine)
        {
            var sessionSortState = HttpContext.Session.GetString("SortState");
            SortState sortOrder = new SortState();
            if (sessionSortState != null)
                sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState);

            int? page = HttpContext.Session.GetInt32("Page");
            if (page == null)
            {
                page = 0;
                HttpContext.Session.SetInt32("Page", 0);
            }

            IQueryable<Medicine> medicines = Sort(db.Medicines, sortOrder,
                medicine.MedicineName, (int)page);
            HttpContext.Session.SetObject("Medicines", medicine);

            MedicinesViewModel medicinesView = new MedicinesViewModel
            {
                MedicineViewModel = medicine,
                PageViewModel = medicines,
                PageNumber = (int)page
            };

            return View(medicinesView);
        }

        private IQueryable<Medicine> Sort(IQueryable<Medicine> medicines,
            SortState sortOrder, string name, int page)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    medicines = medicines.OrderBy(s => s.MedicineName);
                    break;
                case SortState.NameDesc:
                    medicines = medicines.OrderByDescending(s => s.MedicineName);
                    break;
            }
            medicines = medicines.Where(o => o.MedicineName.Contains(name ?? "")).Skip(page * pageSize).Take(pageSize);
            return medicines;
        }

        private void SetSessionMedicines(string sessionMedicines) {
            _medicine.MedicineName = sessionMedicines.Split(':')[0];
        }
    }
}