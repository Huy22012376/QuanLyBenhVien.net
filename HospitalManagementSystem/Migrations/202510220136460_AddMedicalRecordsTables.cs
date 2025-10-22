namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMedicalRecordsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalRecords",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        RecordCode = c.String(maxLength: 50),
                        PatientId = c.Int(),
                        DoctorId = c.Int(),
                        AdmissionDate = c.DateTime(),
                        InitialDiagnosis = c.String(maxLength: 500),
                        TreatmentPlan = c.String(),
                        Status = c.String(maxLength: 50),
                        DischargeDate = c.DateTime(),
                        FinalDiagnosis = c.String(maxLength: 500),
                        TreatmentResult = c.String(maxLength: 500),
                        Notes = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.RecordCode, unique: true)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        PrescriptionId = c.Int(nullable: false, identity: true),
                        RecordId = c.Int(),
                        ProgressId = c.Int(),
                        PrescriptionDate = c.DateTime(),
                        DoctorId = c.Int(),
                        Notes = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PrescriptionId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.MedicalRecords", t => t.RecordId)
                .ForeignKey("dbo.TreatmentProgresses", t => t.ProgressId)
                .Index(t => t.RecordId)
                .Index(t => t.ProgressId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.PrescriptionDetails",
                c => new
                    {
                        DetailId = c.Int(nullable: false, identity: true),
                        PrescriptionId = c.Int(),
                        MedicineName = c.String(maxLength: 200),
                        Dosage = c.String(maxLength: 100),
                        Frequency = c.String(maxLength: 100),
                        Duration = c.String(maxLength: 100),
                        Instructions = c.String(maxLength: 500),
                        Quantity = c.Int(),
                    })
                .PrimaryKey(t => t.DetailId)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId)
                .Index(t => t.PrescriptionId);
            
            CreateTable(
                "dbo.TreatmentProgresses",
                c => new
                    {
                        ProgressId = c.Int(nullable: false, identity: true),
                        RecordId = c.Int(),
                        VisitDate = c.DateTime(),
                        Symptoms = c.String(maxLength: 500),
                        Diagnosis = c.String(maxLength: 500),
                        Treatment = c.String(),
                        DoctorId = c.Int(),
                        Notes = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProgressId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.MedicalRecords", t => t.RecordId)
                .Index(t => t.RecordId)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "ProgressId", "dbo.TreatmentProgresses");
            DropForeignKey("dbo.TreatmentProgresses", "RecordId", "dbo.MedicalRecords");
            DropForeignKey("dbo.TreatmentProgresses", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.PrescriptionDetails", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "RecordId", "dbo.MedicalRecords");
            DropForeignKey("dbo.Prescriptions", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.MedicalRecords", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.MedicalRecords", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.TreatmentProgresses", new[] { "DoctorId" });
            DropIndex("dbo.TreatmentProgresses", new[] { "RecordId" });
            DropIndex("dbo.PrescriptionDetails", new[] { "PrescriptionId" });
            DropIndex("dbo.Prescriptions", new[] { "DoctorId" });
            DropIndex("dbo.Prescriptions", new[] { "ProgressId" });
            DropIndex("dbo.Prescriptions", new[] { "RecordId" });
            DropIndex("dbo.MedicalRecords", new[] { "DoctorId" });
            DropIndex("dbo.MedicalRecords", new[] { "PatientId" });
            DropIndex("dbo.MedicalRecords", new[] { "RecordCode" });
            DropTable("dbo.TreatmentProgresses");
            DropTable("dbo.PrescriptionDetails");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.MedicalRecords");
        }
    }
}
