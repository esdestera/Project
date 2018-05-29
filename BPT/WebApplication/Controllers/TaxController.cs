using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TaxController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Bill
        [HttpGet]
        public ActionResult TaxIndex()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Continue(TaxViewModel model)
        {
            var tax = db.Taxes.Where(m => m.Name.Equals(model.Name)).FirstOrDefault();
            if (tax != null)
            {
                model.Iban = tax.TaxIban;
                return View("TaxConfirmation", model);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaxViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tax = db.Taxes.Where(m => m.Name.Equals(model.Name)).FirstOrDefault();
                var accountSrc = db.Accounts.Where(m => m.Iban.Equals(model.SenderIban)).FirstOrDefault();
                if (tax != null && accountSrc != null)
                {
                    if (accountSrc.Balance > (double)model.Amount)
                    {
                        if (accountSrc.CardLimit < (double)(accountSrc.Balance - (double)model.Amount))
                        {
                            var transaction = new Transaction((double)model.Amount, "out", accountSrc.AccountId, 0, model.SaveToMyServices, model.Details, model.TransactionName);
                            accountSrc.Transactions.Add(transaction);
                            accountSrc.Balance -= (double)model.Amount;
                            if (!string.IsNullOrEmpty(model.TransactionName))
                            {
                                model.SaveToMyServices = true;
                                transaction.SaveToMyTransfers = true;
                            }
                            transaction.Created = DateTime.Now;
                            db.Transactions.Add(transaction);
                            db.SaveChanges();
                            return RedirectToAction("Home", "Home");
                        }
                        else
                        {
                            return View("Error", model);
                        }
                    }
                    else
                    {
                        return View("Error", model);
                    }
                }
                else
                {
                    return View("Error");
                }

            }

            return View("Error");
        }

        private double ConvertToAccountCurrency(string senderCurrency, string receiverCurrency, double amount)
        {
            var result = 0.0;
            if (senderCurrency.Equals(receiverCurrency))
            {
                return (double)amount;
            }

            switch (senderCurrency)
            {
                case "EUR":
                    switch (receiverCurrency)
                    {
                        case "USD":
                            result = Double.Parse(ConfigurationManager.AppSettings["EURTOUSD"]);
                            return amount * result;
                        case "RON":
                            result = Double.Parse(ConfigurationManager.AppSettings["EURTOLEI"]);
                            return amount * result;
                    }
                    break;
                case "USD":
                    switch (receiverCurrency)
                    {
                        case "EUR":
                            result = Double.Parse(ConfigurationManager.AppSettings["USDTOEUR"]);
                            return amount * result;
                        case "RON":
                            result = Double.Parse(ConfigurationManager.AppSettings["USDTOLEI"]);
                            return amount * result;
                    }
                    break;
                case "RON":
                    switch (receiverCurrency)
                    {
                        case "EUR":
                            result = Double.Parse(ConfigurationManager.AppSettings["LEITOEUR"]);
                            return amount * result;
                        case "USD":
                            result = Double.Parse(ConfigurationManager.AppSettings["LEITOUSD"]);
                            return amount * result;
                    }
                    break;

            }

            return result;
        }


    }


}
