using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ProfileViewModel
    {

        [Display(Name = "Email address")] public string EmailAddress { get; set; }

        [Display(Name = "First name")] public string FirstName { get; set; }

        [Display(Name = "Last name")] public string LastName { get; set; }

        [Display(Name = "Password")] public string Password { get; set; }

        [Display(Name = "Username")] public string UserName { get; set; }

        [Display(Name = "PhoneNumber")] public string PhoneNumber { get; set; }
    }

    public class MyAccounts
    {
        [Display(Name = "Bank name")]
        public int BankName { get; set; }

        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

        [Display(Name = "Card limit")]
        public int CardLimit { get; set; }

        [Display(Name = "Currency")]
        public string Currency { get; set; }

    }

    public class NewAccount
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bank nme is rquired.")]
        [Display(Name = "Bank name")]
        public int BankName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "IBAN is rquired.")]
        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

        [Display(Name = "Card limit")]
        public int CardLimit { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency is rquired.")]
        [Display(Name = "Currency")]
        public string Currency { get; set; }

    }
}