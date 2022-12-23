using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    public interface IAccountFactory
    {
        public Account CreateAccount(string ownerName, AccountTypes accountType, decimal openingBalance = 0);
    }

    public class AccountFactoryImpl : IAccountFactory
    {
        public AccountFactoryImpl()
        {

        }

        public Account CreateAccount(string ownerName, AccountTypes accountType, decimal openingBalance = 0)
        {
            switch (accountType)
            {
                case AccountTypes.Checking:
                    return new CheckingAccount(ownerName, openingBalance);
                case AccountTypes.IndividualInvestment:
                    return new IndividualInvestmentAccount(ownerName, openingBalance);
                case AccountTypes.CorporateInvestment:
                    return new CorporateInvestmentAccount(ownerName, openingBalance);
                default:
                    return new CheckingAccount(ownerName, openingBalance);
            }
        }
    }
}
