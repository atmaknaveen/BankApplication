namespace BankApplication.Tests
{
    [TestClass]
    public class BankTests
    {
        private static Bank s_bank;
        private static string bankName = "MyTestBank";

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            s_bank = new Bank(bankName);

            s_bank.OpenAccount("checkingTest", AccountTypes.Checking);                       //Account No: 1000000000
            s_bank.OpenAccount("checkingTestWithBalance", AccountTypes.Checking, 50.0m);     //Account No: 1000000001

            s_bank.OpenAccount("IndividualInvestmentAccountTest", AccountTypes.IndividualInvestment, 1000.0m); //Account No: 1000000002
            s_bank.OpenAccount("CorporateInvestmentAccountTest", AccountTypes.CorporateInvestment, 500.0m);   //Account No: 1000000003
        }
        
        [TestInitialize]
        public void Setup()
        {
        }
        
        [TestMethod]
        public void CreateAccountsInMultipleTest()
        {
            Parallel.For(0, 10, i => { s_bank.OpenAccount($"CheckingAccount{i}", AccountTypes.Checking); });

            //Verify accounts No: 1000000004 to 1000000013 were created
            for(int i = 1000000004; i < 1000000014; i++)
            {
                var account = s_bank.GetAccount(i);
                Assert.IsNotNull(account);
            }
        }

        //Deposit
        [TestMethod]
        public void TestDepositSuccess()
        {
            Assert.IsTrue(s_bank.Deposit(1000000001, 400.0m));   //account already has 50.0, total will be 450 now
            Assert.AreEqual(450.0m, s_bank.GetAccount(1000000001).GetBalance());
        }

        [TestMethod]
        public void TestDepositInvalidAccount()
        {
            Assert.IsFalse(s_bank.Deposit(100, 400.0m));  // This account number does not exist. we start from 1000000000
        }

        //Withdraw
        [TestMethod]
        public void TestWithdrawSuccess()
        {
            //Account No: 1000000003 CorporateInvestmentAccount, has 500.0 in account
            Assert.IsTrue(s_bank.GetAccount(1000000003).Withdraw(33.0m));
        }

        [TestMethod]
        public void TestIndividualInvestmentAccountWithdrawOverLimit()
        {
            //Account No: 1000000002 IndividualInvestmentAccount, has 1000.0 in account
            Assert.IsFalse(s_bank.GetAccount(1000000002).Withdraw(533.0m));
        }

        [TestMethod]
        public void TestTransferSuccess()
        {
            //Account No: 1000000002 IndividualInvestmentAccount, has 1000.0 in account can transfer 100.0 to Account No: 1000000000
            Assert.IsTrue(s_bank.Transfer(1000000002, 1000000000, 10.0m));
        }

        [TestMethod]
        public void TestTransferFail()
        {
            //Account No: 1000000000 does not have 10000.0
            Assert.IsFalse(s_bank.Transfer(1000000000, 1000000002, 10000.0m));
        }

        [TestMethod]
        public void TestTransferFailureInvalidToAndFromAccounts()
        {
            //From account invalid
            Assert.IsFalse(s_bank.Transfer(1, 1000000000, 6.67m));

            //To account invalid
            Assert.IsFalse(s_bank.Transfer(1000000000, 1, 6.67m));
        }
        //Transfer
        [TestMethod]
        public void VerifyBankHasName()
        {
            Assert.IsTrue(!string.IsNullOrEmpty(s_bank.Name));
            Assert.AreEqual(bankName, s_bank.Name);
        }

        [TestMethod]
        public void VerifyEmptyBankNameThrowsApplicationException()
        {
            Assert.ThrowsException<ApplicationException>(() => new Bank(""));
        }
    }
}