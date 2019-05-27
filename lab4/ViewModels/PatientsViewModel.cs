using System.Linq;
using lab4.Models;

namespace lab4.ViewModels
{
    public class PatientsViewModel
    {
        public Patient PatientViewModel { get; set; }
        public IQueryable<Patient> PageViewModel { get; set; }
        public int PageNumber { get; set; }     
    }
}
