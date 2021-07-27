using System;
using ModelLib;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.Dialogs
{
    /// <summary>
    /// Модель-представление ввода клиента
    /// </summary>
    public class ClientInputViewModel : ObservableObject
    {
        public ClientInputViewModel()
        {
            Client = new ClientsViewModel(new Clients());
        }

        /// <summary>
        /// Модель представление клиента
        /// </summary>
        public ClientsViewModel Client { get; set; }

        private bool? dialogResult;
        /// <summary>
        /// Флаг закрытия окна диалога
        /// </summary>
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

        private RelayCommand okCommand;
        /// <summary>
        /// Команда закрытия окна
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
        /// Метод закрытия окна
        /// </summary>
        /// <param name="o"></param>
        private void OkClose(object o)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Проверка доступности закрытия
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CanClose(object o)
        {
            return Client.Error == string.Empty;
        }
    }
}
