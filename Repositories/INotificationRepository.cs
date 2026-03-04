using IInvoiceManagementSystem.Models;

namespace IInvoiceManagementSystem.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllUnreadAsync();
        Task<Notification> AddAsync(Notification notification);
        Task MarkAsReadAsync(int id);
        Task<int> GetUnreadCountAsync();
    }
}