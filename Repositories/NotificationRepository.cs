using Microsoft.EntityFrameworkCore;
using IInvoiceManagementSystem.Data;
using IInvoiceManagementSystem.Models;

namespace IInvoiceManagementSystem.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllUnreadAsync()
        {
            return await _context.Notifications
                .Where(n => !n.IsRead)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _context.Notifications.CountAsync(n => !n.IsRead);
        }
    }
}