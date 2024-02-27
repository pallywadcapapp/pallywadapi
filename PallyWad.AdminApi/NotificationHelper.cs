using PallyWad.Domain;
using PallyWad.Services;
using System.Security.Cryptography.X509Certificates;

namespace PallyWad.AdminApi
{
    public static class NotificationHelper
    {
        private static INotificationsService _notificationsService;
        public static INotificationsService NotificationsService
        {
            get
            {
                return _notificationsService;
            }
        }
        public static void  Notificatio(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }
        public static void SendUserNotification(string memberId, string message, string subject)
        {
            var notif = new Notification()
            {
                memberId = memberId,
                readStatus = false,
                message = message,
                senderId = "Admin",
                subject = subject,
                created_date = DateTime.Now,
            };
            _notificationsService.AddNotification(notif);
        }
    }
}
