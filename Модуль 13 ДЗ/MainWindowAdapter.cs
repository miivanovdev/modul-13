using System;
using System.Windows;
using System.Windows.Input;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Адаптер главного окна приложения
    /// </summary>
    public class MainWindowAdapter : WindowAdapter
    {
        /// <summary>
        /// Фабрика модели представления
        /// </summary>
        private readonly IMainWindowViewModelFactory vmFactory;
        private bool initialized;

        public MainWindowAdapter(Window wpfWindow, IMainWindowViewModelFactory viewModelFactory)
            : base(wpfWindow)
        {
            if (viewModelFactory == null)
            {
                throw new ArgumentNullException("viewModelFactory");
            }

            this.vmFactory = viewModelFactory;
        }

        #region IWindow Members

        /// <summary>
        /// Закрыть окно
        /// </summary>
        public override void Close()
        {
            this.EnsureInitialized();
            base.Close();
        }

        /// <summary>
        /// Создать окно
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public override IWindow CreateChild(object viewModel, Window window)
        {
            this.EnsureInitialized();
            return base.CreateChild(viewModel, window);
        }

        /// <summary>
        /// Показать окно
        /// </summary>
        public override void Show()
        {
            this.EnsureInitialized();
            base.Show();
        }

        /// <summary>
        /// Показать окно как дилоговое
        /// </summary>
        /// <returns></returns>
        public override bool? ShowDialog()
        {
            this.EnsureInitialized();
            return base.ShowDialog();
        }

        #endregion
        /// <summary>
        /// Привязать клавиши
        /// </summary>
        /// <param name="vm"></param>
        private void DeclareKeyBindings(MainWindowViewModel vm)
        {
            /*
            this.WpfWindow.InputBindings.Add(new KeyBinding(vm.RefreshCommand, new KeyGesture(Key.F5)));
            this.WpfWindow.InputBindings.Add(new KeyBinding(vm.InsertProductCommand, new KeyGesture(Key.Insert)));
            this.WpfWindow.InputBindings.Add(new KeyBinding(vm.EditProductCommand, new KeyGesture(Key.Enter)));
            this.WpfWindow.InputBindings.Add(new KeyBinding(vm.DeleteProductCommand, new KeyGesture(Key.Delete)));
            */
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void EnsureInitialized()
        {
            if (this.initialized)
            {
                return;
            }

            var vm = this.vmFactory.Create(this);
            this.WpfWindow.DataContext = vm;
            this.DeclareKeyBindings(vm);

            this.initialized = true;
        }
    }
}
