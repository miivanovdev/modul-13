using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ.Repos
{
    /// <summary>
    /// Хранилище типа счета
    /// </summary>
    public class AccountTypesEntityRepository : BaseRepository<AccountTypes>
    {
        public AccountTypesEntityRepository(BankContext context)
            : base(context)
        {

        }
    }
}
