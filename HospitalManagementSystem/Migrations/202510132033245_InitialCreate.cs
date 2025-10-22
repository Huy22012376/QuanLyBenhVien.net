namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false, maxLength: 10),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Address = c.String(maxLength: 200),
                        DepartmentId = c.Int(),
                        Symptoms = c.String(maxLength: 500),
                        AdmissionDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PatientId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        FullName = c.String(maxLength: 100),
                        Role = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Username, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patients", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Patients", new[] { "DepartmentId" });
            DropTable("dbo.Users");
            DropTable("dbo.Patients");
            DropTable("dbo.Departments");
        }
    }
}
