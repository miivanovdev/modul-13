using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Модуль_13_ДЗ.Factories;

namespace Модуль_13_ДЗ
{
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
