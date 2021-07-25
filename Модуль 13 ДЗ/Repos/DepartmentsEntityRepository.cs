using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ.Repos
{
    class DepartmentsEntityRepository : BaseRepository<Departments>
    {
        public DepartmentsEntityRepository(BankContext context)
            : base(context)
        {
            
        }
    }
}
