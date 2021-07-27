using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Контейнер приложения банка
    /// </summary>
    public class BankAppClientContainer
    {
        public IWindow ResolveWindow()
        {
            IMainWindowViewModelFactory vmFactory =
                new MainWindowViewModelFactory(new BankContext(), new DataInitializer());

            Window mainWindow = new MainWindow();
            IWindow w =
                new MainWindowAdapter(mainWindow, vmFactory);
            return w;
        }
    }
}
