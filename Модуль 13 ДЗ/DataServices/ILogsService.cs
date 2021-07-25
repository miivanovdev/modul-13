using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.DataServices
{
    public interface ILogsService
    {
        List<LogViewModel> GetList();                      // получение всех объектов
        LogViewModel GetOne(int id);                       // получение одного объекта по id
        LogViewModel Create(string message);                    // создание объекта
        void Update(LogViewModel item);                    // обновление объекта
        void UpdateRange(LogViewModel[] items);            // обновление нескольких объектов
        void Delete(int id);                               // удаление объекта по id
        void Rollback();
    }
}
