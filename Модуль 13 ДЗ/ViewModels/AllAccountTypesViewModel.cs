using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;
using System.Windows;
using Модуль_13_ДЗ.DataServices;

namespace Модуль_13_ДЗ.ViewModels
{
    public class AllAccountTypesViewModel : ObservableObject, IAllAccountTypesViewModel
    {
        /// <summary>
        /// Конструктор обеспечивающий загрузгу 
        /// </summary>
        /// <param name="repository"></param>
        public AllAccountTypesViewModel(IAccountTypesService service)
        {
            this.service = service;
            AccountTypes = service.GetList();
        }

        /// <summary>
        /// Коллекция моделей представлений типов счетов
        /// </summary>
        public IList<AccountTypesViewModel> AccountTypes { get; set; }

        /// <summary>
        /// Репозиторий типов счетов
        /// </summary>
        private readonly IAccountTypesService service;
                
        private AccountTypesViewModel selectedType;
        /// <summary>
        /// Выбранный тип счета
        /// </summary>
        public AccountTypesViewModel SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                if (selectedType == value)
                    return;

                selectedType = value;
                NotifyPropertyChanged(nameof(SelectedType));
            }
        }
        
        private RelayCommand addTypeCommand;
        /// <summary>
        /// Команда добавления типа счета
        /// </summary>
        public RelayCommand AddTypeCommand
        {
            get
            {
                return addTypeCommand ??
                (addTypeCommand = new RelayCommand(new Action<object>(AddType),
                                                        new Func<object, bool>(TypeCanBeAdded)
                ));
            }
        }

        /// <summary>
        /// Проверка возможности вызова команды добавления
        /// типа счета
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool TypeCanBeAdded(object o)
        {
            return true;
        }

        /// <summary>
        /// Метод добавления нового типа счета
        /// </summary>
        /// <param name="o"></param>
        private void AddType(object o)
        {
            AccountTypes.Add(service.Create());
        }

        private RelayCommand removeTypeCommand;
        /// <summary>
        /// Команда удаления типа счета
        /// </summary>
        public RelayCommand RemoveTypeCommand
        {
            get
            {
                return removeTypeCommand ??
                (removeTypeCommand = new RelayCommand(new Action<object>(RemoveType),
                                                        new Func<object, bool>(CanTypeBeRemoved)
                ));
            }
        }

        /// <summary>
        /// Проверка возможности удаления типа счета
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CanTypeBeRemoved(object o)
        {
            return SelectedType != null;
        }

        /// <summary>
        /// Удалить тип счета
        /// </summary>
        /// <param name="o"></param>
        private void RemoveType(object o)
        {
            try
            {
                service.Delete(SelectedType.AccountTypesId);

                if (!AccountTypes.Remove(SelectedType))
                    throw new Exception("Не удалось удалить тип счета!");

                if (AccountTypes.Count > 0)
                    SelectedType = AccountTypes.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private RelayCommand updateTypeBeginCommand;
        /// <summary>
        /// Команда при входе в режима редактирования
        /// </summary>
        public RelayCommand UpdateTypeBeginCommand
        {
            get
            {
                return updateTypeBeginCommand ??
                (updateTypeBeginCommand = new RelayCommand(new Action<object>(RowEditBegin)));
            }
        }

        public void RowEditBegin(object args)
        {
            SelectedType.BeginEdit();
        }

        private RelayCommand updateTypeCommitCommand;
        /// <summary>
        /// Команда при выходе из режима редактирования
        /// </summary>
        public RelayCommand UpdateTypeCommitCommand
        {
            get
            {
                return updateTypeCommitCommand ??
                (updateTypeCommitCommand = new RelayCommand(new Action<object>(RowEditEnding)));
            }
        }

        /// <summary>
        /// Метод отвечающий за сохранение изменений 
        /// тип счета, если такие имеются
        /// </summary>
        /// <param name="args"></param>
        private void RowEditEnding(object args)
        {
            if (SelectedType.HasChanges)
            {
                var result = MessageBox.Show(App.Current.MainWindow, "Вы хотите сохранить изменения?", $"Изменения в типе счета {SelectedType.Name}", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        service.Update(SelectedType);
                        SelectedType.EndEdit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        SelectedType.CancelEdit();
                    }
                }
                else
                {
                    SelectedType.CancelEdit();
                }
            }
        }
    }        
}
