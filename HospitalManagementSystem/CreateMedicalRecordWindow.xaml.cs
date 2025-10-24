using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HospitalManagementSystem
{
    public partial class CreateMedicalRecordWindow : Window
    {
        private HospitalContext _context = new HospitalContext();
        private Patient _patient;
        public bool IsSuccess { get; private set; } = false;
        private List<string> _attachedFiles = new List<string>();

        public CreateMedicalRecordWindow(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPatientInfo();
            LoadDepartments();
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
                txtDateOfBirth.Text = _patient.DateOfBirth != default(DateTime)
                    ? _patient.DateOfBirth.ToString("dd/MM/yyyy")
                    : "";
                txtGender.Text = _patient.Gender;
                txtPhoneNumber.Text = _patient.PhoneNumber;
            }
        }

        // Load danh sách khoa
        private void LoadDepartments()
        {
            cmbDepartment.ItemsSource = _context.Departments.ToList();

            if (_patient.DepartmentId.HasValue)
            {
                cmbDepartment.SelectedValue = _patient.DepartmentId.Value;
            }
        }

        // Load danh sách bác sĩ
        private void LoadDoctors()
        {
            cmbDoctor.ItemsSource = _context.Doctors.ToList();
        }

        // Lọc bác sĩ theo khoa
        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDepartment.SelectedValue != null)
            {
                int deptId = (int)cmbDepartment.SelectedValue;
                var doctors = _context.Doctors
                    .Where(d => d.DepartmentId == deptId)
                    .ToList();
                cmbDoctor.ItemsSource = doctors;
                cmbDoctor.SelectedIndex = doctors.Count > 0 ? 0 : -1;
            }
        }

        // Tự động tạo mã sổ bệnh án
        private void GenerateRecordCode()
        {
            int count = _context.MedicalRecords.Count();
            string year = DateTime.Now.Year.ToString();
            string number = (count + 1).ToString("D6");

            txtRecordCode.Text = $"BA{year}{number}";
            txtRecordCode.IsReadOnly = true;
            txtRecordCode.Background = System.Windows.Media.Brushes.LightGray;
        }

        // Kiểm tra ngày nhập viện
        private void dpAdmissionDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // THÊM KIỂM TRA NULL
            if (txtDateError == null || dpAdmissionDate == null)
                return;

            if (dpAdmissionDate.SelectedDate.HasValue)
            {
                DateTime selectedDate = dpAdmissionDate.SelectedDate.Value.Date;
                DateTime today = DateTime.Now.Date;

                if (selectedDate > today)
                {
                    txtDateError.Text = "⚠️ Ngày nhập viện không được ở tương lai!";
                    txtDateError.Visibility = Visibility.Visible;
                    dpAdmissionDate.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    txtDateError.Visibility = Visibility.Collapsed;
                    dpAdmissionDate.BorderBrush = System.Windows.Media.Brushes.Gray;
                }
            }
        }

        // Đếm ký tự chẩn đoán
        private void txtInitialDiagnosis_TextChanged(object sender, TextChangedEventArgs e)
        {
            // THÊM KIỂM TRA NULL
            if (txtDiagnosisCount == null || txtInitialDiagnosis == null)
                return;

            int count = txtInitialDiagnosis.Text.Length;
            txtDiagnosisCount.Text = $"{count}/500 ký tự";

            if (count > 450)
            {
                txtDiagnosisCount.Foreground = System.Windows.Media.Brushes.Red;
            }
            else
            {
                txtDiagnosisCount.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // Đếm ký tự kế hoạch điều trị
        private void txtTreatmentPlan_TextChanged(object sender, TextChangedEventArgs e)
        {
            // THÊM KIỂM TRA NULL
            if (txtPlanCount == null || txtTreatmentPlan == null)
                return;

            int count = txtTreatmentPlan.Text.Length;
            txtPlanCount.Text = $"{count}/1000 ký tự";

            if (count > 900)
            {
                txtPlanCount.Foreground = System.Windows.Media.Brushes.Red;
            }
            else
            {
                txtPlanCount.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // Validation
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtRecordCode.Text))
            {
                ShowToast("⚠️ Vui lòng nhập mã sổ bệnh án!");
                txtRecordCode.Focus();
                return false;
            }

            if (cmbDepartment.SelectedValue == null)
            {
                ShowToast("⚠️ Vui lòng chọn khoa điều trị!");
                cmbDepartment.Focus();
                return false;
            }

            if (cmbDoctor.SelectedValue == null)
            {
                ShowToast("⚠️ Vui lòng chọn bác sĩ điều trị!");
                cmbDoctor.Focus();
                return false;
            }

            if (dpAdmissionDate.SelectedDate == null)
            {
                ShowToast("⚠️ Vui lòng chọn ngày nhập viện!");
                dpAdmissionDate.Focus();
                return false;
            }

            if (dpAdmissionDate.SelectedDate.Value.Date > DateTime.Now.Date)
            {
                ShowToast("⚠️ Ngày nhập viện không được ở tương lai!");
                dpAdmissionDate.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtInitialDiagnosis.Text))
            {
                ShowToast("⚠️ Vui lòng nhập chẩn đoán ban đầu!");
                txtInitialDiagnosis.Focus();
                return false;
            }

            bool exists = _context.MedicalRecords.Any(m => m.RecordCode == txtRecordCode.Text);
            if (exists)
            {
                ShowToast("⚠️ Mã sổ bệnh án đã tồn tại!");
                return false;
            }

            return true;
        }

        // Chọn file đính kèm
        private void btnBrowseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn file đính kèm",
                Filter = "All Files|*.pdf;*.doc;*.docx;*.jpg;*.jpeg;*.png|" +
                         "PDF Files|*.pdf|" +
                         "Word Documents|*.doc;*.docx|" +
                         "Images|*.jpg;*.jpeg;*.png",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileInfo.Length > 10 * 1024 * 1024)
                    {
                        ShowToast($"⚠️ File {Path.GetFileName(file)} vượt quá 10MB!");
                        continue;
                    }

                    _attachedFiles.Add(file);
                }

                UpdateAttachmentList();
            }
        }

        // Cập nhật danh sách file
        private void UpdateAttachmentList()
        {
            if (_attachedFiles.Count > 0)
            {
                lstAttachments.ItemsSource = _attachedFiles.Select(f => Path.GetFileName(f)).ToList();
                lstAttachments.Visibility = Visibility.Visible;
                txtAttachment.Text = $"{_attachedFiles.Count} file đã chọn";
            }
            else
            {
                lstAttachments.Visibility = Visibility.Collapsed;
                txtAttachment.Text = "Chưa có file đính kèm";
            }
        }

        // Xóa file đính kèm
        private void btnRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string fileName = btn.Tag.ToString();
                var fileToRemove = _attachedFiles.FirstOrDefault(f => Path.GetFileName(f) == fileName);
                if (fileToRemove != null)
                {
                    _attachedFiles.Remove(fileToRemove);
                    UpdateAttachmentList();
                    ShowToast($"✓ Đã xóa file {fileName}");
                }
            }
        }

        // Tạo sổ bệnh án
        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            ShowProgress(true);

            try
            {
                await Task.Delay(500);

                var newRecord = new MedicalRecord
                {
                    RecordCode = txtRecordCode.Text.Trim(),
                    PatientId = _patient.PatientId,
                    DoctorId = (int?)cmbDoctor.SelectedValue,
                    AdmissionDate = dpAdmissionDate.SelectedDate,
                    InitialDiagnosis = txtInitialDiagnosis.Text.Trim(),
                    TreatmentPlan = txtTreatmentPlan.Text.Trim(),
                    Status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Notes = txtNotes.Text.Trim(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                _context.MedicalRecords.Add(newRecord);
                _context.SaveChanges();

                if (_attachedFiles.Count > 0)
                {
                    SaveAttachments(newRecord.RecordId);
                }

                ShowProgress(false);
                ShowToast("✓ Tạo sổ bệnh án thành công!");

                await Task.Delay(1500);

                IsSuccess = true;
                await CloseWithAnimation();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                ShowProgress(false);
                ShowToast($"✖ Lỗi: {ex.Message}");
            }
        }

        // Lưu file đính kèm
        private void SaveAttachments(int recordId)
        {
            try
            {
                string attachmentFolder = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Attachments",
                    recordId.ToString());

                if (!Directory.Exists(attachmentFolder))
                {
                    Directory.CreateDirectory(attachmentFolder);
                }

                foreach (string file in _attachedFiles)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(attachmentFolder, fileName);
                    File.Copy(file, destFile, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu file: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // In giấy nhập viện
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                ShowToast("⚠️ Vui lòng điền đầy đủ thông tin trước khi in!");
                return;
            }

            try
            {
                string printContent = GeneratePrintContent();

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    var flowDoc = new System.Windows.Documents.FlowDocument();
                    var para = new System.Windows.Documents.Paragraph();
                    para.Inlines.Add(new System.Windows.Documents.Run(printContent));
                    flowDoc.Blocks.Add(para);

                    flowDoc.PageHeight = printDialog.PrintableAreaHeight;
                    flowDoc.PageWidth = printDialog.PrintableAreaWidth;
                    flowDoc.PagePadding = new Thickness(50);
                    flowDoc.ColumnGap = 0;
                    flowDoc.ColumnWidth = printDialog.PrintableAreaWidth;

                    System.Windows.Documents.IDocumentPaginatorSource idpSource = flowDoc;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Giấy nhập viện");

                    ShowToast("✓ In thành công!");
                }
            }
            catch (Exception ex)
            {
                ShowToast($"✖ Lỗi: {ex.Message}");
            }
        }

        // Tạo nội dung in
        private string GeneratePrintContent()
        {
            var doctor = cmbDoctor.SelectedItem as Doctor;
            var department = cmbDepartment.SelectedItem as Department;

            return $@"
═══════════════════════════════════════════════
        BỆNH VIỆN ĐA KHOA TRUNG ƯƠNG
             GIẤY NHẬP VIỆN
═══════════════════════════════════════════════

Mã sổ bệnh án: {txtRecordCode.Text}
Ngày nhập viện: {dpAdmissionDate.SelectedDate?.ToString("dd/MM/yyyy")}

───────────────────────────────────────────────

THÔNG TIN BỆNH NHÂN:

Họ và tên: {_patient.FullName}
Ngày sinh: {_patient.DateOfBirth.ToString("dd/MM/yyyy")}
Giới tính: {_patient.Gender}
Số điện thoại: {_patient.PhoneNumber}

───────────────────────────────────────────────

THÔNG TIN ĐIỀU TRỊ:

Khoa: {department?.DepartmentName}
Bác sĩ: {doctor?.FullName}
Chẩn đoán: {txtInitialDiagnosis.Text}

───────────────────────────────────────────────

Ngày in: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}
";
        }

        // Hiển thị progress bar
        private void ShowProgress(bool show)
        {
            pnlProgress.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            btnCreate.IsEnabled = !show;
        }

        // Toast notification
        private async void ShowToast(string message)
        {
            txtToast.Text = message;
            toastNotification.Visibility = Visibility.Visible;

            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            toastNotification.BeginAnimation(OpacityProperty, fadeIn);

            await Task.Delay(3000);

            var fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            fadeOut.Completed += (s, e) => toastNotification.Visibility = Visibility.Collapsed;
            toastNotification.BeginAnimation(OpacityProperty, fadeOut);
        }

        // Animation đóng window
        private async Task CloseWithAnimation()
        {
            var fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            this.BeginAnimation(OpacityProperty, fadeOut);
            await Task.Delay(200);
        }

        // Nút Hủy
        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn hủy?\nDữ liệu sẽ không được lưu.",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await CloseWithAnimation();
                this.DialogResult = false;
            }
        }
    }
}