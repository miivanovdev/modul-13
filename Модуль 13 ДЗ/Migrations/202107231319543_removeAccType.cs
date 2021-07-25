namespace Модуль_13_ДЗ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeAccType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Departments", "AccountType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Departments", "AccountType", c => c.Int(nullable: false));
        }
    }
}
