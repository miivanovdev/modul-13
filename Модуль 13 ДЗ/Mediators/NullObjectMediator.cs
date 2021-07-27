using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.Mediators
{
    /// <summary>
    /// Нулевой объект посредника
    /// </summary>
    public class NullObjectMediator : TransactionMediator
    {
        public NullObjectMediator(IWindow window)
            : base(window) 
        {

        }

        public override object GetReciever()
        {
            throw new NotImplementedException();
        }

        public override void Transaction()
        {
            window.Show();
        }
    }
}
