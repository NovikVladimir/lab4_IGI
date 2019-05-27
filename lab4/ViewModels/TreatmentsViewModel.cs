using System.Linq;
using lab4.Models;

namespace lab4.ViewModels
{
    public class TreatmentsViewModel
    {
        public Treatment TreatmentViewModel { get; set; }
        public IQueryable<Treatment> PageViewModel { get; set; }
        public int PageNumber { get; set; }
    }
}
