using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public enum AccountTypes
    {
        Checking,
        IndividualInvestment,
        CorporateInvestment
    }

    public class Bank
    {
        private static int s_StartingAccountNumber = 1000000000;

        private readonly object lockList = new object();
        private List<Account> accounts;
        private IAccountFactory accountFactory;

        public string Name { get; }

        public Bank(string name, IAccountFactory accountFactory = null)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ApplicationException("Bank name cannot be NULL or empty");
            }

            Name = name;
            accounts = new List<Account>();
            
            if(accountFactory == null)
            {
                this.accountFactory = new AccountFactoryImpl();
            }
            else
            {
                this.accountFactory = accountFactory;
            }
        }

        public Account? GetAccount(int accountNumber)
        {
            Account account = null;
            lock (lockList)
            {
                account = accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            }

            if(account == null)
            {
                Console.WriteLine($"Account: {accountNumber} not found.");
            }

            return account;
        }

        public bool Deposit(int toAccountNumber, decimal amount)
        {
            var account = GetAccount(toAccountNumber);
            if(account != null)
            {
                return account.Deposit(amount);
            }

            Console.WriteLine($"Unable to deposit amount: {amount} to account: {toAccountNumber}");
            return false;
        }

        public bool Withdraw(int fromAccountNumber, decimal amount)
        {
            var account = GetAccount(fromAccountNumber);
            if (account != null)
            {
                return account.Withdraw(amount);
            }

            Console.WriteLine($"Unable to deposit amount: {amount} to account: {fromAccountNumber}");
            return false;
        }

        public bool Transfer(int fromAccountNumber, int toAccountNumber, decimal amount)
        {
            var fromAccount = GetAccount(fromAccountNumber);
            var toAccount = GetAccount(toAccountNumber);

            if(fromAccount != null && toAccount != null)
            {
                return fromAccount.Transfer(toAccount, amount);
            }

            Console.WriteLine($"Unable to transfer amount: {amount} from account: {fromAccountNumber} to account: {toAccountNumber}");
            return false;
        }

        public bool OpenAccount(string ownerName, AccountTypes accountType, decimal openingBalance = 0)
        {
            try
            {
                lock (lockList)
                {
                    accounts.Add(accountFactory.CreateAccount(ownerName, accountType, openingBalance));
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to open account of type {accountType.ToString()}. Exception: {ex.ToString()}");
            }
            return false;
        }

        public static int GetNextAccountNumber()
        {
            return s_StartingAccountNumber++;
        }
    }
}
