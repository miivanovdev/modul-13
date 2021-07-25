using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Модуль_13_ДЗ.Factories
{
    abstract class AbstractFactory
    {
        public abstract object CreateProduct();
    }
}
