namespace HospitalManagementSystem.Migrations
{
    using HospitalManagementSystem.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HospitalManagementSystem.Data.HospitalContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HospitalManagementSystem.Data.HospitalContext context)
        {
            // Thêm dữ liệu mẫu cho Departments nếu chưa có
            /*if (!context.Departments.Any())
            {
                context.Departments.AddOrUpdate(
                    d => d.DepartmentName,
                    new Department
                    {
                        DepartmentName = "Noi khoa",
                        Description = "Kham va dieu tri cac benh ly noi tang"
                    },
                    new Department
                    {
                        DepartmentName = "Ngoai khoa",
                        Description = "Phau thuat va dieu tri cac benh ly ngoai khoa"
                    },
                    new Department
                    {
                        DepartmentName = "San khoa",
                        Description = "Cham soc suc khoe ba me va thai nhi"
                    },
                    new Department
                    {
                        DepartmentName = "Nhi khoa",
                        Description = "Kham va dieu tri benh cho tre em"
                    },
                    new Department
                    {
                        DepartmentName = "Tim mach",
                        Description = "Chuyen khoa tim mach"
                    },
                    new Department
                    {
                        DepartmentName = "Than kinh",
                        Description = "Dieu tri cac benh ly than kinh"
                    },
                    new Department
                    {
                        DepartmentName = "Tai Mui Hong",
                        Description = "Kham va dieu tri benh tai mui hong"
                    },
                    new Department
                    {
                        DepartmentName = "Mat",
                        Description = "Kham va dieu tri benh ve mat"
                    }
                );

                context.SaveChanges();
            }*/

            // Thêm tài khoản admin mặc định
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                context.Users.AddOrUpdate(
                    u => u.Username,
                    new User
                    {
                        Username = "admin",
                        Password = "admin123",
                        FullName = "Quan tri vien",
                        Role = "Admin"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}