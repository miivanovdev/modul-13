using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Модуль_13_ДЗ.Mediators
{
    public class NullObjectDialog : WindowAdapter
    {
        public NullObjectDialog()
            : base(new Window())
        {

        }

        public override void Close()
        {
            MessageBox.Show("Указан неизвестный тип окна! Операция невозможна!");
        }

        public override IWindow CreateChild(object viewModel, Window window)
        {
            MessageBox.Show("Указан неизвестный тип окна! Операция невозможна!");
            return this;
        }

        public override void Show()
        {
            MessageBox.Show("Указан неизвестный тип окна! Операция невозможна!");
        }

        public override bool? ShowDialog()
        {
            MessageBox.Show("Указан неизвестный тип окна! Операция невозможна!");
            return null;
        }

    }
}
