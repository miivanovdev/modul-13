using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ModelLib;

namespace Модуль_13_ДЗ.Repos
{
    public class ClientEntitiesRepository : BaseRepository<Clients>
    {
        public ClientEntitiesRepository(BankContext context)
            : base(context)
        {
            
        }

    }
}
