namespace Модуль_13_ДЗ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        OwnerName = c.String(nullable: false, maxLength: 50),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        CreatedDate = c.DateTime(nullable: false, storeType: "date"),
                        MinTerm = c.Int(nullable: false),
                        InterestRate = c.Decimal(nullable: false, storeType: "money"),
                        AccountType = c.Int(nullable: false),
                        BadHistory = c.Boolean(nullable: false),
                        Delay = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Clients_Id = c.Int(),
                        Departments_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Clients_Id)
                .ForeignKey("dbo.Departments", t => t.Departments_Id)
                .Index(t => new { t.DepartmentId, t.OwnerId }, name: "IDX_AccountDepartmentOwnerId")
                .Index(t => t.Clients_Id)
                .Index(t => t.Departments_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        SecondName = c.String(nullable: false, maxLength: 50),
                        Surname = c.String(nullable: false, maxLength: 50),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        BadHistory = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        MinAmount = c.Decimal(nullable: false, storeType: "money"),
                        Delay = c.Int(nullable: false),
                        MinTerm = c.Int(nullable: false),
                        InterestRate = c.Decimal(nullable: false, storeType: "money"),
                        AccountType = c.Int(nullable: false),
                        IsBasic = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 200),
                        Time = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "Departments_Id", "dbo.Departments");
            DropForeignKey("dbo.Accounts", "Clients_Id", "dbo.Clients");
            DropIndex("dbo.Accounts", new[] { "Departments_Id" });
            DropIndex("dbo.Accounts", new[] { "Clients_Id" });
            DropIndex("dbo.Accounts", "IDX_AccountDepartmentOwnerId");
            DropTable("dbo.Log");
            DropTable("dbo.Departments");
            DropTable("dbo.Clients");
            DropTable("dbo.Accounts");
        }
    }
}
