using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.DataServices
{
    public interface IAccountsService
    {
        List<AccountsViewModel> GetList();                      // получение всех объектов
        AccountsViewModel GetOne(int id);                       // получение одного объекта по id

        AccountsViewModel Create(int ownerId, string ownerName, decimal amount, decimal interestRate,
                                 int departmentId, int minTerm, int accountTypeId);                             // создание объекта

        void Update(AccountsViewModel item);                    // обновление объекта
        void UpdateRange(AccountsViewModel[] items);            // обновление нескольких объектов
        void Delete(int id);                                    // удаление объекта по id
        void Rollback();
    }
}
