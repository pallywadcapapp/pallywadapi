using Dapper;
using Microsoft.AspNetCore.Http;
using PallyWad.Domain;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using PallyWad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    [TransientRegistration]
    public class NotificationsService: BaseService, INotificationsService
    {
        private readonly INotificationRepository _NotificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationsService(INotificationRepository NotificationRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _NotificationRepository = NotificationRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddNotification(Notification Notification)
        {
            _NotificationRepository.Add(Notification);
            Save();
        }

        public List<Notification> ListAllNotifications()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);
            var result = _NotificationRepository.FindAll().ToList();
            return result;
        }

        public List<Notification> ListAllNotification(string memberId)
        {
            var parameters = new DynamicParameters();
            var result = _NotificationRepository.FindAll().Where(x => x.memberId == memberId).ToList();
            return result;
        }

        public Notification GetNotification(int id)
        {
            return _NotificationRepository.Get(x => x.Id == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateNotification(Notification Notification)
        {
            _NotificationRepository.Update(Notification);
            Save();
        }
    }

    public interface INotificationsService
    {
        void AddNotification(Notification Notification);
        List<Notification> ListAllNotifications();
        List<Notification> ListAllNotification(string memberId);
        Notification GetNotification(int id);
        void Save();
        void UpdateNotification(Notification Notification);
    }
}
