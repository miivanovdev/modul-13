using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представлениие всех департаментов
    /// </summary>
    public class AllBankDepartmentViewModel : ObservableObject
    {
        /// <summary>
        /// Конструктор обеспечивающий загрузку данных модели 
        /// из БД и обертку их в модель представление
        /// </summary>
        /// <param name="repository"></param>
        public AllBankDepartmentViewModel(IRepository<Departments> repository)
        {
            Repository = repository;

            try
            {
                WrapIntoViewModel(Repository.GetList());
                SelectedDepartment = Departments.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Репозиторий департаментов
        /// </summary>
        private readonly IRepository<Departments> Repository;

        /// <summary>
        /// Коллекция моделей представления департаментов
        /// </summary>
        public List<BankDepartmentViewModel> Departments { get; set; }

        private BankDepartmentViewModel selectedDepartment;

        /// <summary>
        /// Выбранный департамент
        /// </summary>
        public BankDepartmentViewModel SelectedDepartment
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

        /// <summary>
        /// Метод оборачивающий коллекцию моделей в модели представления
        /// </summary>
        /// <param name="list"></param>
        public void WrapIntoViewModel(IEnumerable<Departments> list)
        {
            Departments = new List<BankDepartmentViewModel>();

            foreach (var l in list)
                Departments.Add(new BankDepartmentViewModel(l));
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
