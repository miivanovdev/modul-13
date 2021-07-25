using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelLib;

namespace Модуль_13_ДЗ.Repos
{
    class AccountsEntityRepository : BaseRepository<Accounts>
    {
        public AccountsEntityRepository(BankContext context)
            : base(context)
        {
            
        }

        /// <summary>
        /// Жадная загрузка счета вместе 
        /// с его типом счета
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Accounts> GetList()
        {
            return context.Accounts.Include(a => a.AccountTypes).ToList();
        }
    }
}
