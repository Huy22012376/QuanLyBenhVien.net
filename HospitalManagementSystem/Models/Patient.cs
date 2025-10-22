using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    // Lớp Patient đại diện cho bảng Patients trong CSDL
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Họ và tên bệnh nhân không được để trống.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống.")]
        [StringLength(10)]
        [Display(Name = "Giới tính")]
        public string Gender { get; set; } // Nam, Nữ, Khác

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [StringLength(15)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        // Khóa ngoại đến bảng Departments
        [Display(Name = "Khoa khám")]
        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [StringLength(500)]
        [Display(Name = "Triệu chứng")]
        public string Symptoms { get; set; }

        [Display(Name = "Ngày khám")]
        [DataType(DataType.Date)]
        public DateTime? AdmissionDate { get; set; }
    }
}