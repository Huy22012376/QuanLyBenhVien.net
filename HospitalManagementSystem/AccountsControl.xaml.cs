using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalManagementSystem
{
    public partial class AccountsControl : UserControl
    {
        private HospitalContext _context = new HospitalContext();
        private User _selectedUser = null;

        public AccountsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAccountsList();
        }

        // Load danh sách tài khoản
        private void LoadAccountsList()
        {
            dgAccounts.ItemsSource = _context.Users.ToList();
        }

        // Clear form
        private void ClearAccountForm()
        {
            txtAccountUsername.Clear();
            txtAccountPassword.Clear();
            txtAccountFullName.Clear();
            cmbAccountRole.SelectedIndex = -1;
            _selectedUser = null;
            dgAccounts.SelectedItem = null;
        }

        // Selection changed
        private void dgAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedUser = dgAccounts.SelectedItem as User;
            if (_selectedUser != null)
            {
                txtAccountUsername.Text = _selectedUser.Username;
                txtAccountPassword.Password = _selectedUser.Password;
                txtAccountFullName.Text = _selectedUser.FullName;

                foreach (ComboBoxItem item in cmbAccountRole.Items)
                {
                    if (item.Content.ToString() == _selectedUser.Role)
                    {
                        cmbAccountRole.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        // Thêm tài khoản
        private void btnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAccountUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAccountPassword.Password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra tên đăng nhập đã tồn tại
                var existingUser = _context.Users.FirstOrDefault(u => u.Username == txtAccountUsername.Text.Trim());
                if (existingUser != null)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string selectedRole = (cmbAccountRole.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "User";

                var newUser = new User
                {
                    Username = txtAccountUsername.Text.Trim(),
                    Password = txtAccountPassword.Password, // Lưu ý: Nên hash password trong thực tế
                    FullName = txtAccountFullName.Text.Trim(),
                    Role = selectedRole
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show("Thêm tài khoản thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadAccountsList();
                ClearAccountForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cập nhật tài khoản
        private void btnUpdateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để sửa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtAccountUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAccountPassword.Password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string selectedRole = (cmbAccountRole.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "User";

                _selectedUser.Username = txtAccountUsername.Text.Trim();
                _selectedUser.Password = txtAccountPassword.Password;
                _selectedUser.FullName = txtAccountFullName.Text.Trim();
                _selectedUser.Role = selectedRole;

                _context.SaveChanges();

                MessageBox.Show("Cập nhật tài khoản thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadAccountsList();
                ClearAccountForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xóa tài khoản
        private void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để xóa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa tài khoản '{_selectedUser.Username}' không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Users.Remove(_selectedUser);
                    _context.SaveChanges();

                    MessageBox.Show("Xóa tài khoản thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadAccountsList();
                    ClearAccountForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}