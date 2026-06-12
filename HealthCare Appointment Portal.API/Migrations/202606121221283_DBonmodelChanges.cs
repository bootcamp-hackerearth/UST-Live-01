namespace HealthCare_Appointment_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBonmodelChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HealthRecords", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.HealthRecords", "PatientId", "dbo.Patients");
            AddForeignKey("dbo.HealthRecords", "DoctorId", "dbo.Doctors", "DoctorId");
            AddForeignKey("dbo.HealthRecords", "PatientId", "dbo.Patients", "PatientId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HealthRecords", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.HealthRecords", "DoctorId", "dbo.Doctors");
            AddForeignKey("dbo.HealthRecords", "PatientId", "dbo.Patients", "PatientId", cascadeDelete: true);
            AddForeignKey("dbo.HealthRecords", "DoctorId", "dbo.Doctors", "DoctorId", cascadeDelete: true);
        }
    }
}
