using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lab4.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public string PatientGender { get; set; }
        public string PatientAdress { get; set; }
        public string PatientTelephone { get; set; }
        public string PatientDateOfHospitalization { get; set; }
        public string PatientDischargeDate { get; set; }
        public string PatientDisease { get; set; }
        public string PatientDepartment { get; set; }
        public string PatientAttendingPhysician { get; set; }
        public string PatientResultOfTreatment { get; set; }
        public int? DiseaseID { get; set; }
        public virtual Disease Disease { get; set; }

        public Patient() { }

        public Patient(int PatientID, string PatientName, int PatientAge, string PatientGender, string PatientAdress,
            string PatientTelephone, string PatientDateOfHospitalization, string PatientDischargeDate, string PatientDisease,
            string PatientDepartment, string PatientAttendingPhysician, string PatientResultOfTreatment, int? diseaseID)
        {
            this.PatientID = PatientID;
            this.PatientName = PatientName;
            this.PatientAge = PatientAge;
            this.PatientGender = PatientGender;
            this.PatientAdress = PatientAdress;
            this.PatientTelephone = PatientTelephone;
            this.PatientDateOfHospitalization = PatientDateOfHospitalization;
            this.PatientDischargeDate = PatientDischargeDate;
            this.PatientDisease = PatientDisease;
            this.PatientDepartment = PatientDepartment;
            this.PatientAttendingPhysician = PatientAttendingPhysician;
            this.PatientResultOfTreatment = PatientResultOfTreatment;
            this.DiseaseID = diseaseID;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Patient;

            if (obj == null)
            {
                return false;
            }
            if (obj == this)
            {
                return true;
            }

            return this.PatientID == item.PatientID;
        }

        public override int GetHashCode()
        {
            return this.PatientID.GetHashCode();
        }
    }
}
