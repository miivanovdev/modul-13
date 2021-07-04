using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class ClientEntitiesRepository : BaseRepository<Clients>
    {
        public ClientEntitiesRepository(BankEntities context)
            : base(context)
        {
            
        }

    }
}
