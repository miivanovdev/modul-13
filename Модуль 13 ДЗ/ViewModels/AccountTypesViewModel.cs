using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    public class AccountTypesViewModel : ObservableObject, IDataErrorInfo, IEditableObject
    {
        public AccountTypesViewModel(AccountTypes accountTypes)
        {
            this.accountTypes = accountTypes;
        }

        private readonly AccountTypes accountTypes;

        public AccountTypes AccountTypes { get { return accountTypes; } }

        public int AccountTypesId
        {
            get { return accountTypes.Id; }
            set
            {
                if (value == accountTypes.Id)
                    return;

                accountTypes.Id = value;

                NotifyPropertyChanged(nameof(AccountTypesId));
            }
        }

        public string Name
        {
            get { return accountTypes.Name; }
            set
            {
                if (value == accountTypes.Name)
                    return;

                accountTypes.Name = value;
                
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public bool CanAdded
        {
            get { return accountTypes.CanAdded; }
            set
            {
                if (value == accountTypes.CanAdded)
                    return;

                accountTypes.CanAdded = value;

                if (value == false)
                    AddingDependsOnMinTerm = false;

                NotifyPropertyChanged(nameof(CanAdded));
            }
        }

        public bool CanWithdrawed
        {
            get { return accountTypes.CanWithdrawed; }
            set
            {
                if (value == accountTypes.CanWithdrawed)
                    return;

                accountTypes.CanWithdrawed = value;

                if (value == false)
                    WithdrawingDependsOnMinTerm = false;

                NotifyPropertyChanged(nameof(CanWithdrawed));
            }
        }

        public bool CanClose
        {
            get { return accountTypes.CanClose; }
            set
            {
                if (value == accountTypes.CanClose)
                    return;

                accountTypes.CanClose = value;

                if (value == false)
                    ClosingDependsOnMinTerm = false;

                NotifyPropertyChanged(nameof(CanClose));
            }
        }

        public bool AddingDependsOnMinTerm
        {
            get { return accountTypes.AddingDependsOnMinTerm; }
            set
            {
                if (value == accountTypes.AddingDependsOnMinTerm)
                    return;                    

                accountTypes.AddingDependsOnMinTerm = value;

                NotifyPropertyChanged(nameof(AddingDependsOnMinTerm));
            }
        }

        public bool WithdrawingDependsOnMinTerm
        {
            get { return accountTypes.WithdrawingDependsOnMinTerm; }
            set
            {
                if (value == accountTypes.WithdrawingDependsOnMinTerm)
                    return;

                accountTypes.WithdrawingDependsOnMinTerm = value;

                NotifyPropertyChanged(nameof(WithdrawingDependsOnMinTerm));
            }
        }

        public bool ClosingDependsOnMinTerm
        {
            get { return accountTypes.ClosingDependsOnMinTerm; }
            set
            {
                if (value == accountTypes.ClosingDependsOnMinTerm)
                    return;

                accountTypes.ClosingDependsOnMinTerm = value;

                NotifyPropertyChanged(nameof(ClosingDependsOnMinTerm));
            }
        }

        public bool IsCapitalized
        {
            get { return accountTypes.IsCapitalized; }
            set
            {
                if (value == accountTypes.IsCapitalized)
                    return;

                accountTypes.IsCapitalized = value;

                NotifyPropertyChanged(nameof(IsCapitalized));
            }
        }

        public bool AllowedRevision
        {
            get { return accountTypes.AllowedRevision; }
            set
            {
                if (value == accountTypes.AllowedRevision)
                    return;

                accountTypes.AllowedRevision = value;

                NotifyPropertyChanged(nameof(AllowedRevision));
            }
        }

        public int? DepartmentsRefId
        {
            get { return accountTypes.DepartmentsRefId; }
            set
            {
                if (value == accountTypes.DepartmentsRefId)
                    return;

                accountTypes.DepartmentsRefId = value;

                NotifyPropertyChanged(nameof(DepartmentsRefId));
            }
        }

        public bool HasChanges
        {
            get
            {
                if (OriginAccountTypes == null)
                    return false;

                if( this.Name                           != OriginAccountTypes.Name                          ||
                    this.CanAdded                       != OriginAccountTypes.CanAdded                      ||
                    this.CanClose                       != OriginAccountTypes.CanClose                      ||
                    this.CanWithdrawed                  != OriginAccountTypes.CanWithdrawed                 ||
                    this.AddingDependsOnMinTerm         != OriginAccountTypes.AddingDependsOnMinTerm        ||
                    this.ClosingDependsOnMinTerm        != OriginAccountTypes.ClosingDependsOnMinTerm       ||
                    this.WithdrawingDependsOnMinTerm    != OriginAccountTypes.WithdrawingDependsOnMinTerm   ||
                    this.IsCapitalized                  != OriginAccountTypes.IsCapitalized                 ||
                    this.DepartmentsRefId               != OriginAccountTypes.DepartmentsRefId)
                    return true;

                return false;
            }
        }       
                
        public string this[string columnName]
        {
            get
            {
                Error = String.Empty;
                switch (columnName)
                {
                    case nameof(Name):

                        if (Name == string.Empty)
                        {
                            Error = "Введите имя!";
                        }
                        else
                        {
                            if (Name.Length > 25)
                                Error = "Слишком длинное имя!";
                        }

                        break;
                }

                return Error;
            }
        }

        public string Error { get; set; }

        private AccountTypes OriginAccountTypes { get; set; }

        public void BeginEdit()
        {
            if (OriginAccountTypes == null)
                OriginAccountTypes = new AccountTypes()
                {
                    Name                            = this.Name,
                    CanAdded                        = this.CanAdded,
                    CanClose                        = this.CanClose,
                    CanWithdrawed                   = this.CanWithdrawed,
                    AddingDependsOnMinTerm          = this.AddingDependsOnMinTerm,
                    ClosingDependsOnMinTerm         = this.ClosingDependsOnMinTerm,
                    WithdrawingDependsOnMinTerm     = this.WithdrawingDependsOnMinTerm,
                    IsCapitalized                   = this.IsCapitalized,
                    DepartmentsRefId                = this.accountTypes.DepartmentsRefId
                };
        }

        public void CancelEdit()
        {
            if (!HasChanges)
                return;

            this.Name                           = OriginAccountTypes.Name;
            this.CanAdded                       = OriginAccountTypes.CanAdded;
            this.CanClose                       = OriginAccountTypes.CanClose;
            this.CanWithdrawed                  = OriginAccountTypes.CanWithdrawed;
            this.AddingDependsOnMinTerm         = OriginAccountTypes.AddingDependsOnMinTerm;
            this.ClosingDependsOnMinTerm        = OriginAccountTypes.ClosingDependsOnMinTerm;
            this.WithdrawingDependsOnMinTerm    = OriginAccountTypes.WithdrawingDependsOnMinTerm;
            this.IsCapitalized                  = OriginAccountTypes.IsCapitalized;
            this.accountTypes.DepartmentsRefId  = OriginAccountTypes.DepartmentsRefId;
        }

        public void EndEdit()
        {
            OriginAccountTypes = null;
        }
    }
}
