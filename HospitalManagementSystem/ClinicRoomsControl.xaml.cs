using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalManagementSystem
{
    public partial class ClinicRoomsControl : UserControl
    {
        private HospitalContext _context = new HospitalContext();
        private ClinicRoom _selectedRoom = null;

        public ClinicRoomsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDepartments();
            LoadClinicRooms();
        }

        // Load departments
        private void LoadDepartments()
        {
            // Load cho form input
            cmbRoomDepartment.ItemsSource = _context.Departments.ToList();

            // Load cho filter
            var departments = _context.Departments.ToList();
            departments.Insert(0, new Department { DepartmentId = 0, DepartmentName = "-- Tất cả khoa --" });
            cmbFilterDepartment.ItemsSource = departments;
            cmbFilterDepartment.SelectedIndex = 0;
        }

        // Load danh sách phòng khám
        private void LoadClinicRooms()
        {
            dgClinicRooms.ItemsSource = _context.ClinicRooms
                .Include(r => r.Department)
                .ToList();
        }

        // Lọc phòng khám theo khoa
        private void cmbFilterDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFilterDepartment.SelectedValue == null)
                return;

            var selectedDeptId = (int)cmbFilterDepartment.SelectedValue;

            if (selectedDeptId == 0) // "Tất cả khoa"
            {
                dgClinicRooms.ItemsSource = _context.ClinicRooms
                    .Include(r => r.Department)
                    .ToList();
            }
            else
            {
                dgClinicRooms.ItemsSource = _context.ClinicRooms
                    .Include(r => r.Department)
                    .Where(r => r.DepartmentId == selectedDeptId)
                    .ToList();
            }
        }

        // Clear form
        private void ClearRoomForm()
        {
            txtRoomCode.Clear();
            txtRoomName.Clear();
            cmbRoomDepartment.SelectedIndex = -1;
            txtRoomFloor.Clear();
            cmbRoomType.SelectedIndex = -1;
            txtRoomCapacity.Clear();
            cmbRoomStatus.SelectedIndex = 0;
            txtRoomEquipment.Clear();
            chkIsAvailable.IsChecked = true;
            _selectedRoom = null;
            dgClinicRooms.SelectedItem = null;
        }

        // Selection changed
        private void dgClinicRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRoom = dgClinicRooms.SelectedItem as ClinicRoom;
            if (_selectedRoom != null)
            {
                txtRoomCode.Text = _selectedRoom.RoomCode;
                txtRoomName.Text = _selectedRoom.RoomName;
                cmbRoomDepartment.SelectedValue = _selectedRoom.DepartmentId;
                txtRoomFloor.Text = _selectedRoom.Floor;

                foreach (ComboBoxItem item in cmbRoomType.Items)
                {
                    if (item.Content.ToString() == _selectedRoom.RoomType)
                    {
                        cmbRoomType.SelectedItem = item;
                        break;
                    }
                }

                txtRoomCapacity.Text = _selectedRoom.Capacity?.ToString() ?? "";

                foreach (ComboBoxItem item in cmbRoomStatus.Items)
                {
                    if (item.Content.ToString() == _selectedRoom.Status)
                    {
                        cmbRoomStatus.SelectedItem = item;
                        break;
                    }
                }

                txtRoomEquipment.Text = _selectedRoom.Equipment;
                chkIsAvailable.IsChecked = _selectedRoom.IsAvailable;
            }
        }

        // Thêm phòng khám
        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRoomCode.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phòng.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRoomName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên phòng.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var existingRoom = _context.ClinicRooms
                    .FirstOrDefault(r => r.RoomCode == txtRoomCode.Text.Trim());
                if (existingRoom != null)
                {
                    MessageBox.Show("Mã phòng đã tồn tại!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newRoom = new ClinicRoom
                {
                    RoomCode = txtRoomCode.Text.Trim(),
                    RoomName = txtRoomName.Text.Trim(),
                    DepartmentId = (int?)cmbRoomDepartment.SelectedValue,
                    Floor = txtRoomFloor.Text.Trim(),
                    RoomType = (cmbRoomType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Capacity = string.IsNullOrWhiteSpace(txtRoomCapacity.Text) ?
                               (int?)null : int.Parse(txtRoomCapacity.Text),
                    Status = (cmbRoomStatus.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Equipment = txtRoomEquipment.Text.Trim(),
                    IsAvailable = chkIsAvailable.IsChecked ?? false
                };

                _context.ClinicRooms.Add(newRoom);
                _context.SaveChanges();

                MessageBox.Show("Thêm phòng khám thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadClinicRooms();
                ClearRoomForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cập nhật phòng khám
        private void btnUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom == null)
            {
                MessageBox.Show("Vui lòng chọn một phòng khám để sửa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtRoomCode.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phòng.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRoomName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên phòng.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectedRoom.RoomCode = txtRoomCode.Text.Trim();
                _selectedRoom.RoomName = txtRoomName.Text.Trim();
                _selectedRoom.DepartmentId = (int?)cmbRoomDepartment.SelectedValue;
                _selectedRoom.Floor = txtRoomFloor.Text.Trim();
                _selectedRoom.RoomType = (cmbRoomType.SelectedItem as ComboBoxItem)?.Content.ToString();
                _selectedRoom.Capacity = string.IsNullOrWhiteSpace(txtRoomCapacity.Text) ?
                                         (int?)null : int.Parse(txtRoomCapacity.Text);
                _selectedRoom.Status = (cmbRoomStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
                _selectedRoom.Equipment = txtRoomEquipment.Text.Trim();
                _selectedRoom.IsAvailable = chkIsAvailable.IsChecked ?? false;

                _context.SaveChanges();

                MessageBox.Show("Cập nhật phòng khám thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LoadClinicRooms();
                ClearRoomForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xóa phòng khám
        private void btnDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom == null)
            {
                MessageBox.Show("Vui lòng chọn một phòng khám để xóa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phòng '{_selectedRoom.RoomName}' không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.ClinicRooms.Remove(_selectedRoom);
                    _context.SaveChanges();

                    MessageBox.Show("Xóa phòng khám thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadClinicRooms();
                    ClearRoomForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void txtRoomEquipment_TextChanged()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}