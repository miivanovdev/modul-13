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
                        DepartmentsRefId = c.Int(),
                        ClientsRefId = c.Int(nullable: false),
                        ClientsName = c.String(nullable: false, maxLength: 50),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        CreatedDate = c.DateTime(nullable: false, storeType: "date"),
                        MinTerm = c.Int(nullable: false),
                        InterestRate = c.Decimal(nullable: false, storeType: "money"),
                        BadHistory = c.Boolean(nullable: false),
                        AccountTypesId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Departments_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountTypes", t => t.AccountTypesId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Departments_Id)
                .ForeignKey("dbo.Clients", t => t.ClientsRefId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentsRefId)
                .Index(t => new { t.DepartmentsRefId, t.ClientsRefId }, name: "IDX_AccountDepartmentOwnerId")
                .Index(t => t.AccountTypesId)
                .Index(t => t.Departments_Id);
            
            CreateTable(
                "dbo.AccountTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CanWithdrawed = c.Boolean(nullable: false),
                        CanAdded = c.Boolean(nullable: false),
                        CanClose = c.Boolean(nullable: false),
                        AddingDependsOnMinTerm = c.Boolean(nullable: false),
                        WithdrawingDependsOnMinTerm = c.Boolean(nullable: false),
                        ClosingDependsOnMinTerm = c.Boolean(nullable: false),
                        IsCapitalized = c.Boolean(nullable: false),
                        AllowedRevision = c.Boolean(nullable: false),
                        DepartmentsRefId = c.Int(),
                        Name = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Departments_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Departments_Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsRefId)
                .Index(t => t.DepartmentsRefId)
                .Index(t => t.Departments_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        MinAmount = c.Decimal(nullable: false, storeType: "money"),
                        MinTerm = c.Int(nullable: false),
                        InterestRate = c.Decimal(nullable: false, storeType: "money"),
                        AccountType = c.Int(nullable: false),
                        IsBasic = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.Accounts", "DepartmentsRefId", "dbo.Departments");
            DropForeignKey("dbo.Accounts", "ClientsRefId", "dbo.Clients");
            DropForeignKey("dbo.AccountTypes", "DepartmentsRefId", "dbo.Departments");
            DropForeignKey("dbo.AccountTypes", "Departments_Id", "dbo.Departments");
            DropForeignKey("dbo.Accounts", "Departments_Id", "dbo.Departments");
            DropForeignKey("dbo.Accounts", "AccountTypesId", "dbo.AccountTypes");
            DropIndex("dbo.AccountTypes", new[] { "Departments_Id" });
            DropIndex("dbo.AccountTypes", new[] { "DepartmentsRefId" });
            DropIndex("dbo.Accounts", new[] { "Departments_Id" });
            DropIndex("dbo.Accounts", new[] { "AccountTypesId" });
            DropIndex("dbo.Accounts", "IDX_AccountDepartmentOwnerId");
            DropTable("dbo.Log");
            DropTable("dbo.Clients");
            DropTable("dbo.Departments");
            DropTable("dbo.AccountTypes");
            DropTable("dbo.Accounts");
        }
    }
}
