﻿using System;
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
    public class PatientController : Controller
    {
        private int pageSize = 5;
        private Context db;
        private Patient _patient = new Patient
        {
            PatientName = "",
            PatientDisease = "",
            PatientAttendingPhysician = ""
        };

        public PatientController(Context PatientContext)
        {
            db = PatientContext;
        }

        [HttpGet]
        public IActionResult Index(SortState sortOrder = SortState.No, int index = 0)
        {
            Patient sessionPatient = HttpContext.Session.GetObject<Patient>("Patient");
            string sessionSortState = HttpContext.Session.GetString("SortStatePatient");
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

            if (sessionPatient != null)
            {
                _patient = sessionPatient;
            }

            if (sessionSortState != null)
                if (sortOrder == SortState.No)
                    sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState);

            ViewData["NameSort"] = sortOrder == SortState.NameDesc ? SortState.NameAsc : SortState.NameDesc;
            HttpContext.Session.SetString("SortState", sortOrder.ToString());
            IQueryable<Patient> Patients = Sort(db.Patients, sortOrder,
                _patient.PatientName, (int)page);
            PatientsViewModel PatientsView = new PatientsViewModel
            {
                PatientViewModel = _patient,
                PageViewModel = Patients,
                PageNumber = (int)page
            };

            return View(PatientsView);
        }

        [HttpPost]
        public IActionResult Index(Patient patient)
        {
            var sessionSortState = HttpContext.Session.GetString("SortStatePatient");
            SortState sortOrder = new SortState();
            if (sessionSortState != null)
                sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState);

            int? page = HttpContext.Session.GetInt32("Page");
            if (page == null)
            {
                page = 0;
                HttpContext.Session.SetInt32("Page", 0);
            }

            IQueryable<Patient> patients = Sort(db.Patients, sortOrder,
                 patient.PatientName, (int)page);
            HttpContext.Session.SetObject("Patient", patient);

            PatientsViewModel patientsView = new PatientsViewModel
            {
                PatientViewModel = patient,
                PageViewModel = patients,
                PageNumber = (int)page
            };

            return View(patientsView);
        }

        private IQueryable<Patient> Sort(IQueryable<Patient> Patients,
            SortState sortOrder, string name, int page)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    Patients = Patients.OrderBy(s => s.PatientName);
                    break;
                case SortState.NameDesc:
                    Patients = Patients.OrderByDescending(s => s.PatientName);
                    break;
            }
            Patients = Patients.Where(o => o.PatientName.Contains(name ?? ""))
                .Skip(page * pageSize).Take(pageSize);
            return Patients;
        }

        [HttpGet]
        public IActionResult Add()
        {
            List<Patient> patients = PatientContext.GetPage(0, pageSize);
            return View(patients);
        }

        [HttpPost]
        public string Add(string name, string disease, string attendingPhysician)
        {
            return "Пациент " + name + " с болезнью " + disease + " и врачом " + attendingPhysician + " успешно зарегистрирован";
        }
    }
}