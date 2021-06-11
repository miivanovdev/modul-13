using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class AllBankDepartmentViewModel : ObservableObject
    {
        public AllBankDepartmentViewModel(IRepository<BankDepartment> repository)
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

        private readonly IRepository<BankDepartment> Repository;

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

        public void WrapIntoViewModel(IEnumerable<BankDepartment> list)
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
            add
            {
                onDepartmentSelectionChange += value;
            }
            remove { onDepartmentSelectionChange -= value; }
        }


    }
}
