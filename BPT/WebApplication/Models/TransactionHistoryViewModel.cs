using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Models
{
    public class TransactionHistoryViewModel
    {

        public ICollection<TransactionInfoViewModel> Transactions { get; set; }
    }
}
