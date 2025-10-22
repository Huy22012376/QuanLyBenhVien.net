using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalManagementSystem
{
    public partial class DoctorsControl : UserControl
    {
        private HospitalContext _context = new HospitalContext();
        private Doctor _selectedDoctor = null;

        public DoctorsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDepartments();
            LoadDoctorsList();
        }

        // Load departments
        private void LoadDepartments()
        {
            cmbDoctorDepartment.ItemsSource = _context.Departments.ToList();
        }

        // Load danh sách bác sĩ
        private void LoadDoctorsList()
        {
            dgDoctors.ItemsSource = _context.Doctors
                .Include(d => d.Department)
                .ToList();
        }

        // Clear form
        private void ClearDoctorForm()
        {
            txtDoctorFullName.Clear();
            dpDoctorDateOfBirth.SelectedDate = null;
            cmbDoctorGender.SelectedIndex = -1;
            txtDoctorPhone.Clear();
            txtDoctorEmail.Clear();
            txtDoctorAddress.Clear();
            cmbDoctorDepartment.SelectedIndex = -1;
            cmbDoctorDegree.SelectedIndex = -1;
            txtDoctorExperience.Clear();
            _selectedDoctor = null;
            dgDoctors.SelectedItem = null;
        }

        // Selection changed
        private void dgDoctors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedDoctor = dgDoctors.SelectedItem as Doctor;
            if (_selectedDoctor != null)
            {
                txtDoctorFullName.Text = _selectedDoctor.FullName;
                dpDoctorDateOfBirth.SelectedDate = _selectedDoctor.DateOfBirth;

                foreach (ComboBoxItem item in cmbDoctorGender.Items)
                {
                    if (item.Content.ToString() == _selectedDoctor.Gender)
                    {
                        cmbDoctorGender.SelectedItem = item;
                        break;
                    }
                }

                txtDoctorPhone.Text = _selectedDoctor.PhoneNumber;
                txtDoctorEmail.Text = _selectedDoctor.Email;
                txtDoctorAddress.Text = _selectedDoctor.Address;
                cmbDoctorDepartment.SelectedValue = _selectedDoctor.DepartmentId;

                foreach (ComboBoxItem item in cmbDoctorDegree.Items)
                {
                    if (item.Content.ToString() == _selectedDoctor.Degree)
                    {
                        cmbDoctorDegree.SelectedItem = item;
                        break;
                    }
                }

                txtDoctorExperience.Text = _selectedDoctor.YearsOfExperience?.ToString() ?? "";
            }
        }

        // Thêm bác sĩ
        private void btnAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtDoctorFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên bác sĩ.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpDoctorDateOfBirth.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày sinh.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbDoctorGender.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn giới tính.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string selectedGender = (cmbDoctorGender.SelectedItem as ComboBoxItem)?.Content.ToString();
                string selectedDegree = (cmbDoctorDegree.SelectedItem as ComboBoxItem)?.Content.ToString();

                int experience = 0;
                if (!string.IsNullOrWhiteSpace(txtDoctorExperience.Text))
                {
                    if (!int.TryParse(txtDoctorExperience.Text, out experience))
                    {
                        MessageBox.Show("Số năm kinh nghiệm phải là số nguyên.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                var newDoctor = new Doctor
                {
                    FullName = txtDoctorFullName.Text.Trim(),
                    DateOfBirth = dpDoctorDateOfBirth.SelectedDate.Value,
                    Gender = selectedGender,
                    PhoneNumber = txtDoctorPhone.Text.Trim(),
                    Email = txtDoctorEmail.Text.Trim(),
                    Address = txtDoctorAddress.Text.Trim(),
                    DepartmentId = (int?)cmbDoctorDepartment.SelectedValue,
                    Degree = selectedDegree,
                    YearsOfExperience = experience
                };

                _context.Doctors.Add(newDoctor);
                _context.SaveChanges();

                MessageBox.Show("Thêm bác sĩ thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadDoctorsList();
                ClearDoctorForm();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                var errorMessages = dbEx.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("\n", errorMessages);
                MessageBox.Show("Vui lòng kiểm tra lại dữ liệu nhập:\n" + fullErrorMessage,
                    "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cập nhật bác sĩ
        private void btnUpdateDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDoctor == null)
            {
                MessageBox.Show("Vui lòng chọn một bác sĩ để sửa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtDoctorFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên bác sĩ.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpDoctorDateOfBirth.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày sinh.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbDoctorGender.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn giới tính.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string selectedGender = (cmbDoctorGender.SelectedItem as ComboBoxItem)?.Content.ToString();
                string selectedDegree = (cmbDoctorDegree.SelectedItem as ComboBoxItem)?.Content.ToString();

                int experience = 0;
                if (!string.IsNullOrWhiteSpace(txtDoctorExperience.Text))
                {
                    if (!int.TryParse(txtDoctorExperience.Text, out experience))
                    {
                        MessageBox.Show("Số năm kinh nghiệm phải là số nguyên.", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                _selectedDoctor.FullName = txtDoctorFullName.Text.Trim();
                _selectedDoctor.DateOfBirth = dpDoctorDateOfBirth.SelectedDate.Value;
                _selectedDoctor.Gender = selectedGender;
                _selectedDoctor.PhoneNumber = txtDoctorPhone.Text.Trim();
                _selectedDoctor.Email = txtDoctorEmail.Text.Trim();
                _selectedDoctor.Address = txtDoctorAddress.Text.Trim();
                _selectedDoctor.DepartmentId = (int?)cmbDoctorDepartment.SelectedValue;
                _selectedDoctor.Degree = selectedDegree;
                _selectedDoctor.YearsOfExperience = experience;

                _context.SaveChanges();

                MessageBox.Show("Cập nhật thông tin bác sĩ thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadDoctorsList();
                ClearDoctorForm();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                var errorMessages = dbEx.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("\n", errorMessages);
                MessageBox.Show("Vui lòng kiểm tra lại dữ liệu nhập:\n" + fullErrorMessage,
                    "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xóa bác sĩ
        private void btnDeleteDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDoctor == null)
            {
                MessageBox.Show("Vui lòng chọn một bác sĩ để xóa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa bác sĩ '{_selectedDoctor.FullName}' không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Doctors.Remove(_selectedDoctor);
                    _context.SaveChanges();

                    MessageBox.Show("Xóa bác sĩ thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadDoctorsList();
                    ClearDoctorForm();
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