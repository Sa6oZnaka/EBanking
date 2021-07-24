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
            //_users.add("Alex7", "12345678", "Random Name", "alex@mail.bg");
            //_users.add("Alex2", "12345", "Random Name", "test@test.test");

            //Console.WriteLine(_users.authenticate("Alex1", "12345"));
            //Console.WriteLine(_users.authenticate("Alex1", "123451"));

            // Add user account
            //Console.WriteLine(_users.addUserAccount("Alex1", "Savings", Guid.NewGuid()));
            //Console.WriteLine(_users.addUserAccount("Alex2", "test", Guid.NewGuid()));

            // Deposit
            //Console.WriteLine(_users.UserAccounts.deposit(Guid.Parse("46adc225-80ec-478d-a610-70ce7fcfcf6f"), 1000));

            // Add transaction
            //Console.WriteLine("TX " + _users.UserAccounts.sendToUser(Guid.Parse("46adc225-80ec-478d-a610-70ce7fcfcf6f"), Guid.Parse("2ad314f2-d0c8-405a-a2b9-dbcaa138b765"), 150));

            // Withdraw
            //Console.WriteLine("Withdraw " + _users.UserAccounts.withdraw(Guid.Parse("46adc225-80ec-478d-a610-70ce7fcfcf6f"), 10));
        }


    }
}
