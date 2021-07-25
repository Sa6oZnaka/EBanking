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
            //_users.add("Alex2", "12345313131", "Random Name", "test@test.test");

            //Console.WriteLine(_users.authenticate("Alex1", "12345"));
            //Console.WriteLine(_users.authenticate("Alex1", "123451"));

            // Add user account
            //_users.addUserAccount("Alex7", "Savings21");
            //_users.addUserAccount("Alex2", "test");

            // Deposit
            _users.UserAccounts.deposit(Guid.Parse("780c53ba-bb4b-4274-ab42-f6b9ae68414a"), 1000);

            // Add transaction
            _users.UserAccounts.sendToUser(Guid.Parse("780c53ba-bb4b-4274-ab42-f6b9ae68414a"), Guid.Parse("77c24091-5d32-41e5-9273-69f08b28ef7e"), 150);

            // Withdraw
            _users.UserAccounts.withdraw(Guid.Parse("780c53ba-bb4b-4274-ab42-f6b9ae68414a"), 10);
        }


    }
}
