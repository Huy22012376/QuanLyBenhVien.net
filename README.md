# 🏥 Hệ Thống Quản Lý Bệnh Viện

Ứng dụng quản lý bệnh viện được xây dựng bằng WPF C# và Entity Framework.

## ✨ Tính năng

- 👥 Quản lý Bệnh nhân
- 👨‍⚕️ Quản lý Bác sĩ
- 🏢 Quản lý Khoa
- 🏥 Quản lý Phòng khám
- 👤 Quản lý Tài khoản
- 📋 Quản lý Sổ bệnh án (đang phát triển)

## 🛠️ Công nghệ sử dụng

- **Framework:** .NET Framework 4.7.2
- **UI:** WPF (Windows Presentation Foundation)
- **Database:** SQL Server / SQL Server Express
- **ORM:** Entity Framework 6.x
- **Ngôn ngữ:** C#

## 📋 Yêu cầu hệ thống

- Windows 7 trở lên
- .NET Framework 4.7.2 hoặc cao hơn
- SQL Server 2012 trở lên (hoặc SQL Server Express)
- Visual Studio 2019/2022 (để phát triển)

## 🚀 Cài đặt

1. Clone repository:
```bash
git clone https://github.com/Huy22012376/QuanLyBenhVien.net.git
```

2. Mở solution trong Visual Studio:
```
HospitalManagementSystem.sln
```

3. Cấu hình connection string:
   - Copy `App.config.example` thành `App.config`
   - Sửa connection string trong `App.config`:
```xml
<add name="HospitalDB" 
     connectionString="Data Source=YOUR_SERVER\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True" 
     providerName="System.Data.SqlClient" />
```

4. Restore NuGet packages:
```
Tools → NuGet Package Manager → Restore NuGet Packages
```

5. Tạo database:
   - Mở Package Manager Console
   - Chạy: `Update-Database`

6. Build và Run:
```
F5 hoặc Ctrl+F5
```

## 🔑 Đăng nhập mặc định

- **Username:** admin
- **Password:** admin123

## 📂 Cấu trúc dự án
```
HospitalManagementSystem/
├── Data/               # Entity Framework DbContext
├── Models/             # Entity models
├── Helpers/            # Helper classes
├── Views/              # XAML views
├── Resources/          # Styles và resources
└── App.config          # Configuration (không được push lên Git)
```

## 👨‍💻 Tác giả

- **Huy22012376** - [GitHub](https://github.com/Huy22012376)

## 📄 License

Dự án này được phát hành dưới MIT License.

## 🤝 Đóng góp

Mọi đóng góp đều được chào đón! Vui lòng tạo Pull Request.

## 📧 Liên hệ

Nếu có bất kỳ câu hỏi nào, vui lòng liên hệ qua Issues trên GitHub.
