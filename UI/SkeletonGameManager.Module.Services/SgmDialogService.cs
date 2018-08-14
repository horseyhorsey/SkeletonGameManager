using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.Services
{
    public class SgmDialogService : ISgmDialogService
    {
        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        public void ShowDialog(string message)
        {
            NotificationRequest.Raise(new Notification
            {
                Content = "Notification Message",
                Title = "Notification"               
            });
        }
    }
}
