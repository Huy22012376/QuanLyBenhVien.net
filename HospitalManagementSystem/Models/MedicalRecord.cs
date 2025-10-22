using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public class MedicalRecord
        //============================================== HỒ SƠ Y TẾ =================================================
    {
        [Key]
        public int RecordId { get; set; }

        [StringLength(50)]
        public string RecordCode { get; set; }

        public int? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        public DateTime? AdmissionDate { get; set; }

        [StringLength(500)]
        public string InitialDiagnosis { get; set; }

        public string TreatmentPlan { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public DateTime? DischargeDate { get; set; }

        [StringLength(500)]
        public string FinalDiagnosis { get; set; }

        [StringLength(500)]
        public string TreatmentResult { get; set; }

        public string Notes { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<TreatmentProgress> TreatmentProgresses { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}