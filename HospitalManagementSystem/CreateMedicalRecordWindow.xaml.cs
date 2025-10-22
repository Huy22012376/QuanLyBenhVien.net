using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System;
using System.Linq;
using System.Windows;

namespace HospitalManagementSystem
{
    public partial class CreateMedicalRecordWindow : Window
    {
        private HospitalContext _context = new HospitalContext();
        private Patient _patient;
        public bool IsSuccess { get; private set; } = false;

        public CreateMedicalRecordWindow(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPatientInfo();
            LoadDoctors();
            GenerateRecordCode();
        }

        // Load thông tin bệnh nhân
        private void LoadPatientInfo()
        {
            if (_patient != null)
            {
                txtPatientInfo.Text = $"Bệnh nhân: {_patient.FullName} - SĐT: {_patient.PhoneNumber}";
                txtPatientName.Text = _patient.FullName;
                txtDateOfBirth.Text = _patient.DateOfBirth.ToString("dd/MM/yyyy");
                txtGender.Text = _patient.Gender;
                txtPhoneNumber.Text = _patient.PhoneNumber;
            }
        }

        // Load danh sách bác sĩ
        private void LoadDoctors()
        {
            cmbDoctor.ItemsSource = _context.Doctors.ToList();
        }

        // Tự động tạo mã sổ bệnh án
        private void GenerateRecordCode()
        {
            // Đếm số sổ bệnh án hiện có
            int count = _context.MedicalRecords.Count();

            // Tạo mã theo format: BA + năm + số thứ tự (6 chữ số)
            string year = DateTime.Now.Year.ToString();
            string number = (count + 1).ToString("D6"); // D6 = 6 chữ số, có số 0 đứng trước

            txtRecordCode.Text = $"BA{year}{number}";
            txtRecordCode.IsReadOnly = true;
            txtRecordCode.Background = System.Windows.Media.Brushes.LightGray;
        }

        // Validation
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtRecordCode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sổ bệnh án!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtRecordCode.Focus();
                return false;
            }

            if (cmbDoctor.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bác sĩ điều trị!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbDoctor.Focus();
                return false;
            }

            if (dpAdmissionDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày nhập viện!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                dpAdmissionDate.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtInitialDiagnosis.Text))
            {
                MessageBox.Show("Vui lòng nhập chẩn đoán ban đầu!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtInitialDiagnosis.Focus();
                return false;
            }

            // Kiểm tra mã sổ BA đã tồn tại chưa
            bool exists = _context.MedicalRecords.Any(m => m.RecordCode == txtRecordCode.Text);
            if (exists)
            {
                MessageBox.Show("Mã sổ bệnh án đã tồn tại!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        // Nút Tạo sổ bệnh án
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                var newRecord = new MedicalRecord
                {
                    RecordCode = txtRecordCode.Text.Trim(),
                    PatientId = _patient.PatientId,
                    DoctorId = (int?)cmbDoctor.SelectedValue,
                    AdmissionDate = dpAdmissionDate.SelectedDate,
                    InitialDiagnosis = txtInitialDiagnosis.Text.Trim(),
                    TreatmentPlan = txtTreatmentPlan.Text.Trim(),
                    Status = (cmbStatus.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString(),
                    Notes = txtNotes.Text.Trim(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                _context.MedicalRecords.Add(newRecord);
                _context.SaveChanges();

                MessageBox.Show(
                    $"Tạo sổ bệnh án thành công!\n\nMã SBA: {newRecord.RecordCode}\nBệnh nhân: {_patient.FullName}",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                IsSuccess = true;
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Nút Hủy
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn hủy bỏ?\nDữ liệu đã nhập sẽ không được lưu.",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                this.DialogResult = false;
                this.Close();
            }
        }
    }
}