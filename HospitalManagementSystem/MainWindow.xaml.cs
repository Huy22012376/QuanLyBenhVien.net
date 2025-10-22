using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HospitalManagementSystem.Helpers;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Diagnostics;

namespace HospitalManagementSystem
{
    public partial class MainWindow : Window
    {
        private HospitalContext _context = new HospitalContext();
        private bool isMenuExpanded = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Maximize window
            this.WindowState = WindowState.Maximized;

            // Load thông tin người dùng
            UpdateUserInfo();
        }

        

       
        private void UpdateUserInfo()
        {
            if (SessionManager.CurrentUser != null)
            {
                var user = SessionManager.CurrentUser;

                // Hiển thị tên người dùng
                txtUserName.Text = user.FullName ?? user.Username;


                // Hiển thị vai trò
                txtUserRole.Text = GetRoleDisplayName(user.Role);
                // Phân quyền menu
                ApplyRolePermissions(user.Role);
            }
            else
            {
                txtUserName.Text = "Guest";
                txtUserRole.Text = "Khách";
            }
        }

        private string GetRoleDisplayName(string role)
        {
            switch (role?.ToLower())
            {
                case "admin":
                    return "Quản trị viên";
                case "doctor":
                    return "Bác sĩ";
                case "nurse":
                    return "Y tá";
                case "receptionist":
                    return "Lễ tân";
                default:
                    return "Người dùng";
            }
        }

        private void ApplyRolePermissions(string role)
        {
            // Ẩn menu Quản lý Tài khoản nếu không phải Admin
            if (role?.ToLower() != "admin")
            {
                btnMenuAccounts.Visibility = Visibility.Collapsed;
                btnMenuDepartments.Visibility = Visibility.Collapsed;
                btnMenuSettings.Visibility = Visibility.Collapsed;
            }

            // Doctor chỉ xem được bệnh nhân
            if (role?.ToLower() == "doctor")
            {
                btnMenuDoctors.Visibility = Visibility.Collapsed;
                // Có thể ẩn các nút Thêm/Xóa
                // btnAddPatient.Visibility = Visibility.Collapsed;
            }
        }

        // ========== THU GỌN/MỞ RỘNG MENU ==========
        private void btnToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuExpanded)
            {
                // Thu gọn menu
                colMenu.Width = new GridLength(50);
                menuContent.Visibility = Visibility.Collapsed;
                menuCollapsed.Visibility = Visibility.Visible;
                isMenuExpanded = false;
            }
            else
            {
                // Mở rộng menu
                colMenu.Width = new GridLength(250);
                menuContent.Visibility = Visibility.Visible;
                menuCollapsed.Visibility = Visibility.Collapsed;
                isMenuExpanded = true;
            }
        }

        // ========== CHUYỂN ĐỔI PANEL ==========
        private void ShowPanel(UIElement panelToShow)
        {
            // Ẩn tất cả panels
            pnlPatients.Visibility = Visibility.Collapsed;
            pnlDepartments.Visibility = Visibility.Collapsed;
            pnlDoctors.Visibility = Visibility.Collapsed;
            pnlAccounts.Visibility = Visibility.Collapsed;
            pnlClinicRooms.Visibility = Visibility.Collapsed;
            pnlMedicalRecords.Visibility = Visibility.Collapsed;

            // Hiển thị panel được chọn
            panelToShow.Visibility = Visibility.Visible;
        }

        private void SetActiveMenu(Button activeButton, string pageTitle)
        {
            // Reset tất cả buttons về style mặc định
            btnMenuPatients.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuDoctors.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuDepartments.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuMedicalRecords.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuClinicRooms.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuRoomsBeds.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuBedAssignment.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuTestsScans.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuSurgery.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuExamination.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuMedicines.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuPositions.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuServiceUsage.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuExamForm.Style = (Style)FindResource("MenuButtonStyle");
            btnSearchStaff.Style = (Style)FindResource("MenuButtonStyle");
            btnSearchRoomInfo.Style = (Style)FindResource("MenuButtonStyle");
            btnSearchMedicalRecord.Style = (Style)FindResource("MenuButtonStyle");
            btnSearchPrescription.Style = (Style)FindResource("MenuButtonStyle");
            btnSearchPatientList.Style = (Style)FindResource("MenuButtonStyle");
            btnReportExamination.Style = (Style)FindResource("MenuButtonStyle");
            btnReportTreatment.Style = (Style)FindResource("MenuButtonStyle");
            btnReportRevenue.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuSettings.Style = (Style)FindResource("MenuButtonStyle");
            btnMenuAccounts.Style = (Style)FindResource("MenuButtonStyle");

            // Set active button
            activeButton.Style = (Style)FindResource("MenuButtonActiveStyle");

            // Cập nhật tiêu đề
            txtPageTitle.Text = pageTitle;
        }

        // ========== MENU HANDLERS ==========
        


        private void btnMenuMedicines_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(btnMenuMedicines, "Quản lý kho thuốc");
            MessageBox.Show("Chức năng Quản lý Thuốc đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void btnMenuSettings_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(btnMenuSettings, "Cài đặt hệ thống");
            MessageBox.Show("Chức năng Cài đặt đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        // ========== XỬ LÝ MENU ACCORDION ==========

        private void btnExpandManagement_Click(object sender, RoutedEventArgs e)
        {
            if (pnlManagement.Visibility == Visibility.Visible)
            {
                pnlManagement.Visibility = Visibility.Collapsed;
                btnExpandManagement.Content = "📋  Quản lý  ▶";
            }
            else
            {
                pnlManagement.Visibility = Visibility.Visible;
                btnExpandManagement.Content = "📋  Quản lý  ▼";
            }
        }

        private void btnExpandServices_Click(object sender, RoutedEventArgs e)
        {
            if (pnlServices.Visibility == Visibility.Visible)
            {
                pnlServices.Visibility = Visibility.Collapsed;
                btnExpandServices.Content = "🔧  Dịch vụ  ▶";
            }
            else
            {
                pnlServices.Visibility = Visibility.Visible;
                btnExpandServices.Content = "🔧  Dịch vụ  ▼";
            }
        }

        private void btnExpandSearch_Click(object sender, RoutedEventArgs e)
        {
            if (pnlSearch.Visibility == Visibility.Visible)
            {
                pnlSearch.Visibility = Visibility.Collapsed;
                btnExpandSearch.Content = "🔍  Tra cứu  ▶";
            }
            else
            {
                pnlSearch.Visibility = Visibility.Visible;
                btnExpandSearch.Content = "🔍  Tra cứu  ▼";
            }
        }

        private void btnExpandReports_Click(object sender, RoutedEventArgs e)
        {
            if (pnlReports.Visibility == Visibility.Visible)
            {
                pnlReports.Visibility = Visibility.Collapsed;
                btnExpandReports.Content = "📊  Báo cáo  ▶";
            }
            else
            {
                pnlReports.Visibility = Visibility.Visible;
                btnExpandReports.Content = "📊  Báo cáo  ▼";
            }
        }

        // ========== CÁC MENU ITEM CHƯA PHÁT TRIỂN ==========

        



        private void btnMenuRoomsBeds_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Phòng bệnh - Giường bệnh đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMenuBedAssignment_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Phân giường - Theo dõi điều trị đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMenuTestsScans_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Xét nghiệm - Chụp chiếu đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMenuSurgery_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Phẫu thuật đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMenuExamination_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Khám bệnh đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMenuPositions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Chức vụ đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMenuServiceUsage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Sử dụng dịch vụ đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMenuExamForm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Phiếu khám đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // TRA CỨU
        private void btnSearchStaff_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Danh sách Nhân viên đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSearchRoomInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Thông tin Phòng bệnh đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSearchMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Thông tin Số bệnh án đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSearchPrescription_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Thông tin Đơn thuốc đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSearchPatientList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Danh sách Bệnh nhân đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // BÁO CÁO
        private void btnReportExamination_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Báo cáo Khám bệnh đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnReportTreatment_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Báo cáo Chữa bệnh đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnReportRevenue_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Báo cáo Doanh thu đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ========== QUẢN LÝ BỆNH NHÂN ==========
        private void btnMenuPatients_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(btnMenuPatients, "Quản lý thông tin bệnh nhân");
            ShowPanel(pnlPatients);
        }

        // ========== QUẢN LÝ KHOA ==========
        private void btnMenuDepartments_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(btnMenuDepartments, "Quản lý khoa khám bệnh");
            ShowPanel(pnlDepartments);
        }

        // ========== QUẢN LÝ BÁC SĨ ==========

        private void btnMenuDoctors_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(btnMenuDoctors, "Quản lý thông tin bác sĩ");
            ShowPanel(pnlDoctors);
        }

        //=========== QUẢN LÝ SỔ BỆNH ÁN =========
        private void btnMenuMedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(btnMenuMedicalRecords, "Quản lý sổ bệnh án");
            ShowPanel(pnlMedicalRecords);
        }

        // ========== QUẢN LÝ TÀI KHOẢN ==========

        private void btnMenuAccounts_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(btnMenuAccounts, "Quản lý tài khoản người dùng");
            ShowPanel(pnlAccounts);
        }

        // ========== QUẢN LÝ PHÒNG KHÁM ==========

        // Menu click handler
        private void btnMenuClinicRooms_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenu(btnMenuClinicRooms, "Quản lý phòng khám");
            ShowPanel(pnlClinicRooms);
        }
    }
}