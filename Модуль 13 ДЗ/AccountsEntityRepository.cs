using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelLib;

namespace Модуль_13_ДЗ
{
    class AccountsEntityRepository : BaseRepository<Accounts>
    {
        public AccountsEntityRepository(BankEntities context)
            : base(context)
        {
            
        }

    }
}
