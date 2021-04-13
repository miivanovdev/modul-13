using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Модуль_13_ДЗ
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // <summary>
        /// Оповещение подписавшихся объектов
        /// </summary>
        /// <param name="propertyName">Изменеяемое свойство</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
