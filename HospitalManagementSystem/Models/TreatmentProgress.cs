using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public class TreatmentProgress
        //==================================== TIẾN TRÌNH ĐIỀU TRỊ ====================================
    {
        [Key]
        public int ProgressId { get; set; }

        public int? RecordId { get; set; }
        [ForeignKey("RecordId")]
        public virtual MedicalRecord MedicalRecord { get; set; }

        public DateTime? VisitDate { get; set; }

        [StringLength(500)]
        public string Symptoms { get; set; }

        [StringLength(500)]
        public string Diagnosis { get; set; }

        public string Treatment { get; set; }

        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        public string Notes { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}