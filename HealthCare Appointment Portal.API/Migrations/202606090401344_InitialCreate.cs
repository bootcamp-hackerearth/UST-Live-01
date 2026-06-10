namespace HealthCare_Appointment_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        ScheduledDate = c.DateTime(nullable: false),
                        TimeSlot = c.String(nullable: false, maxLength: 20),
                        Status = c.Int(nullable: false),
                        CancellationReason = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Specialisation = c.Int(nullable: false),
                        YearsOfExperience = c.Int(nullable: false),
                        ConsultationFee = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                        Diagnosis = c.String(nullable: false, maxLength: 500),
                        Prescription = c.String(nullable: false, maxLength: 500),
                        Notes = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.AppointmentId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PatientId);
            
            CreateTable(
                "dbo.Insurances",
                c => new
                    {
                        InsuranceId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 100),
                        PolicyNumber = c.String(nullable: false, maxLength: 50),
                        CoverageAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpiryDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InsuranceId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserCode = c.String(nullable: false, maxLength: 10),
                        Email = c.String(maxLength: 100),
                        PasswordHash = c.String(maxLength: 256),
                        Role = c.Int(nullable: false),
                        ReferenceId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Insurances", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.HealthRecords", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.HealthRecords", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.HealthRecords", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Insurances", new[] { "PatientId" });
            DropIndex("dbo.HealthRecords", new[] { "DoctorId" });
            DropIndex("dbo.HealthRecords", new[] { "PatientId" });
            DropIndex("dbo.HealthRecords", new[] { "AppointmentId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropTable("dbo.Users");
            DropTable("dbo.Insurances");
            DropTable("dbo.Patients");
            DropTable("dbo.HealthRecords");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
        }
    }
}
