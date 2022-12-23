using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public class CheckingAccount: Account
    {
        public CheckingAccount(string owner, decimal initBalance = 0)
            : base(owner, initBalance)
        {

        }

        public override bool Deposit(decimal amount)
        {
            if (amount > 0)
            {
                lock(this)
                {
                    Balance += amount;
                }
                
                Console.WriteLine($"Successfully Deposited {amount} into {AccountNumber} ");
                return true;
            }

            Console.WriteLine($"Unable to deposit {amount} into {AccountNumber}");
            return false;
        }

        public override bool Transfer(Account toAccount, decimal amount)
        {
            if (Withdraw(amount))
            {
                // make this atomic
                toAccount.Deposit(amount);
                Console.WriteLine($"Successfully Transferred {amount} to {toAccount.AccountNumber} ");
                return true;
            }

            Console.WriteLine($"Unable to Transfer {amount} to {toAccount.AccountNumber} ");
            return false;
        }

        public override bool Withdraw(decimal amount)
        {
            if (amount <= Balance)
            {
                lock (this) 
                { 
                    Balance -= amount; 
                }
                
                Console.WriteLine($"Successfully Withdrawn {amount} from {AccountNumber} ");
                return true;
            }

            Console.WriteLine($"Unable to Withdrawn {amount} from {AccountNumber} ");
            return false;
        }
    }
}
