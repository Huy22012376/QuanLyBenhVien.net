namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClinicRoomTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClinicRooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        RoomCode = c.String(nullable: false, maxLength: 20),
                        RoomName = c.String(nullable: false, maxLength: 100),
                        DepartmentId = c.Int(),
                        Floor = c.String(maxLength: 20),
                        RoomType = c.String(maxLength: 50),
                        Capacity = c.Int(),
                        Status = c.String(maxLength: 20),
                        Equipment = c.String(maxLength: 500),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoomId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClinicRooms", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.ClinicRooms", new[] { "DepartmentId" });
            DropTable("dbo.ClinicRooms");
        }
    }
}
