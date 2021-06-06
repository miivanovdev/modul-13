using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace ModelLib
{
    public enum AccountType
    {
        [Description("Базовый")]
        Basic = 0,
        [Description("Физический")]
        PhysicalAccount = 1,
        [Description("Индивидуальный")]
        IndividualAccount = 2,
        [Description("Привелигированный")]
        PrivilegedAccount = 3
    }
    
}
