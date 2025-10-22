using HospitalManagementSystem.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace HospitalManagementSystem.Data
{
    // Lớp HospitalContext kế thừa từ DbContext của Entity Framework
    public class HospitalContext : DbContext
    {
        // Constructor gọi đến constructor của lớp base và truyền vào tên chuỗi kết nối
        // EF sẽ tìm chuỗi kết nối có tên 'HospitalDbConnectionString' trong file App.config
        public HospitalContext() : base("name=HospitalDbConnectionString")
        {
            // TẠO LẠI DATABASE MỖI KHI MODEL THAY ĐỔI (chỉ dùng trong dev)
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HospitalContext>());

        }

        // Mỗi thuộc tính DbSet<T> tương ứng với một bảng trong CSDL
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<ClinicRoom> ClinicRooms { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<TreatmentProgress> TreatmentProgresses { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình thêm nếu cần (ví dụ: tên bảng, quan hệ phức tạp)
            // Đảm bảo Username là duy nhất
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // MedicalRecords
            modelBuilder.Entity<MedicalRecord>()
                .Property(m => m.CreatedDate)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<MedicalRecord>()
                .HasIndex(m => m.RecordCode)
                .IsUnique();
        }
    }
}