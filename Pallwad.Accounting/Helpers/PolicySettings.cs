using PallyWad.Services;
using PallyWad.Services.Generics;

namespace Pallwad.Accounting.Helpers
{
    public static class PolicySettings<T> where T : class
    {
        /*public static IUnitOfWork unitofWork;
        private static MailSender mailSender;

        public static void SendPostingNotification(string email, string status, string name, string message, string phnoeNo, ISmtpConfigService smtpConfigService)
        {
            NotificationProperties notifier = new NotificationProperties();

            //string notUrl = new ConfigurationHelper().config["underwritersGroup"] + "Underwritting/Policy"; // ConfigurationManager.AppSettings["webUrl"] + "Underwritting/Policy"; 
            string subject = "Finance Posting Operations - " + status;
            string receivername = "Posting Officer - " + name;
            notifier.sender = new ConfigurationHelper().config["appSender"];

            string filepath = ""; //new ConfigurationHelper().config["rentionlimitpath"];
            mailSender = new MailSender(smtpConfigService);
            var mailmessage = mailSender.doMail(email, "", "", subject, receivername, message, filepath);
        }*/
    }
}
