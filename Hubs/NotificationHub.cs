using Microsoft.AspNetCore.SignalR;

namespace IInvoiceManagementSystem.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string title, string message, string timestamp)
        {
            await Clients.All.SendAsync("ReceiveNotification", title, message, timestamp);
        }
    }
}