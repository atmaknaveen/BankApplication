using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public abstract class Account
    {
        public string Owner { get; }
        public int AccountNumber { get;}

        protected decimal Balance;
        
        public Account(string owner, decimal initBalance = 0)
        {
            Owner = owner;
            Balance = initBalance;

            AccountNumber = Bank.GetNextAccountNumber();
        }

        public abstract bool Deposit(decimal amount);

        public abstract bool Withdraw(decimal amount);

        public decimal GetBalance()
        {
            return Balance;
        }

        public abstract bool Transfer(Account toAccount, decimal amount);
    }
}
