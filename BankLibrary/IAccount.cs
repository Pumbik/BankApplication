using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    interface IAccount
    {
        void Put(decimal sum); //положить деньги на счет
        decimal Withdraw(decimal sum); // снять деньги со счета
    }
}
