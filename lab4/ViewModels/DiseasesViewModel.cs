using System.Linq;
using lab4.Models;

namespace lab4.ViewModels
{
    public enum SortState
    {
        No,
        NameAsc,
        NameDesc,
    }
    public class DiseasesViewModel
    {
        public Disease DiseaseViewModel { get; set; }
        public IQueryable<Disease> PageViewModel { get; set; }
        public int PageNumber { get; set; }
    }
}
