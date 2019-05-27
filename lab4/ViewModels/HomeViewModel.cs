using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using lab4.Models;

namespace lab4.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Disease> Diseases { get; set; }
        public IEnumerable<Medicine> Medicines { get; set; }
        public IEnumerable<Patient> Patients { get; set; }
        public IEnumerable<Treatment> Treatments { get; set; }
    }
}
