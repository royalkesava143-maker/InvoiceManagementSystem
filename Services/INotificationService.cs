using IInvoiceManagementSystem.DTOs;

namespace IInvoiceManagementSystem.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUnreadNotificationsAsync();
        Task<NotificationDto> CreateNotificationAsync(string title, string message);
        Task MarkAsReadAsync(int id);
        Task<int> GetUnreadCountAsync();
    }
}