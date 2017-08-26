using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
   public abstract class Account: IAccount
    {
        //Событие возникающее при выводе денег
        protected internal event AccountStateHandler Withdrawed;
        //Событие возникающее при добавлении счета
        protected internal event AccountStateHandler Added;
        //Событие- при открытии счета
        protected internal event AccountStateHandler Opened;
        //Событие- при закрытии счета
        protected internal event AccountStateHandler Closed;
        //Событие- начисление процентов
        protected internal event AccountStateHandler Calculated;

        protected int _id;
        static int counter = 0;

        protected decimal _sum; //переменная для хранения суммы
        protected int _percentage; // переменная для хранения процента

        protected int _days = 0; // время с момента открытия счета

        public Account(decimal sum, int percentage)
        {
            _sum = sum;
            _percentage = percentage;
            _id = ++counter;
        }

        public decimal CurrentSum // текущая сумма на счету
        { get { return _sum; } }

        public int Id
        { get { return _id; } }

        //вызoв событий
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        //вызов отдельных событий. Для каждого события определяется свой витруальный метод
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }
        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected internal virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        public virtual void Put(decimal sum)
        {
            _sum = sum;
            OnAdded(new AccountEventArgs("На счет поступило " + sum, sum));
            //OnAdded(new AccountEventArgs($"На счет поступило {sum}", sum));
        }
        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if(sum<=_sum)
            {
                _sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs("Сумма " + sum + "снята со счета  " + _id, sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs("Недостаточно денег на с счете " + _id, sum));
            }
            return result;
        }

        //открытие счета
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs("Открыт новый депозитный счет! Id счета: " + this._id, this._sum));
        }

        //закрытие счета
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs("Счет " + _id + " закрыт. Итоговая сумма: " + CurrentSum, CurrentSum));
        }

        protected internal void IncrementDays()
        {
            _days++;
        }

        //начисление процентов
        protected internal virtual void Calculate()
        {
            decimal increment = _sum * _percentage / 100;
            _sum = _sum + increment;
            OnCalculated(new AccountEventArgs("Начислены проценты в размере: " + increment, increment));
        }
    }
}
