using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EBanking.Data.Entities;
using EBanking.Data;
using EBanking.Data.Interfaces;

namespace EBanking
{
    class Transactions
    {

        IEBankingDbContext _db;

        public List<Transaction> All {
            get
            {
                return _db.Transactions.All.ToList();
            } 
        
        }

        public Transactions(IEBankingDbContext db)
        {
            _db = db;
        }

    }
}
