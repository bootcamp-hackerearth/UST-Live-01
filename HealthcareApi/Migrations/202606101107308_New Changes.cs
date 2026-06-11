namespace HealthcareMvcApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patients", "PhoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "PhoneNumber", c => c.String(nullable: false, maxLength: 15));
        }
    }
}
