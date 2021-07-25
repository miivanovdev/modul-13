using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.ViewModels
{
    public interface IAllClientsViewModel
    {
        IList<ClientsViewModel> Clients { get; set; }
        event Action<object> OnClientSelectionChange;
        ClientsViewModel SelectedClient { get; set; }
        RelayCommand AddClientCommand { get; }
        RelayCommand RemoveClientCommand { get; }
        RelayCommand UpdateClientBeginCommand { get; }
        RelayCommand UpdateClientCommitCommand { get; }
    }
}
