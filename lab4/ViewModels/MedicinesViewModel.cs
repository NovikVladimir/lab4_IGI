using System.Linq;
using lab4.Models;

namespace lab4.ViewModels
{
    public class MedicinesViewModel
    {
        public Medicine MedicineViewModel { get; set; }
        public IQueryable<Medicine> PageViewModel { get; set; }
        public int PageNumber { get; set; }
    }
}
