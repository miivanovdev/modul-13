using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ
{
    public class TransactionViewModel : ObservableObject
    {
        public TransactionViewModel(List<DepartmentsViewModel> departments, List<AccountsViewModel> accounts, decimal CurrentAmount)
        {
            if (accounts == null || departments == null || CurrentAmount == 0)
                throw new TransactionFailureException("Неверно переданы счета или отсутствует счет отправитель!");

            this.Departments = departments;
            SelectedDepartment = departments.First();
            this.Accounts = accounts;
            Data = new DialogDataModel("Перевод", CurrentAmount, true);
        }

        public decimal Amount { get { return Data.Amount; } }

        public DialogDataModel Data { get; set; }

        /// <summary>
        /// Все счета
        /// </summary>
        private List<AccountsViewModel> Accounts;

        public List<DepartmentsViewModel> Departments { get; private set; }

        /// <summary>
        /// Отфильтрованные счета
        /// </summary>
        public ReadOnlyCollection<AccountsViewModel> AccountViews
        {
            get { return new ReadOnlyCollection<AccountsViewModel>(GetSubset()); }
        }

        /// <summary>
        /// Фильтр по департаменту
        /// </summary>
        private DepartmentsViewModel selectedDepartment;
        public DepartmentsViewModel SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                if (selectedDepartment != value)
                {
                    selectedDepartment = value;
                    NotifyPropertyChanged(nameof(SelectedDepartment));
                    NotifyPropertyChanged(nameof(AccountViews));
                }
            }
        }

        private List<AccountsViewModel> GetSubset()
        {
            return Accounts.Where(x => x.DepartmentId == SelectedDepartment.DepartmentId).ToList();
        }

        /// <summary>
        /// Результат диалога
        /// </summary>
        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                if (dialogResult == value)
                    return;

                dialogResult = value;
                NotifyPropertyChanged(nameof(DialogResult));
            }
        }

        private AccountsViewModel selectedAccount;
        /// <summary>
        /// Выбранный счет получатель
        /// </summary>
        public AccountsViewModel SelectedAccount
        {
            get
            {
                return selectedAccount;
            }
            set
            {
                if (selectedAccount == value)
                    return;

                selectedAccount = value;
                Data.TotalAmount = selectedAccount.Amount;
                NotifyPropertyChanged(nameof(SelectedAccount));
            }
        }

        private RelayCommand okCommand;
        /// <summary>
        /// Команда закрытия при положительном результате диалога
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return okCommand ??
                (okCommand = new RelayCommand(new Action<object>(OkClose),
                                              new Func<object, bool>(CanClose)
                ));
            }
        }

        /// <summary>
        /// Положительное закрытие диалога
        /// </summary>
        /// <param name="o"></param>
        private void OkClose(object o)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Проверка допустимости положительного исхода диалога
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CanClose(object o)
        {
            return SelectedAccount != null &&
                   Data.IsValid;
        }
    }
}
