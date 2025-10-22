using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public class PrescriptionDetail
        //========================================= CHI TIẾT ĐƠN THUỐC ============================
    {
        [Key]
        public int DetailId { get; set; }

        public int? PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }

        [StringLength(200)]
        public string MedicineName { get; set; }

        [StringLength(100)]
        public string Dosage { get; set; }

        [StringLength(100)]
        public string Frequency { get; set; }

        [StringLength(100)]
        public string Duration { get; set; }

        [StringLength(500)]
        public string Instructions { get; set; }

        public int? Quantity { get; set; }
    }
}