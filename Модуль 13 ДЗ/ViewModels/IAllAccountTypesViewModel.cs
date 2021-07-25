using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.ViewModels
{
    public interface IAllAccountTypesViewModel
    {
        IList<AccountTypesViewModel> AccountTypes { get; set; }
        AccountTypesViewModel SelectedType { get; set; }
        RelayCommand AddTypeCommand { get; }
        RelayCommand RemoveTypeCommand { get; }
        RelayCommand UpdateTypeBeginCommand { get; }
        RelayCommand UpdateTypeCommitCommand { get; }
    }
}
