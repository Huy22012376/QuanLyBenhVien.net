using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    // Lớp User đại diện cho bảng Users trong CSDL - quản lý tài khoản đăng nhập
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
        [StringLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có từ 6-100 ký tự.")]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string Role { get; set; } // Vai trò: Admin, Doctor, Nurse, Receptionist
    }
}