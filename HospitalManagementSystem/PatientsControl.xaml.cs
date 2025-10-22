using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalManagementSystem
{
    public partial class PatientsControl : UserControl
    {
        private HospitalContext _context = new HospitalContext();
        private Patient _selectedPatient = null;

        public PatientsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDepartments();
            LoadPatients();
            dpAdmissionDate.SelectedDate = DateTime.Now;
        }

        // Load departments
        private void LoadDepartments()
        {
            cmbDepartment.ItemsSource = _context.Departments.ToList();
        }

        // Load danh sách bệnh nhân
        private void LoadPatients()
        {
            dgPatients.ItemsSource = _context.Patients
                .Include(p => p.Department)
                .ToList();
        }

        // Clear form
        private void ClearPatientForm()
        {
            txtFullName.Clear();
            dpDateOfBirth.SelectedDate = null;
            cmbGender.SelectedIndex = -1;
            txtPhoneNumber.Clear();
            txtAddress.Clear();
            cmbDepartment.SelectedIndex = -1;
            txtSymptoms.Clear();
            dpAdmissionDate.SelectedDate = DateTime.Now;
            _selectedPatient = null;
            dgPatients.SelectedItem = null;
        }

        // Selection changed
        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPatient = dgPatients.SelectedItem as Patient;
            if (_selectedPatient != null)
            {
                txtFullName.Text = _selectedPatient.FullName;
                dpDateOfBirth.SelectedDate = _selectedPatient.DateOfBirth;

                foreach (ComboBoxItem item in cmbGender.Items)
                {
                    if (item.Content.ToString() == _selectedPatient.Gender)
                    {
                        cmbGender.SelectedItem = item;
                        break;
                    }
                }

                txtPhoneNumber.Text = _selectedPatient.PhoneNumber;
                txtAddress.Text = _selectedPatient.Address;
                cmbDepartment.SelectedValue = _selectedPatient.DepartmentId;
                txtSymptoms.Text = _selectedPatient.Symptoms;
                dpAdmissionDate.SelectedDate = _selectedPatient.AdmissionDate;
            }
        }

        // Thêm bệnh nhân
        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên bệnh nhân.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpDateOfBirth.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày sinh.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbGender.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn giới tính.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string selectedGender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString();

                var newPatient = new Patient
                {
                    FullName = txtFullName.Text.Trim(),
                    DateOfBirth = dpDateOfBirth.SelectedDate.Value,
                    Gender = selectedGender,
                    PhoneNumber = txtPhoneNumber.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    DepartmentId = (int?)cmbDepartment.SelectedValue,
                    Symptoms = txtSymptoms.Text.Trim(),
                    AdmissionDate = dpAdmissionDate.SelectedDate
                };

                _context.Patients.Add(newPatient);
                _context.SaveChanges();

                MessageBox.Show("Thêm bệnh nhân thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadPatients();
                ClearPatientForm();
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

        // Cập nhật bệnh nhân
        private void btnUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Vui lòng chọn một bệnh nhân để sửa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên bệnh nhân.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpDateOfBirth.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày sinh.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbGender.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn giới tính.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string selectedGender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString();

                _selectedPatient.FullName = txtFullName.Text.Trim();
                _selectedPatient.DateOfBirth = dpDateOfBirth.SelectedDate.Value;
                _selectedPatient.Gender = selectedGender;
                _selectedPatient.PhoneNumber = txtPhoneNumber.Text.Trim();
                _selectedPatient.Address = txtAddress.Text.Trim();
                _selectedPatient.DepartmentId = (int?)cmbDepartment.SelectedValue;
                _selectedPatient.Symptoms = txtSymptoms.Text.Trim();
                _selectedPatient.AdmissionDate = dpAdmissionDate.SelectedDate;

                _context.SaveChanges();

                MessageBox.Show("Cập nhật thông tin bệnh nhân thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadPatients();
                ClearPatientForm();
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

        // Xóa bệnh nhân
        private void btnDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Vui lòng chọn một bệnh nhân để xóa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa bệnh nhân '{_selectedPatient.FullName}' không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Patients.Remove(_selectedPatient);
                    _context.SaveChanges();

                    MessageBox.Show("Xóa bệnh nhân thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadPatients();
                    ClearPatientForm();
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