using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerMessaging.Messaging;

namespace TaskManagerDomain.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUserAsync(NotificationMessageDto notification);
    }
}
