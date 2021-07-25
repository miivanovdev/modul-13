using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.Factories
{
    public interface IMainWindowViewModelFactory
    {
        MainWindowViewModel Create(IWindow window);
    }
}
