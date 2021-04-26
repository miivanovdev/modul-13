using System;
using System.ComponentModel;

namespace ModelLib
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
