using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System.Linq;
using System.Windows;

namespace HospitalManagementSystem
{
    public partial class RegisterWindow : Window
    {
        private HospitalContext _context = new HospitalContext();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện click nút "Đăng ký"
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy thông tin từ form
                string username = txtUsername.Text.Trim();
                string fullName = txtFullName.Text.Trim();
                string password = txtPassword.Password;
                string confirmPassword = txtConfirmPassword.Password;

                // Kiểm tra tính hợp lệ của dữ liệu nhập
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Vui lòng nhập tên tài khoản.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (username.Length < 3)
                {
                    MessageBox.Show("Tên tài khoản phải có ít nhất 3 ký tự.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPassword.Focus();
                    return;
                }

                if (password.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPassword.Focus();
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Mật khẩu nhập lại không khớp!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtConfirmPassword.Clear();
                    txtConfirmPassword.Focus();
                    return;
                }

                // Kiểm tra xem tên tài khoản đã tồn tại chưa
                var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
                if (existingUser != null)
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại. Vui lòng chọn tên khác.",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtUsername.Focus();
                    return;
                }

                // Tạo đối tượng User mới
                var newUser = new User
                {
                    Username = username,
                    Password = password, // Lưu ý: Trong thực tế nên mã hóa mật khẩu (hash)
                    FullName = string.IsNullOrWhiteSpace(fullName) ? username : fullName,
                    Role = "User" // Vai trò mặc định
                };

                // Thêm vào CSDL
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Thông báo thành công
                MessageBox.Show("Đăng ký tài khoản thành công!\nBạn có thể đăng nhập ngay bây giờ.",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Quay lại trang đăng nhập
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                // Xử lý lỗi validation từ Entity Framework
                var errorMessages = dbEx.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("\n", errorMessages);
                MessageBox.Show("Vui lòng kiểm tra lại dữ liệu nhập:\n" + fullErrorMessage,
                    "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sự kiện click nút "Quay lại đăng nhập"
        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ đăng nhập và đóng cửa sổ đăng ký
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}