using System;
using System.Collections.Generic;

namespace ModelLib
{
    public interface IRepository<T>
        where T : class
    {
        IEnumerable<T> GetList();           // получение всех объектов
        T GetOne(int id);                   // получение одного объекта по id
        void Create(T item);                // создание объекта
        void Update(T item);                // обновление объекта
        void UpdateBoth(T item1, T item2);  // обновление объекта
        void Delete(int id);                // удаление объекта по id
    }
}
