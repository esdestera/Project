using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WebApplication.Models;

namespace WebApplication
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            //return Task.FromResult(0);
            return Task.Factory.StartNew(() =>
            {
                SendEmail(message);
            });
        }

        void SendEmail(IdentityMessage message)
        {
            #region formatter
            string text = message.Body;
            string html =  message.Body;
            #endregion

            MailMessage msg = new MailMessage();
            var from = ConfigurationManager.AppSettings["Email"];
            var pass = ConfigurationManager.AppSettings["Password"];
            var to = string.Empty;
            if (message.Destination.Equals("admin@admin.com"))
            {
                to = from;
            }
            else
            {
                to = message.Destination;
            }
            
            msg.From = new MailAddress(from);
            msg.To.Add(new MailAddress(to));
            msg.Subject = message.Subject;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
           // msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(from, pass);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            try
            {

                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
       


public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(RoleStore<IdentityRole> rolesStore)
            :base(rolesStore)
        {

        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            
            var manager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
            if (!manager.RoleExists("Admin"))
            {
                manager.Create(new IdentityRole("Admin"));
            }
            return manager;
        }
    }
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            var billContext = new ApplicationDbContext();

            var bill1 = billContext.Bills.Where(m => m.Name.Equals("CEZ RO")).FirstOrDefault();
            var bill2 = billContext.Bills.Where(m => m.Name.Equals("Telekom - Romania")).FirstOrDefault();
            var bill3 = billContext.Bills.Where(m => m.Name.Equals("TelekomMobile - Romania")).FirstOrDefault();
            var bill4 = billContext.Bills.Where(m => m.Name.Equals(" Compania de Apa Oltenia S.A.")).FirstOrDefault();

            var tax1 = billContext.Taxes.Where(m => m.Name.Equals("CEZ RO")).FirstOrDefault();
            var tax2 = billContext.Taxes.Where(m => m.Name.Equals("Telekom - Romania")).FirstOrDefault();
            var tax3 = billContext.Taxes.Where(m => m.Name.Equals("TelekomMobile - Romania")).FirstOrDefault();


            if (bill1 == null && bill2 == null && bill3 == null && bill4 == null && tax1 == null && tax2 == null && tax3 ==null)
            {
                //var electricityBillAccount = new Account("RO49 AAAA 1B31 0075 9384 0000", "RON");
                //var phoneBillAccount = new Account("RO93 BTKN 4639 6698 8182 8361", "RON");
                //var mobileBillAccount = new Account("RO64 UBOZ 7631 1825 5787 9219", "RON");
                //var waterBillAccount = new Account("RO51 BMZV 4588 1913 5346 3349", "RON");

                var b1 = new Bill("CEZ RO", "RO49 AAAA 1B31 0075 9384 0000");
                var b2 = new Bill("Telekom - Romania", "RO93 BTKN 4639 6698 8182 8361");
                var b3 = new Bill("TelekomMobile - Romania", "RO64 UBOZ 7631 1825 5787 9219");
                var b4 = new Bill(" Compania de Apa Oltenia S.A.", "RO51 BMZV 4588 1913 5346 3349");

                var t1 = new Tax("Sanitation", "RO14 JLFB 9551 9253 3416 3469");
                var t2 = new Tax("Parking", "RO81 QBBE 5290 4709 8563 6122");
                var t3 = new Tax("Rent", "RO11 VYHO 3215 2715 6144 9480");

                //billContext.Accounts.Add(electricityBillAccount);
                //billContext.Accounts.Add(phoneBillAccount);
                //billContext.Accounts.Add(mobileBillAccount);
                //billContext.Accounts.Add(waterBillAccount);

                billContext.Bills.Add(b1);
                billContext.Bills.Add(b2);
                billContext.Bills.Add(b3);
                billContext.Bills.Add(b4);

                billContext.Taxes.Add(t1);
                billContext.Taxes.Add(t2);
                billContext.Taxes.Add(t3);

                billContext.SaveChanges();
            }

            var adminUser = manager.FindByName("admin@admin.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = "admin@admin.com", NameIdentifier= "admin@admin.com", Email = "admin@admin.com", EmailConfirmed = true };       
               manager.Create(adminUser, "@dm!n!str@t0r");
                            
            }
            if (!manager.IsInRole(adminUser.Id, "Admin"))
            {
                manager.AddToRole(adminUser.Id, "Admin");
            }
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        internal Task SendEmailFromAdmin(string id, string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
