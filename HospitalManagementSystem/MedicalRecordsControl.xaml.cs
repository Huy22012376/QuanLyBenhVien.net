using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalManagementSystem
{
    public partial class MedicalRecordsControl : UserControl
    {
        private HospitalContext _context = new HospitalContext();
        private Patient _selectedPatient = null;

        public MedicalRecordsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllMedicalRecords();
        }

        // Tìm kiếm bệnh nhân
        private void txtSearchPatient_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchPatient.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                lstPatients.ItemsSource = null;
                return;
            }

            var patients = _context.Patients
                .Where(p => p.FullName.ToLower().Contains(searchText) ||
                           p.PhoneNumber.Contains(searchText))
                .Take(20)
                .ToList();

            lstPatients.ItemsSource = patients;
        }

        // Chọn bệnh nhân
        private void lstPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPatient = lstPatients.SelectedItem as Patient;
            if (_selectedPatient != null)
            {
                txtPatientInfo.Text = $"Bệnh nhân: {_selectedPatient.FullName} - SĐT: {_selectedPatient.PhoneNumber}";
                btnCreateRecord.IsEnabled = true;

                // Load sổ bệnh án của bệnh nhân này
                LoadPatientMedicalRecords(_selectedPatient.PatientId);
            }
        }

        // Load sổ bệnh án của 1 bệnh nhân
        private void LoadPatientMedicalRecords(int patientId)
        {
            dgMedicalRecords.ItemsSource = _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.AdmissionDate)
                .ToList();
        }

        // Load tất cả sổ bệnh án
        private void LoadAllMedicalRecords()
        {
            dgMedicalRecords.ItemsSource = _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .OrderByDescending(m => m.AdmissionDate)
                .Take(100)
                .ToList();
        }

        // Tạo sổ bệnh án mới
        private void btnCreateRecord_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Mở window popup
            var createWindow = new CreateMedicalRecordWindow(_selectedPatient);
            bool? result = createWindow.ShowDialog();

            // Nếu tạo thành công, reload danh sách
            if (result == true && createWindow.IsSuccess)
            {
                LoadPatientMedicalRecords(_selectedPatient.PatientId);
                MessageBox.Show("Đã tạo sổ bệnh án thành công!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Xem tất cả sổ bệnh án
        private void btnViewAllRecords_Click(object sender, RoutedEventArgs e)
        {
            _selectedPatient = null;
            btnCreateRecord.IsEnabled = false;
            txtPatientInfo.Text = "Tất cả sổ bệnh án";
            LoadAllMedicalRecords();
        }

        // Double click để xem chi tiết
        private void dgMedicalRecords_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedRecord = dgMedicalRecords.SelectedItem as MedicalRecord;
            if (selectedRecord != null)
            {
                // TODO: Mở form xem chi tiết sổ bệnh án
                MessageBox.Show($"Sẽ xem chi tiết sổ BA: {selectedRecord.RecordCode}", "Thông báo");
            }
        }
    }
}