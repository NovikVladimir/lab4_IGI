using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using lab4.Data;
using lab4.Models;
using lab4.ViewModels;
using lab4.Filters;

namespace lab4.Controllers
{
    [CatchExceptionFilter]
    public class TreatmentController : Controller
    {
        private int pageSize = 5;
        private Context db;
        private Treatment _treatment = new Treatment
        {
            TreatmentDisease = "",
            TreatmentMedication = "",
            TreatmentDate = "",
            TreatmentDosage = "",
            TreatmentDurationOfTreatment = ""
        };

        public TreatmentController(Context TreatmentContext)
        {
            db = TreatmentContext;
        }

        [HttpGet]
        public IActionResult Index(SortState sortOrder = SortState.No, int index = 0)
        {
            Treatment sessionTreatment = HttpContext.Session.GetObject<Treatment>("Treatment");
            string sessionSortState = HttpContext.Session.GetString("SortStateTreatment");
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

            if (sessionTreatment != null)
            {
                _treatment = sessionTreatment;
            }

            if (sessionSortState != null)
                if (sortOrder == SortState.No)
                    sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState);

            ViewData["NameSort"] = sortOrder == SortState.NameDesc ? SortState.NameAsc : SortState.NameDesc;
            HttpContext.Session.SetString("SortState", sortOrder.ToString());
            IQueryable<Treatment> Treatments = Sort(db.Treatments, sortOrder,
                _treatment.TreatmentDisease, (int)page);
            TreatmentsViewModel TreatmentsView = new TreatmentsViewModel
            {
                TreatmentViewModel = _treatment,
                PageViewModel = Treatments,
                PageNumber = (int)page
            };

            return View(TreatmentsView);
        }

        [HttpPost]
        public IActionResult Index(Treatment treatment)
        {
            var sessionSortState = HttpContext.Session.GetString("SortStateTreatment");
            SortState sortOrder = new SortState();
            if (sessionSortState != null)
                sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState);

            int? page = HttpContext.Session.GetInt32("Page");
            if (page == null)
            {
                page = 0;
                HttpContext.Session.SetInt32("Page", 0);
            }

            IQueryable<Treatment> treatments = Sort(db.Treatments, sortOrder,
                treatment.TreatmentDisease, (int)page);
            HttpContext.Session.SetObject("Treatment", treatment);

            TreatmentsViewModel treatmentsView = new TreatmentsViewModel
            {
                TreatmentViewModel = treatment,
                PageViewModel = treatments,
                PageNumber = (int)page
            };

            return View(treatmentsView);
        }

        private IQueryable<Treatment> Sort(IQueryable<Treatment> Treatments,
            SortState sortOrder, string name, int page)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    Treatments = Treatments.OrderBy(s => s.TreatmentDisease);
                    break;
                case SortState.NameDesc:
                    Treatments = Treatments.OrderByDescending(s => s.TreatmentDisease);
                    break;
            }
            Treatments = Treatments.Where(o => o.TreatmentDisease.Contains(name ?? ""))
                .Skip(page * pageSize).Take(pageSize);
            return Treatments;
        }
    }
}