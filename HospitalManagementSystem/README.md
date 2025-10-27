# 🏥 Hệ Thống Quản Lý Bệnh Viện

Ứng dụng quản lý bệnh viện được xây dựng bằng WPF C# và Entity Framework.

## ✨ Tính năng đã hoàn thành

### 📋 Quản lý cơ bản
- ✅ **Quản lý Bệnh nhân**: Thêm, sửa, xóa, tìm kiếm thông tin bệnh nhân
- ✅ **Quản lý Bác sĩ**: Quản lý thông tin bác sĩ, chuyên khoa, bằng cấp
- ✅ **Quản lý Khoa**: Quản lý các khoa khám bệnh
- ✅ **Quản lý Phòng khám**: 
  - Quản lý phòng khám theo khoa
  - Lọc phòng khám theo khoa
  - Quản lý trạng thái phòng
- ✅ **Quản lý Tài khoản**: 
  - Phân quyền người dùng (Admin, Doctor, Nurse, Receptionist, User)
  - Đăng nhập/Đăng xuất

### 📋 Sổ Bệnh Án (Mới)
- ✅ **Tạo sổ bệnh án mới**:
  - Tự động tạo mã sổ bệnh án (format: BA2025000001)
  - Chọn khoa điều trị và bác sĩ
  - Nhập chẩn đoán ban đầu và kế hoạch điều trị
  - Upload file đính kèm (PDF, DOC, DOCX, JPG, PNG - max 10MB)
  - Validation nâng cao (ngày nhập viện, giới hạn ký tự)
  - In giấy nhập viện
- ✅ **Xem danh sách sổ bệnh án**:
  - Tìm kiếm bệnh nhân
  - Lọc theo bệnh nhân
  - Hiển thị thông tin chi tiết

### 🎨 UI/UX Nâng cao
- ✅ Animation fade in/out khi mở/đóng form
- ✅ Progress bar khi lưu dữ liệu
- ✅ Toast notification tự động ẩn
- ✅ Đếm ký tự realtime
- ✅ Focus effect cho TextBox
- ✅ Responsive design

### 🔐 Phân quyền
- Admin: Toàn quyền
- Doctor: Xem và quản lý bệnh nhân, sổ bệnh án
- Nurse: Xem thông tin bệnh nhân
- Receptionist: Quản lý lịch hẹn (đang phát triển)

## 🛠️ Công nghệ sử dụng

- **Framework:** .NET Framework 4.7.2
- **UI:** WPF (Windows Presentation Foundation)
- **Database:** SQL Server Express
- **ORM:** Entity Framework 6.x
- **Pattern:** MVVM, Repository Pattern
- **Ngôn ngữ:** C#

## 📋 Yêu cầu hệ thống

- Windows 7 trở lên
- .NET Framework 4.7.2 hoặc cao hơn
- SQL Server 2012 trở lên (hoặc SQL Server Express)
- Visual Studio 2019/2022 (để phát triển)
- Dung lượng: ~100MB

## 🚀 Cài đặt

### 1. Clone repository
```bash
git clone https://github.com/Huy22012376/QuanLyBenhVien.net.git
```

### 2. Mở solution

Mở file `HospitalManagementSystem.sln` trong Visual Studio

### 3. Cấu hình connection string

Mở file `App.config` và sửa connection string:
```xml
<connectionStrings>
  <add name="HospitalDB" 
       connectionString="Data Source=YOUR_SERVER\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Lưu ý:** Thay `YOUR_SERVER` bằng tên máy tính của bạn.

**Tìm tên máy:**
```cmd
hostname
```

### 4. Restore NuGet packages
```
Tools → NuGet Package Manager → Restore NuGet Packages
```

### 5. Tạo database

**Cách 1: Tự động (Entity Framework)**

Mở **Package Manager Console** và chạy:
```powershell
Update-Database
```

**Cách 2: Cho phép tự động tạo**

Trong file `Data/HospitalContext.cs`, đảm bảo có:
```csharp
Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HospitalContext>());
```

Sau đó chạy ứng dụng (F5), database sẽ tự động được tạo.

### 6. Build và Run

Nhấn `F5` hoặc `Ctrl+F5` để chạy ứng dụng.

## 🔑 Đăng nhập mặc định

- **Username:** `admin`
- **Password:** `admin123`

## 📂 Cấu trúc dự án
```
HospitalManagementSystem/
├── Data/                           # Entity Framework DbContext
├── Models/                         # Entity models
│   ├── Patient.cs
│   ├── Doctor.cs
│   ├── Department.cs
│   ├── ClinicRoom.cs
│   ├── MedicalRecord.cs
│   ├── TreatmentProgress.cs
│   ├── Prescription.cs
│   └── User.cs
├── Helpers/                        # Helper classes
│   └── SessionManager.cs
├── UserControls/                   # User Controls (đề xuất)
│   ├── PatientsControl.xaml
│   ├── DoctorsControl.xaml
│   ├── DepartmentsControl.xaml
│   ├── AccountsControl.xaml
│   ├── ClinicRoomsControl.xaml
│   └── MedicalRecordsControl.xaml
├── Windows/                        # Popup Windows
│   └── CreateMedicalRecordWindow.xaml
├── Attachments/                    # File đính kèm (tự động tạo)
│   ├── 1/                          # RecordId folders
│   └── 2/
├── MainWindow.xaml                 # Cửa sổ chính
├── LoginWindow.xaml                # Cửa sổ đăng nhập
├── Styles.xaml                     # Styles và themes
└── App.config                      # Configuration
```

## 📸 Screenshots

### Màn hình đăng nhập
![Login](screenshots/login.png)

### Dashboard chính
![Main Dashboard](screenshots/main.png)

### Quản lý Bệnh nhân
![Patients Management](screenshots/patients.png)

### Tạo Sổ Bệnh Án
![Medical Records](screenshots/medical-records.png)

## 🔄 Workflow Git

### Trước khi bắt đầu làm việc:
```bash
git pull origin master
```

### Sau khi code xong:
```bash
git add .
git commit -m "Add/Update feature description"
git push origin master
```

### Branch naming convention:
```
feature/ten-tinh-nang      → Tính năng mới
fix/ten-loi                → Sửa lỗi
hotfix/ten-loi-gap         → Sửa lỗi khẩn cấp
refactor/ten-phan          → Tái cấu trúc code
docs/ten-tai-lieu          → Cập nhật tài liệu
```

### Commit message convention:
```
feat: Add medical records module
fix: Fix login validation bug
refactor: Restructure database context
docs: Update README with setup instructions
style: Format code according to standards
test: Add unit tests for Doctor service
```

## 📝 TODO - Tính năng đang phát triển

- [ ] **Sổ bệnh án nâng cao:**
  - [ ] Xem chi tiết sổ bệnh án với tabs
  - [ ] Quá trình điều trị (multiple visits)
  - [ ] Đơn thuốc chi tiết
  - [ ] Kết quả xét nghiệm
  - [ ] Xuất viện và tổng kết
- [ ] **Module Xét nghiệm:**
  - [ ] Quản lý loại xét nghiệm
  - [ ] Đăng ký xét nghiệm
  - [ ] Nhập kết quả
  - [ ] In phiếu kết quả
- [ ] **Module Phẫu thuật:**
  - [ ] Quản lý loại phẫu thuật
  - [ ] Đăng ký phẫu thuật
  - [ ] Phòng mổ
  - [ ] Theo dõi hậu phẫu
- [ ] **Module Thuốc:**
  - [ ] Quản lý kho thuốc
  - [ ] Nhập/xuất thuốc
  - [ ] Đơn thuốc chi tiết
  - [ ] Cảnh báo hết hạn
- [ ] **Module Lịch hẹn:**
  - [ ] Đặt lịch khám
  - [ ] Quản lý lịch bác sĩ
  - [ ] Thông báo nhắc nhở
- [ ] **Báo cáo thống kê:**
  - [ ] Báo cáo khám bệnh
  - [ ] Báo cáo doanh thu
  - [ ] Báo cáo bệnh nhân
  - [ ] Export Excel/PDF
- [ ] **In ấn:**
  - [ ] In phiếu khám bệnh
  - [ ] In đơn thuốc
  - [ ] In kết quả xét nghiệm
  - [ ] In sổ bệnh án

## 🐛 Báo lỗi

Nếu phát hiện lỗi, vui lòng tạo [Issue](https://github.com/Huy22012376/QuanLyBenhVien.net/issues) mới với:
- Mô tả chi tiết lỗi
- Các bước để reproduce
- Screenshots (nếu có)
- Thông tin môi trường (Windows version, .NET version)

## 🤝 Đóng góp

Mọi đóng góp đều được chào đón! Quy trình đóng góp:

1. Fork repository
2. Tạo branch mới (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Tạo Pull Request

## 👨‍💻 Nhóm phát triển

- **Huy22012376** - [GitHub](https://github.com/Huy22012376) - Lead Developer
- **Huyui2410** - [GitHub](https://github.com/Huyui2410) - Developer

## 📄 License

Dự án này được phát hành dưới MIT License - xem file [LICENSE](LICENSE) để biết thêm chi tiết.

## 📧 Liên hệ

Nếu có bất kỳ câu hỏi nào, vui lòng liên hệ qua:
- GitHub Issues: https://github.com/Huy22012376/QuanLyBenhVien.net/issues
- Email: [your-email@example.com]

## 🙏 Lời cảm ơn

- [Entity Framework](https://github.com/dotnet/ef6) - ORM framework
- [Material Design](https://material.io/) - Design inspiration
- [Icons8](https://icons8.com/) - Icons

---

## 📊 Thống kê dự án

![GitHub repo size](https://img.shields.io/github/repo-size/Huy22012376/QuanLyBenhVien.net)
![GitHub contributors](https://img.shields.io/github/contributors/Huy22012376/QuanLyBenhVien.net)
![GitHub last commit](https://img.shields.io/github/last-commit/Huy22012376/QuanLyBenhVien.net)
![GitHub issues](https://img.shields.io/github/issues/Huy22012376/QuanLyBenhVien.net)

---

⭐ Nếu bạn thấy dự án hữu ích, hãy cho 1 star nhé!

**Phiên bản:** 1.0.0  
**Cập nhật:** 28/10/2025