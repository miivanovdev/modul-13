using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Адаптер окна
    /// </summary>
    public class WindowAdapter : IWindow
    {
        private readonly Window wpfWindow;

        public WindowAdapter(Window wpfWindow)
        {
            if (wpfWindow == null)
            {
                throw new ArgumentNullException("window");
            }

            this.wpfWindow = wpfWindow;
        }

        #region IWindow Members
        /// <summary>
        /// Закрыть окно
        /// </summary>
        public virtual void Close()
        {
            this.wpfWindow.Close();
        }

        /// <summary>
        /// Создать дочернее окно
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public virtual IWindow CreateChild(object viewModel, Window window)
        {
            var cw = window;
            cw.Owner = this.wpfWindow;
            cw.DataContext = viewModel;
            WindowAdapter.ConfigureBehavior(cw);

            return new WindowAdapter(cw);
        }

        /// <summary>
        /// Показать окно
        /// </summary>
        public virtual void Show()
        {
            this.wpfWindow.Show();
        }

        /// <summary>
        /// Показать окно как диалоговое
        /// </summary>
        /// <returns></returns>
        public virtual bool? ShowDialog()
        {
            return this.wpfWindow.ShowDialog();
        }

        #endregion
        /// <summary>
        /// Адаптируемое окно
        /// </summary>
        protected Window WpfWindow
        {
            get { return this.wpfWindow; }
        }

        /// <summary>
        /// Сконфигурировать поведение окна
        /// </summary>
        /// <param name="cw"></param>
        private static void ConfigureBehavior(Window cw)
        {
            cw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cw.CommandBindings.Add(new CommandBinding(PresentationCommands.Accept, (sender, e) => cw.DialogResult = true));
        }
    }
}
