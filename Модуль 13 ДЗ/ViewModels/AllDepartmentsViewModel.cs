using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using ModelLib;
using Модуль_13_ДЗ.DataServices;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представлениие всех департаментов
    /// </summary>
    public class AllDepartmentsViewModel : ObservableObject, IAllDepartmentsViewModel
    {
        /// <summary>
        /// Конструктор обеспечивающий загрузку данных модели 
        /// из БД и обертку их в модель представление
        /// </summary>
        /// <param name="repository"></param>
        public AllDepartmentsViewModel(IDepartmentsService service)
        {
            this.service = service;
            Departments = service.GetList();
            SelectedDepartment = Departments.First();
        }

        /// <summary>
        /// Репозиторий департаментов
        /// </summary>
        private readonly IDepartmentsService service;

        /// <summary>
        /// Коллекция моделей представления департаментов
        /// </summary>
        public List<DepartmentsViewModel> Departments { get; set; }

        private DepartmentsViewModel selectedDepartment;

        /// <summary>
        /// Выбранный департамент
        /// </summary>
        public DepartmentsViewModel SelectedDepartment
        {
            get
            {
                return selectedDepartment;
            }
            set
            {
                if (selectedDepartment == value)
                    return;
                                
                selectedDepartment = value;
                NotifyPropertyChanged(nameof(SelectedDepartment));
                onDepartmentSelectionChange?.Invoke(SelectedDepartment);
            }
        }
               
        private event Action<object> onDepartmentSelectionChange;
        /// <summary>
        /// Событие смены выбранного департамента
        /// </summary>
        public event Action<object> OnDepartmentSelectionChange
        {
            add { onDepartmentSelectionChange += value; }
            remove { onDepartmentSelectionChange -= value; }
        }
    }
}
