using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;
using ModelLib;

namespace Модуль_13_ДЗ.DataServices
{
    public class DepartmentsService : IDepartmentsService
    {
        public DepartmentsService(IRepository<Departments> repository)
        {
            this.repository = repository;
        }

        private readonly IRepository<Departments> repository;

        public DepartmentsViewModel Create()
        {
            Departments item = new Departments(); //фабрика
            repository.Create(item);
            return WrapOne(item);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public List<DepartmentsViewModel> GetList()
        {
            return WrapIntoViewModel(repository.GetList());
        }

        public DepartmentsViewModel GetOne(int id)
        {
            return WrapOne(repository.GetOne(id));
        }

        public void Rollback()
        {
            repository.Rollback();
        }

        public void Update(DepartmentsViewModel item)
        {
            repository.Update(item.Departments);
        }

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

        private DepartmentsViewModel WrapOne(Departments item)
        {
            return new DepartmentsViewModel(item);
        }
    }
}
