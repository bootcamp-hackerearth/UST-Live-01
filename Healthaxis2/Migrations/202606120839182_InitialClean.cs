namespace Healthaxis2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialClean : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        ScheduledDate = c.DateTime(nullable: false),
                        Slot = c.String(nullable: false),
                        Status = c.String(nullable: false),
                        CancellationReason = c.String(),
                        Doctor_DoctorId = c.Int(),
                        Patient_PatientId = c.Int(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.doctors", t => t.Doctor_DoctorId)
                .ForeignKey("dbo.patients", t => t.Patient_PatientId)
                .ForeignKey("dbo.doctors", t => t.DoctorId)
                .ForeignKey("dbo.patients", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.Doctor_DoctorId)
                .Index(t => t.Patient_PatientId);
            
            CreateTable(
                "dbo.doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        DoctorName = c.String(nullable: false, maxLength: 30),
                        Specialisation = c.String(nullable: false),
                        Experience = c.Int(nullable: false),
                        Fees = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DoctorId);
            
            CreateTable(
                "dbo.HealthRecords",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        VisitDate = c.DateTime(nullable: false),
                        Diagnosis = c.String(nullable: false, maxLength: 50),
                        Prescription = c.String(nullable: false, maxLength: 100),
                        Notes = c.String(maxLength: 100),
                        Doctor_DoctorId = c.Int(),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.appointments", t => t.AppointmentId)
                .ForeignKey("dbo.doctors", t => t.DoctorId)
                .ForeignKey("dbo.patients", t => t.PatientId)
                .ForeignKey("dbo.doctors", t => t.Doctor_DoctorId)
                .Index(t => t.AppointmentId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.Doctor_DoctorId);
            
            CreateTable(
                "dbo.patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        PatientName = c.String(nullable: false, maxLength: 30),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(),
                        InsuranceId = c.String(),
                        RegisteredDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PatientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.appointments", "PatientId", "dbo.patients");
            DropForeignKey("dbo.appointments", "DoctorId", "dbo.doctors");
            DropForeignKey("dbo.HealthRecords", "Doctor_DoctorId", "dbo.doctors");
            DropForeignKey("dbo.HealthRecords", "PatientId", "dbo.patients");
            DropForeignKey("dbo.appointments", "Patient_PatientId", "dbo.patients");
            DropForeignKey("dbo.HealthRecords", "DoctorId", "dbo.doctors");
            DropForeignKey("dbo.HealthRecords", "AppointmentId", "dbo.appointments");
            DropForeignKey("dbo.appointments", "Doctor_DoctorId", "dbo.doctors");
            DropIndex("dbo.HealthRecords", new[] { "Doctor_DoctorId" });
            DropIndex("dbo.HealthRecords", new[] { "DoctorId" });
            DropIndex("dbo.HealthRecords", new[] { "PatientId" });
            DropIndex("dbo.HealthRecords", new[] { "AppointmentId" });
            DropIndex("dbo.appointments", new[] { "Patient_PatientId" });
            DropIndex("dbo.appointments", new[] { "Doctor_DoctorId" });
            DropIndex("dbo.appointments", new[] { "DoctorId" });
            DropIndex("dbo.appointments", new[] { "PatientId" });
            DropTable("dbo.patients");
            DropTable("dbo.HealthRecords");
            DropTable("dbo.doctors");
            DropTable("dbo.appointments");
        }
    }
}
