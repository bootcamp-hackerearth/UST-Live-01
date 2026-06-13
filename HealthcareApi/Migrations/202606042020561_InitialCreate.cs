namespace HealthcareMvcApp.Migrations
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
                        SlotNumber = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CancellationReason = c.String(),
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
                        PracticeStartDate = c.DateTime(nullable: false),
                        ConsultationFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DoctorId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Email = c.String(nullable: false),
                        InsuranceId = c.String(nullable: false, maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PatientId);
            
            CreateTable(
                "dbo.HealthRecords",
                c => new
                    {
                        HealthRecordId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        AppointmentId = c.Int(nullable: false),
                        VisitDate = c.DateTime(nullable: false),
                        Diagnosis = c.String(nullable: false, maxLength: 500),
                        Prescription = c.String(nullable: false, maxLength: 500),
                        Notes = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.HealthRecordId)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.AppointmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HealthRecords", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.HealthRecords", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.HealthRecords", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.HealthRecords", new[] { "AppointmentId" });
            DropIndex("dbo.HealthRecords", new[] { "DoctorId" });
            DropIndex("dbo.HealthRecords", new[] { "PatientId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropTable("dbo.HealthRecords");
            DropTable("dbo.Patients");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
        }
    }
}
