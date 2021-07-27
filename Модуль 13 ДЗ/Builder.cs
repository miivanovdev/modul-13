using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Модуль_13_ДЗ
{
    public class Builder
    {
        public Builder(BankContext context)
        {
            this.context = context;
        }

        private readonly BankContext context;

        public void CompleteObject(object target)
        {
            var props = target.GetType().GetProperties();

            foreach (var prop in  props)
            {
                if(prop.CanWrite)
                    prop.SetValue(target, Build(prop.PropertyType));
            }
        }

        private object Build(Type type)
        {
            var ctors = type.GetConstructors().Where(x => x.IsStatic == false && x.GetParameters().Length > 0);
            ConstructorInfo ctor = ctors.First();

            List<object> listParams = new List<object>();
                        
            foreach (var param in ctor.GetParameters())
            {
                if(param.ParameterType == typeof(BankContext))
                {
                    listParams.Add(context);
                    continue;
                }

                if (param.GetType().IsInterface == true)
                {
                    var types = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => param.GetType().IsAssignableFrom(p));

                    listParams.Add(Build(types.First()));
                }

                listParams.Add(Build(param.GetType()));
            }
            
            return ctor.Invoke(listParams.ToArray());
        }
    }
}

