using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using lab4.Data;
using lab4.Models;
using lab4.ViewModels;
using lab4.Filters;
using Newtonsoft.Json;

namespace lab4.Controllers
{
    [CatchExceptionFilter]
    public class DiseaseController : Controller
    {
        private int pageSize = 5;
        private Context db;
        private Disease _disease = new Disease
        {
            DiseaseName = "",
            DiseaseSymptoms = "",
            DiseaseDuration = "",
            DiseaseConsequences = ""
    };

        public DiseaseController(Context diseaseContext)
        {
            db = diseaseContext;
        }

        [HttpGet]
        public IActionResult Index(SortState sortOrder = SortState.No, int index = 0)
        {
            Disease sessionDisease = HttpContext.Session.GetObject<Disease>("Disease");
            string sessionSortState = HttpContext.Session.GetString("SortStateDisease");
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

            if (sessionDisease != null)
            {
                _disease = sessionDisease;
            }

            if (sessionSortState != null)
                if (sortOrder == SortState.No)
                    sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState);

            ViewData["NameSort"] = sortOrder == SortState.NameDesc ? SortState.NameAsc : SortState.NameDesc;
            HttpContext.Session.SetString("SortStateDisease", sortOrder.ToString());

            IQueryable<Disease> diseases = Sort(db.Diseases, sortOrder,
                _disease.DiseaseName, (int)page);
            DiseasesViewModel diseasesView = new DiseasesViewModel
            {
                DiseaseViewModel = _disease,
                PageViewModel = diseases,
                PageNumber = (int)page
            };

            return View(diseasesView);
        }

        [HttpPost]
        public IActionResult Index(Disease disease)
        {
            var sessionSortState = HttpContext.Session.GetString("SortStateDisease");
            SortState sortOrder = new SortState();
            if (sessionSortState != null)
                sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState);

            int? page = HttpContext.Session.GetInt32("Page");
            if (page == null)
            {
                page = 0;
                HttpContext.Session.SetInt32("Page", 0);
            }

            IQueryable<Disease> diseases = Sort(db.Diseases, sortOrder,
                disease.DiseaseName, (int)page);
            HttpContext.Session.SetObject("Disease", disease);

            DiseasesViewModel diseasesView = new DiseasesViewModel
            {
                DiseaseViewModel = disease,
                PageViewModel = diseases,
                PageNumber = (int)page
            };

            return View(diseasesView);
        }

        private IQueryable<Disease> Sort(IQueryable<Disease> diseases,
            SortState sortOrder, string name, int page)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    diseases = diseases.OrderBy(s => s.DiseaseName);
                    break;
                case SortState.NameDesc:
                    diseases = diseases.OrderByDescending(s => s.DiseaseName);
                    break;
            }
            diseases = diseases.Where(o => o.DiseaseName.Contains(name ?? ""))
                .Skip(page * pageSize).Take(pageSize);
            return diseases;
        }
    }
}