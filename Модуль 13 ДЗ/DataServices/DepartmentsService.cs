using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;
using ModelLib;

namespace Модуль_13_ДЗ.DataServices
{
    /// <summary>
    /// Сервис департаментов
    /// </summary>
    public class DepartmentsService : IDepartmentsService
    {
        public DepartmentsService(IRepository<Departments> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Интерфейс взаймодействия с хранилищем
        /// </summary>
        private readonly IRepository<Departments> repository;

        /// <summary>
        /// Создать департамент
        /// </summary>
        /// <returns></returns>
        public DepartmentsViewModel Create()
        {
            Departments item = new Departments(); 
            repository.Create(item);
            return WrapOne(item);
        }

        /// <summary>
        /// Удалить департамент по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            repository.Delete(id);
        }

        /// <summary>
        /// Получить список департаментов
        /// </summary>
        /// <returns></returns>
        public List<DepartmentsViewModel> GetList()
        {
            return WrapIntoViewModel(repository.GetList());
        }

        /// <summary>
        /// Получить департамент по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DepartmentsViewModel GetOne(int id)
        {
            return WrapOne(repository.GetOne(id));
        }

        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        public void Rollback()
        {
            repository.Rollback();
        }

        /// <summary>
        /// Обновить департамент
        /// </summary>
        /// <param name="item"></param>
        public void Update(DepartmentsViewModel item)
        {
            repository.Update(item.Departments);
        }

        /// <summary>
        /// Обновить несколько департаментов
        /// </summary>
        /// <param name="items"></param>
        public void UpdateRange(DepartmentsViewModel[] items)
        {
            List<Departments> itemsModel = new List<Departments>();

            foreach (var i in items)
                itemsModel.Add(i.Departments);

            repository.UpdateRange(itemsModel.ToArray());
        }

        /// <summary>
        /// Метод оборачивает коллекцию моделей типов счетов
        /// в модель представление
        /// </summary>
        /// <param name="list"></param>
        private List<DepartmentsViewModel> WrapIntoViewModel(IEnumerable<Departments> list)
        {
            List<DepartmentsViewModel> Departments = new List<DepartmentsViewModel>();

            foreach (var l in list)
                Departments.Add(WrapOne(l));

            return Departments;
        }

        /// <summary>
        /// Обернуть модель в представление
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private DepartmentsViewModel WrapOne(Departments item)
        {
            return new DepartmentsViewModel(item);
        }
    }
}
