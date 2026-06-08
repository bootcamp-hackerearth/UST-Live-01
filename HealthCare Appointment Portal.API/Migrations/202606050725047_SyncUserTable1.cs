namespace HealthCare_Appointment_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncUserTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
