using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.MVVM.Model;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Класс расширения методов List<BankAccount>
    /// </summary>
    public static class BankAccountListExt
    {
        /// <summary>
        /// Метод возвращает подмножество счетов с соответствующими идентификаторами
        /// департамента и клиента
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="departmentId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static List<BankAccount> GetAccountsSubset(this List<BankAccount> accounts, int departmentId = 0, int clientId = 0)
        {
            if (clientId == 0 && departmentId > 0)
                return accounts.Where(x => x.DepartmentId == departmentId).ToList();

            if(clientId > 0 && departmentId == 0)
                return accounts.Where(x => x.OwnerId == clientId).ToList();

            if (clientId > 0 && departmentId > 0)
                return accounts.Where(x => x.OwnerId == clientId && x.DepartmentId == departmentId).ToList();

            return accounts;
        }

        /// <summary>
        /// Метод возвращает подмножество счетов соответствующего типа
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public static List<BankAccount> GetAccountsSubset(this List<BankAccount> accounts, AccountType accountType)
        {
            return accounts.Where(x => x.Type == accountType).ToList();
        }

    }
}
