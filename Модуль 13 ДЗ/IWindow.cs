using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Модуль_13_ДЗ
{
    public interface IWindow
    {
        void Close();
        IWindow CreateChild(object viewModel, Window window);
        void Show();
        bool? ShowDialog();
    }
}
