namespace HealthCare_Appointment_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "PasswordHash", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "PasswordHash", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
