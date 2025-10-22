using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public class Prescription
        //=========================================  ĐƠN THUỐC ===========================
    {
        [Key]
        public int PrescriptionId { get; set; }

        public int? RecordId { get; set; }
        [ForeignKey("RecordId")]
        public virtual MedicalRecord MedicalRecord { get; set; }

        public int? ProgressId { get; set; }
        [ForeignKey("ProgressId")]
        public virtual TreatmentProgress TreatmentProgress { get; set; }

        public DateTime? PrescriptionDate { get; set; }

        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime? CreatedDate { get; set; }

        // Navigation property
        public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; }
    }
}