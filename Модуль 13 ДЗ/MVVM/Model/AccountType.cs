using System;
using System.ComponentModel;

namespace Модуль_13_ДЗ.MVVM.Model
{
    public enum AccountType
    {
        [Description("Базовый")]
        Basic,
        [Description("Физический")]
        PhysicalAccount,
        [Description("Индивидуальный")]
        IndividualAccount,
        [Description("Привелигированный")]
        PrivilegedAccount
    }
}
