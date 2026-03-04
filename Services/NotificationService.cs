using IInvoiceManagementSystem.DTOs;
using IInvoiceManagementSystem.Models;
using IInvoiceManagementSystem.Repositories;
using IInvoiceManagementSystem.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IInvoiceManagementSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllUnreadAsync();
            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                CreatedDate = n.CreatedDate,
                IsRead = n.IsRead
            });
        }

        public async Task<NotificationDto> CreateNotificationAsync(string title, string message)
        {
            Console.WriteLine($"🔵 STEP 1: Creating notification: {title} - {message}");

            var notification = new Notification
            {
                Title = title,
                Message = message,
                CreatedDate = DateTime.Now,
                IsRead = false
            };

            Console.WriteLine("🟡 STEP 2: Saving to database...");
            var created = await _notificationRepository.AddAsync(notification);
            Console.WriteLine($"🟢 STEP 3: Saved to database with ID: {created.Id}");

            // Send real-time notification via SignalR
            try
            {
                Console.WriteLine("🟡 STEP 4: Attempting to send SignalR notification...");
                await _hubContext.Clients.All.SendAsync("ReceiveNotification",
                    title,
                    message,
                    created.CreatedDate.ToString("hh:mm:ss tt"));
                Console.WriteLine("✅ STEP 5: SignalR notification sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ STEP 4 ERROR: {ex.Message}");
            }

            return new NotificationDto
            {
                Id = created.Id,
                Title = created.Title,
                Message = created.Message,
                CreatedDate = created.CreatedDate,
                IsRead = created.IsRead
            };
        }

        public async Task MarkAsReadAsync(int id)
        {
            await _notificationRepository.MarkAsReadAsync(id);
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _notificationRepository.GetUnreadCountAsync();
        }
    }
}