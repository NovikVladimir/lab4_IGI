using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using lab4.Data;
using lab4.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace lab4.Filters
{
    public class DataFilter : Attribute, IActionFilter
    {
        private string type;
        Context db = new Context();
        public DataFilter(string type)
        {
            this.type = type;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (type == "addDisease")
                context.HttpContext.Session.SetString("getDiseases", JsonConvert.SerializeObject(db.Diseases.ToList()));
            if (type == "getDiseases")
            {
                context.HttpContext.Session.SetString("getDiseases", JsonConvert.SerializeObject(db.Diseases.ToList()));
            }
            if (type == "addMedicine")
                context.HttpContext.Session.SetString("getOwners", JsonConvert.SerializeObject(db.Medicines.ToList()));
            if (type == "getMedicines")
            {
                context.HttpContext.Session.SetString("getOwners", JsonConvert.SerializeObject(db.Medicines.ToList()));
            }
            if (type == "addMedicine")
            {
                context.HttpContext.Session.SetString("getMedicines", JsonConvert.SerializeObject(db.Medicines.Select(p => new Medicine() { MedicineName = p.MedicineName, MedicineDosage = p.MedicineManufacturer, MedicinePackaging = p.MedicinePackaging }).ToList()));
                context.HttpContext.Session.SetString("getDiseases", JsonConvert.SerializeObject(db.Diseases.Select(p => new Disease() { DiseaseName = p.DiseaseName, DiseaseSymptoms = p.DiseaseSymptoms, DiseaseDuration = p.DiseaseDuration }).ToList()));
                db.Medicines.ToList();
                db.Diseases.ToList();
                context.HttpContext.Items.Add("getMedicines", db.Medicines.ToList());
            }
            if (type == "getMedicines")
            {
                context.HttpContext.Session.SetString("getMedicines", JsonConvert.SerializeObject(db.Medicines.Select(p => new Medicine() { MedicineName = p.MedicineName, MedicineDosage = p.MedicineManufacturer, MedicinePackaging = p.MedicinePackaging }).ToList()));
                context.HttpContext.Session.SetString("getDiseases", JsonConvert.SerializeObject(db.Diseases.Select(p => new Disease() { DiseaseName = p.DiseaseName, DiseaseSymptoms = p.DiseaseSymptoms, DiseaseDuration = p.DiseaseDuration }).ToList()));
                db.Medicines.ToList();
                db.Diseases.ToList();
                context.HttpContext.Items.Add("getMedicines", db.Medicines.ToList());
            }

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (type == "addDisease" && context.HttpContext.Session.Keys.Contains("allow"))
            {
                db.Diseases.Add(JsonConvert.DeserializeObject<Disease>(context.HttpContext.Session.GetString("addDisease")));
                db.SaveChanges();
            }
            if (type == "addMedicine" && context.HttpContext.Session.Keys.Contains("allow"))
            {
                db.Medicines.Add(JsonConvert.DeserializeObject<Medicine>(context.HttpContext.Session.GetString("addMedicine")));
                db.SaveChanges();
            }
            if (type == "addMedicine" && context.HttpContext.Session.Keys.Contains("allow"))
            {
                Medicine Medicine = JsonConvert.DeserializeObject<Medicine>(context.HttpContext.Session.GetString("Medicine"));
                string Disease = context.HttpContext.Session.GetString("Disease");
                db.Medicines.Add(Medicine);
                db.SaveChanges();
            }
        }

    }
}
