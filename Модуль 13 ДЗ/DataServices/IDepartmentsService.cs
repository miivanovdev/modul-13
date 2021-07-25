using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.DataServices
{
    public interface IDepartmentsService
    {
        List<DepartmentsViewModel> GetList();                      // получение всех объектов
        DepartmentsViewModel GetOne(int id);                       // получение одного объекта по id
        DepartmentsViewModel Create();                             // создание объекта
        void Update(DepartmentsViewModel item);                    // обновление объекта
        void UpdateRange(DepartmentsViewModel[] items);            // обновление нескольких объектов
        void Delete(int id);                                       // удаление объекта по id
        void Rollback();
    }
}
