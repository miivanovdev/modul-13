using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class DataInitializer : CreateDatabaseIfNotExists<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            try
            {               
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
                    new Departments() { Name = "Не выбрано!",                                    Id = 1, InterestRate = 0,  MinAmount = 0,      MinTerm = 0 },
                    new Departments() { Name = "Отдел по работе с физическими лицами",           Id = 2, InterestRate = 15, MinAmount = 50000,  MinTerm = 6 },
                    new Departments() { Name = "Отдел по работе с юридическими лицами",          Id = 3, InterestRate = 15, MinAmount = 30000,  MinTerm = 1 },
                    new Departments() { Name = "Отдел по работе с привелигированными клиентами", Id = 4, InterestRate = 20, MinAmount = 100000, MinTerm = 1 }
                };

                context.Departments.AddOrUpdate(Departments.ToArray());

                var AccountTypes = new List<AccountTypes>()
                {
                    new AccountTypes() { Name = "Физический счет",          Id = 1, DepartmentsRefId = 2, CanAdded = true, CanWithdrawed = true, WithdrawingDependsOnMinTerm = true, CanClose = true, ClosingDependsOnMinTerm = true },
                    new AccountTypes() { Name = "Юридический счет",         Id = 2, DepartmentsRefId = 3, CanAdded = true, AddingDependsOnMinTerm = true, CanWithdrawed = true, CanClose = true, ClosingDependsOnMinTerm = true },
                    new AccountTypes() { Name = "Привелигированный счет",   Id = 3, DepartmentsRefId = 4, IsCapitalized = true, CanAdded = true, CanWithdrawed = true, CanClose = true }
                };

                context.AccountTypes.AddOrUpdate(AccountTypes.ToArray());

                var Accounts = new List<Accounts>()
                {
                    new Accounts() {ClientsRefId = Clients[0].Id, ClientsName = Clients[0].FullName, Amount = 40000,  InterestRate = Departments[1].InterestRate, DepartmentsRefId = Departments[1].Id, MinTerm = Departments[1].MinTerm, CreatedDate = new DateTime(2020, 11, 05), AccountTypesId = AccountTypes[0].Id} ,
                    new Accounts() {ClientsRefId = Clients[0].Id, ClientsName = Clients[0].FullName, Amount = 38120,  InterestRate = Departments[1].InterestRate, DepartmentsRefId = Departments[1].Id, MinTerm = Departments[1].MinTerm, CreatedDate = new DateTime(2019, 10, 23), AccountTypesId = AccountTypes[0].Id} ,
                    new Accounts() {ClientsRefId = Clients[1].Id, ClientsName = Clients[1].FullName, Amount = 78540,  InterestRate = Departments[1].InterestRate, DepartmentsRefId = Departments[1].Id, MinTerm = Departments[1].MinTerm, CreatedDate = new DateTime(2020, 09, 04), AccountTypesId = AccountTypes[0].Id} ,
                    new Accounts() {ClientsRefId = Clients[1].Id, ClientsName = Clients[1].FullName, Amount = 53000,  InterestRate = Departments[1].InterestRate, DepartmentsRefId = Departments[1].Id, MinTerm = Departments[1].MinTerm, CreatedDate = new DateTime(2020, 06, 12), AccountTypesId = AccountTypes[0].Id} ,
                    new Accounts() {ClientsRefId = Clients[2].Id, ClientsName = Clients[2].FullName, Amount = 72600,  InterestRate = Departments[2].InterestRate, DepartmentsRefId = Departments[2].Id, MinTerm = Departments[2].MinTerm, CreatedDate = new DateTime(2019, 01, 24), AccountTypesId = AccountTypes[1].Id} ,
                    new Accounts() {ClientsRefId = Clients[2].Id, ClientsName = Clients[2].FullName, Amount = 41300,  InterestRate = Departments[2].InterestRate, DepartmentsRefId = Departments[2].Id, MinTerm = Departments[2].MinTerm, CreatedDate = new DateTime(2021, 01, 13), AccountTypesId = AccountTypes[1].Id} ,
                    new Accounts() {ClientsRefId = Clients[3].Id, ClientsName = Clients[3].FullName, Amount = 41100,  InterestRate = Departments[2].InterestRate, DepartmentsRefId = Departments[2].Id, MinTerm = Departments[2].MinTerm, CreatedDate = new DateTime(2020, 08, 07), AccountTypesId = AccountTypes[1].Id} ,
                    new Accounts() {ClientsRefId = Clients[3].Id, ClientsName = Clients[3].FullName, Amount = 32500,  InterestRate = Departments[2].InterestRate, DepartmentsRefId = Departments[2].Id, MinTerm = Departments[2].MinTerm, CreatedDate = new DateTime(2020, 02, 14), AccountTypesId = AccountTypes[1].Id} ,
                    new Accounts() {ClientsRefId = Clients[4].Id, ClientsName = Clients[4].FullName, Amount = 122500, InterestRate = Departments[3].InterestRate, DepartmentsRefId = Departments[3].Id, MinTerm = Departments[3].MinTerm, CreatedDate = new DateTime(2020, 07, 15), AccountTypesId = AccountTypes[2].Id} ,
                    new Accounts() {ClientsRefId = Clients[5].Id, ClientsName = Clients[5].FullName, Amount = 205331, InterestRate = Departments[3].InterestRate, DepartmentsRefId = Departments[3].Id, MinTerm = Departments[3].MinTerm, CreatedDate = new DateTime(2018, 10, 12), AccountTypesId = AccountTypes[2].Id} ,
                    new Accounts() {ClientsRefId = Clients[6].Id, ClientsName = Clients[6].FullName, Amount = 157800, InterestRate = Departments[3].InterestRate, DepartmentsRefId = Departments[3].Id, MinTerm = Departments[3].MinTerm, CreatedDate = new DateTime(2019, 11, 21), AccountTypesId = AccountTypes[2].Id}

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
