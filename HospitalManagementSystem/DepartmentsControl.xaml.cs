using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalManagementSystem
{
    public partial class DepartmentsControl : UserControl
    {
        private HospitalContext _context = new HospitalContext();
        private Department _selectedDepartment = null;

        public DepartmentsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDepartmentsList();
        }

        // Load danh sách khoa
        private void LoadDepartmentsList()
        {
            dgDepartments.ItemsSource = _context.Departments.ToList();
        }

        // Clear form
        private void ClearDepartmentForm()
        {
            txtDepartmentName.Clear();
            txtDepartmentDescription.Clear();
            _selectedDepartment = null;
            dgDepartments.SelectedItem = null;
        }

        // Selection changed
        private void dgDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedDepartment = dgDepartments.SelectedItem as Department;
            if (_selectedDepartment != null)
            {
                txtDepartmentName.Text = _selectedDepartment.DepartmentName;
                txtDepartmentDescription.Text = _selectedDepartment.Description;
            }
        }

        // Thêm khoa
        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khoa.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newDepartment = new Department
                {
                    DepartmentName = txtDepartmentName.Text.Trim(),
                    Description = txtDepartmentDescription.Text.Trim()
                };

                _context.Departments.Add(newDepartment);
                _context.SaveChanges();

                MessageBox.Show("Thêm khoa thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadDepartmentsList();
                ClearDepartmentForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cập nhật khoa
        private void btnUpdateDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDepartment == null)
            {
                MessageBox.Show("Vui lòng chọn một khoa để sửa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khoa.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectedDepartment.DepartmentName = txtDepartmentName.Text.Trim();
                _selectedDepartment.Description = txtDepartmentDescription.Text.Trim();

                _context.SaveChanges();

                MessageBox.Show("Cập nhật khoa thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadDepartmentsList();
                ClearDepartmentForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xóa khoa
        private void btnDeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDepartment == null)
            {
                MessageBox.Show("Vui lòng chọn một khoa để xóa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Kiểm tra xem khoa có bệnh nhân không
            var hasPatients = _context.Patients.Any(p => p.DepartmentId == _selectedDepartment.DepartmentId);
            if (hasPatients)
            {
                MessageBox.Show("Không thể xóa khoa này vì đang có bệnh nhân!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra xem khoa có bác sĩ không
            var hasDoctors = _context.Doctors.Any(d => d.DepartmentId == _selectedDepartment.DepartmentId);
            if (hasDoctors)
            {
                MessageBox.Show("Không thể xóa khoa này vì đang có bác sĩ!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra xem khoa có phòng khám không
            var hasRooms = _context.ClinicRooms.Any(r => r.DepartmentId == _selectedDepartment.DepartmentId);
            if (hasRooms)
            {
                MessageBox.Show("Không thể xóa khoa này vì đang có phòng khám!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khoa '{_selectedDepartment.DepartmentName}' không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Departments.Remove(_selectedDepartment);
                    _context.SaveChanges();

                    MessageBox.Show("Xóa khoa thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadDepartmentsList();
                    ClearDepartmentForm();
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