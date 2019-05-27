using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lab4.Models
{
    public class Disease
    {
        [Key]
        public int DiseaseID { get; set; }
        public string DiseaseName { get; set; }
        public string DiseaseSymptoms { get; set; }
        public string DiseaseDuration { get; set; }
        public string DiseaseConsequences { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }

        public Disease() { }

        public Disease(int DiseaseID, string DiseaseName, string DiseaseSymptoms, string DiseaseDuration, string DiseaseConsequences,
            int? medicineID)
        {
            this.DiseaseID = DiseaseID;
            this.DiseaseName = DiseaseName;
            this.DiseaseSymptoms = DiseaseSymptoms;
            this.DiseaseDuration = DiseaseDuration;
            this.DiseaseConsequences = DiseaseConsequences;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Disease;

            if (obj == null)
            {
                return false;
            }
            if (obj == this)
            {
                return true;
            }

            return this.DiseaseID == item.DiseaseID;
        }

        public override int GetHashCode()
        {
            return this.DiseaseID.GetHashCode();
        }
    }
}
