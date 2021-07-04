using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ
{
    class DepartmentsEntityRepository : BaseRepository<Departments>
    {
        public DepartmentsEntityRepository(BankEntities context)
            : base(context)
        {
            
        }
    }
}
