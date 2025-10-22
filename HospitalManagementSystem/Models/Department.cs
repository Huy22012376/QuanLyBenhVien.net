using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    // Lớp Department đại diện cho bảng Departments trong CSDL
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Tên khoa không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên khoa không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên khoa")]
        public string DepartmentName { get; set; }

        [StringLength(200)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        // Navigation Property: Một khoa có nhiều bệnh nhân
        public virtual ICollection<Patient> Patients { get; set; }
    }
}