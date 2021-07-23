using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EBankingDbContext = EBanking.Data.EBankingDbContext;

namespace EBanking
{
    class EBank
    {

        EBankingDbContext _db;
        Users _users;

        public EBank()
        {
            _db = new EBankingDbContext("./db.txt");
            _users = new Users(_db);

            // Add user
            //Console.WriteLine(_users.add("Alex1", "12345", "Random Name", "test@test.test"));
            //Console.WriteLine(_users.add("Alex2", "12345", "Random Name", "test@test.test"));

            // Add user account
            //Console.WriteLine(_users.addUserAccount("Alex1", "Savings", Guid.NewGuid()));
            //Console.WriteLine(_users.addUserAccount("Alex2", "test", Guid.NewGuid()));

            // Deposit
            Console.WriteLine(_users.UserAccounts.deposit(Guid.Parse("02a581be-f9da-4442-8080-0e6d0aca87d4"), 1000));

            // Add transaction
            Console.WriteLine("TX " + _users.UserAccounts.sendToUser(Guid.Parse("02a581be-f9da-4442-8080-0e6d0aca87d4"), Guid.Parse("6f634ad4-c7c3-409d-bd01-d1874d6d0c9f"), 150));

            // Withdraw
            Console.WriteLine("Withdraw " + _users.UserAccounts.withdraw(Guid.Parse("02a581be-f9da-4442-8080-0e6d0aca87d4"), 200));
        }


    }
}
