using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.DataServices
{
    public interface IAccountTypesService
    {
        List<AccountTypesViewModel> GetList();               // получение всех объектов
        AccountTypesViewModel GetOne(int id);                       // получение одного объекта по id
        AccountTypesViewModel Create();                             // создание объекта
        void Update(AccountTypesViewModel item);                    // обновление объекта
        void UpdateRange(AccountTypesViewModel[] items);            // обновление нескольких объектов
        void Delete(int id);                                        // удаление объекта по id
        void Rollback();
    }
}
