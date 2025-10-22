using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public class ClinicRoom
    {
        [Key]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Mã phòng không được để trống")]
        [StringLength(20)]
        public string RoomCode { get; set; }

        [Required(ErrorMessage = "Tên phòng không được để trống")]
        [StringLength(100)]
        public string RoomName { get; set; }

        // Không có annotation ở đây
        public int? DepartmentId { get; set; }

        // Đặt [ForeignKey] ở đây, trỏ đến property DepartmentId
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [StringLength(20)]
        public string Floor { get; set; }

        [StringLength(50)]
        public string RoomType { get; set; }

        public int? Capacity { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Equipment { get; set; }

        public bool IsAvailable { get; set; }
    }
}