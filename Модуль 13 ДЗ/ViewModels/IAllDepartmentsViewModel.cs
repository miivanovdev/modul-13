using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.ViewModels
{
    public interface IAllDepartmentsViewModel
    {
        List<DepartmentsViewModel> Departments { get; set; }
        DepartmentsViewModel SelectedDepartment { get; set; }
        event Action<object> OnDepartmentSelectionChange;
    }
}
