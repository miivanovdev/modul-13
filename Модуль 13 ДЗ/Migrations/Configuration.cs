namespace Модуль_13_ДЗ.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using ModelLib;

    internal sealed class Configuration : DbMigrationsConfiguration<Модуль_13_ДЗ.BankEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Модуль_13_ДЗ.BankEntities context)
        {
            try
            {
                // This method will be called after migrating to the latest version.

                //  You can use the DbSet<T>.AddOrUpdate() helper extension method
                //  to avoid creating duplicate seed data.
                var Clients = new List<Clients>()
                {
                    new Clients("Кулибяка", "Вадим", "Натанович", 27450, true)  { Id = 1 },
                    new Clients("Пыпырин", "Владимир", "Юльевич", 38850)        { Id = 2 },
                    new Clients("Прокопов", "Алексей", "Александрович", 40450)  { Id = 3 },
                    new Clients("Никишин", "Олег", "Викторович", 120250)        { Id = 4 },
                    new Clients("Крупская", "Анна", "Сергеевна", 117300)        { Id = 5 },
                    new Clients("Коняев", "Станислав", "Валерьевич", 301200)    { Id = 6 },
                    new Clients("Чизмар", "Валентина", "Витальевна", 178500)    { Id = 7 }
                };

                //Clients.ForEach(x => context.Clients.AddOrUpdate(c => new { c.ClientId }, x));
                context.Clients.AddOrUpdate(Clients.ToArray());

                var Departments = new List<Departments>()
                {
                    new Departments() { Name = "Не выбрано!",                                    Id = 1, InterestRate = 0,  MinAmount = 0,      MinTerm = 0,  AccountType = (int)AccountType.Basic },
                    new Departments() { Name = "Отдел по работе с физическими лицами",           Id = 2, InterestRate = 15, MinAmount = 50000,  MinTerm = 6,  AccountType = (int)AccountType.IndividualAccount },
                    new Departments() { Name = "Отдел по работе с юридическими лицами",          Id = 3, InterestRate = 15, MinAmount = 30000,  MinTerm = 12, AccountType = (int)AccountType.PhysicalAccount },
                    new Departments() { Name = "Отдел по работе с привелигированными клиентами", Id = 4, InterestRate = 20, MinAmount = 100000, MinTerm = 18, AccountType = (int)AccountType.PrivilegedAccount }
                };

                context.Departments.AddOrUpdate(Departments.ToArray());

                var Accounts = new List<Accounts>()
                {
                    new Accounts() { OwnerId = Clients[0].Id, OwnerName = Clients[0].FullName, Amount = 40000,  InterestRate = Departments[1].InterestRate, DepartmentId = Departments[1].Id, MinTerm = Departments[1].MinTerm, Delay = 0, CreatedDate = new DateTime(2020, 11, 05), AccountType = Departments[1].AccountType} ,
                    new Accounts() { OwnerId = Clients[0].Id, OwnerName = Clients[0].FullName, Amount = 38120,  InterestRate = Departments[1].InterestRate, DepartmentId = Departments[1].Id, MinTerm = Departments[1].MinTerm, Delay = 0, CreatedDate = new DateTime(2019, 10, 23), AccountType = Departments[1].AccountType} ,
                    new Accounts() { OwnerId = Clients[1].Id, OwnerName = Clients[1].FullName, Amount = 78540,  InterestRate = Departments[1].InterestRate, DepartmentId = Departments[1].Id, MinTerm = Departments[1].MinTerm, Delay = 0, CreatedDate = new DateTime(2020, 09, 04), AccountType = Departments[1].AccountType} ,
                    new Accounts() { OwnerId = Clients[1].Id, OwnerName = Clients[1].FullName, Amount = 53000,  InterestRate = Departments[1].InterestRate, DepartmentId = Departments[1].Id, MinTerm = Departments[1].MinTerm, Delay = 0, CreatedDate = new DateTime(2020, 06, 12), AccountType = Departments[1].AccountType} ,
                    new Accounts() { OwnerId = Clients[2].Id, OwnerName = Clients[2].FullName, Amount = 72600,  InterestRate = Departments[2].InterestRate, DepartmentId = Departments[2].Id, MinTerm = Departments[2].MinTerm, Delay = 0, CreatedDate = new DateTime(2019, 01, 24), AccountType = Departments[2].AccountType} ,
                    new Accounts() { OwnerId = Clients[2].Id, OwnerName = Clients[2].FullName, Amount = 41300,  InterestRate = Departments[2].InterestRate, DepartmentId = Departments[2].Id, MinTerm = Departments[2].MinTerm, Delay = 0, CreatedDate = new DateTime(2021, 01, 13), AccountType = Departments[2].AccountType} ,
                    new Accounts() { OwnerId = Clients[3].Id, OwnerName = Clients[3].FullName, Amount = 41100,  InterestRate = Departments[2].InterestRate, DepartmentId = Departments[2].Id, MinTerm = Departments[2].MinTerm, Delay = 0, CreatedDate = new DateTime(2020, 08, 07), AccountType = Departments[2].AccountType} ,
                    new Accounts() { OwnerId = Clients[3].Id, OwnerName = Clients[3].FullName, Amount = 32500,  InterestRate = Departments[2].InterestRate, DepartmentId = Departments[2].Id, MinTerm = Departments[2].MinTerm, Delay = 0, CreatedDate = new DateTime(2020, 02, 14), AccountType = Departments[2].AccountType} ,
                    new Accounts() { OwnerId = Clients[4].Id, OwnerName = Clients[4].FullName, Amount = 122500, InterestRate = Departments[3].InterestRate, DepartmentId = Departments[3].Id, MinTerm = Departments[3].MinTerm, Delay = 0, CreatedDate = new DateTime(2020, 07, 15), AccountType = Departments[3].AccountType} ,
                    new Accounts() { OwnerId = Clients[5].Id, OwnerName = Clients[5].FullName, Amount = 205331, InterestRate = Departments[3].InterestRate, DepartmentId = Departments[3].Id, MinTerm = Departments[3].MinTerm, Delay = 0, CreatedDate = new DateTime(2018, 10, 12), AccountType = Departments[3].AccountType} ,
                    new Accounts() { OwnerId = Clients[6].Id, OwnerName = Clients[6].FullName, Amount = 157800, InterestRate = Departments[3].InterestRate, DepartmentId = Departments[3].Id, MinTerm = Departments[3].MinTerm, Delay = 0, CreatedDate = new DateTime(2019, 11, 21), AccountType = Departments[3].AccountType}
                };

                context.Accounts.AddOrUpdate(Accounts.ToArray());

                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();

                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}
