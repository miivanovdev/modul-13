using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.DataServices
{
    public interface IClientsService
    {
        List<ClientsViewModel> GetList();                      // получение всех объектов
        ClientsViewModel GetOne(int id);                       // получение одного объекта по id
        void Create(ClientsViewModel item);                    // создание объекта
        void Update(ClientsViewModel item);                    // обновление объекта
        void UpdateRange(ClientsViewModel[] items);            // обновление нескольких объектов
        void Delete(int id);                                    // удаление объекта по id
        void Rollback();
    }
}
