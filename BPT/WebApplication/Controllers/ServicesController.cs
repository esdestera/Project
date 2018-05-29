using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult MyServices()
        {
            MyServicesViewModel model = new MyServicesViewModel();
            model.Transactions = new List<TransactionInfoViewModel>();
            var currentUser = db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser != null)
            {
                var accounts = db.Accounts.Where(m => m.UserId == currentUser.Id).ToList();
                var trans = new List<Transaction>();
                var transactions = new List<Transaction>();
                foreach (var account in accounts)
                {
                    trans = db.Transactions.Where(m => m.SenderId == account.AccountId).ToList();
                    if (trans != null)
                    {
                        foreach (var transaction in trans)
                        {
                            if (transaction.SaveToMyTransfers == true)
                            {
                                var transactionInfo = new TransactionInfoViewModel();
                                transactionInfo.TransactionName = transaction.TransactionName;
                                transactionInfo.SenderIban = account.Iban;
                                transactionInfo.Amount = transaction.Amount;
                                transactionInfo.Created = transaction.Created;
                                //var receiver = db.Accounts.Where(m => m.AccountId == transaction.ReceiverId).FirstOrDefault();
                                //transactionInfo.ReceiverIban = receiver.Iban;
                                //transactionInfo.ReceiverCurrency = receiver.Currency;
                                transactionInfo.SenderCurrency = account.Currency;
                                transactionInfo.TransactionId = transaction.TransactionId;
                                model.Transactions.Add(transactionInfo);
                            }
                        }
                    }

                }

                return View(model);
            }

            return RedirectToAction("InvalidUser", "Account");

        }
    }
}
