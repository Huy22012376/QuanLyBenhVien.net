using HospitalManagementSystem.Data;
using System.Linq;
using System.Windows;
using HospitalManagementSystem.Helpers;

namespace HospitalManagementSystem
{
    public partial class LoginWindow : Window
    {
        private HospitalContext _context = new HospitalContext();

        public LoginWindow()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện click nút "Đăng nhập"
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy thông tin từ form
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Password;

                // Kiểm tra dữ liệu nhập
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Vui lòng nhập tên tài khoản.", "Thông báo",
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

                // Kiểm tra thông tin đăng nhập trong CSDL
                var user = _context.Users.FirstOrDefault(u =>
                    u.Username == username && u.Password == password);

                if (user != null)
                {
                    //đăng nhập thành công
                    // Lưu thông tin user vào Session
                    SessionManager.CurrentUser = user;

                    MessageBox.Show($"Đăng nhập thành công!\nXin chào {user.FullName ?? username}!",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    // Đăng nhập thất bại
                    MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng!",
                        "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sự kiện click nút "Đăng ký tài khoản"
        private void btnGoToRegister_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ đăng ký và đóng cửa sổ đăng nhập
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}