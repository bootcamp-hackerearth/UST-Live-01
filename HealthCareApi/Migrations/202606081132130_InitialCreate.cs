namespace HealthCareApi.Migrations
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
                        ScheduledDate = c.DateTime(nullable: false, storeType: "date"),
                        TimeSlot = c.String(maxLength: 20),
                        Status = c.String(maxLength: 20),
                        CancellationReason = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
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
                        DoctorId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Specialisation = c.String(nullable: false, maxLength: 50),
                        YearsOfExperience = c.Int(nullable: false),
                        ConsultationFee = c.Decimal(nullable: false, precision: 10, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DoctorId)
                .ForeignKey("dbo.Users", t => t.DoctorId)
                .Index(t => t.DoctorId)
                .Index(t => t.UserId, unique: true);
            
            CreateTable(
                "dbo.DoctorAvailableSlots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        TimeSlot = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.HealthRecords",
                c => new
                    {
                        RecordId = c.Int(nullable: false),
                        AppointmentId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        VisitDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Diagnosis = c.String(maxLength: 500),
                        Prescription = c.String(maxLength: 500),
                        Notes = c.String(maxLength: 1000),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .ForeignKey("dbo.Appointments", t => t.RecordId)
                .Index(t => t.RecordId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        DateOfBirth = c.DateTime(nullable: false, storeType: "date"),
                        Gender = c.String(maxLength: 10),
                        PhoneNumber = c.String(maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 100),
                        InsuranceId = c.String(maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.PatientId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(maxLength: 256),
                        Role = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.DoctorLeaves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        LeaveDate = c.DateTime(nullable: false),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.VwDoctorProfiles",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Specialisation = c.String(),
                        YearsOfExperience = c.Int(nullable: false),
                        ConsultationFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        TotalAppointments = c.Int(),
                    })
                .PrimaryKey(t => t.DoctorId);
            
            CreateTable(
                "dbo.VwDoctorSchedules",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        DoctorName = c.String(),
                        PatientId = c.Int(nullable: false),
                        PatientName = c.String(),
                        ScheduledDate = c.DateTime(nullable: false),
                        TimeSlot = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.AppointmentId);
            
            CreateTable(
                "dbo.VwPatientAppointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        PatientName = c.String(),
                        DoctorId = c.Int(nullable: false),
                        DoctorName = c.String(),
                        Specialisation = c.String(),
                        ScheduledDate = c.DateTime(nullable: false),
                        TimeSlot = c.String(),
                        Status = c.String(),
                        CancellationReason = c.String(),
                    })
                .PrimaryKey(t => t.AppointmentId);
            
            CreateTable(
                "dbo.VwPatientHealthHistories",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        PatientName = c.String(),
                        DoctorName = c.String(),
                        Specialisation = c.String(),
                        VisitDate = c.DateTime(nullable: false),
                        Diagnosis = c.String(),
                        Prescription = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.RecordId);
            
            CreateTable(
                "dbo.VwPatientProfiles",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        InsuranceId = c.String(),
                        AppointmentCount = c.Int(),
                    })
                .PrimaryKey(t => t.PatientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.HealthRecords", "RecordId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorLeaves", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.HealthRecords", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Patients", "UserId", "dbo.Users");
            DropForeignKey("dbo.Doctors", "DoctorId", "dbo.Users");
            DropForeignKey("dbo.HealthRecords", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorAvailableSlots", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.DoctorLeaves", new[] { "DoctorId" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Patients", new[] { "Email" });
            DropIndex("dbo.Patients", new[] { "UserId" });
            DropIndex("dbo.HealthRecords", new[] { "DoctorId" });
            DropIndex("dbo.HealthRecords", new[] { "PatientId" });
            DropIndex("dbo.HealthRecords", new[] { "RecordId" });
            DropIndex("dbo.DoctorAvailableSlots", new[] { "DoctorId" });
            DropIndex("dbo.Doctors", new[] { "UserId" });
            DropIndex("dbo.Doctors", new[] { "DoctorId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropTable("dbo.VwPatientProfiles");
            DropTable("dbo.VwPatientHealthHistories");
            DropTable("dbo.VwPatientAppointments");
            DropTable("dbo.VwDoctorSchedules");
            DropTable("dbo.VwDoctorProfiles");
            DropTable("dbo.DoctorLeaves");
            DropTable("dbo.Users");
            DropTable("dbo.Patients");
            DropTable("dbo.HealthRecords");
            DropTable("dbo.DoctorAvailableSlots");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
        }
    }
}
