using IInvoiceManagementSystem.Models;

namespace IInvoiceManagementSystem.Repositories
{
    public interface IInvoiceRepository
    {
        Task<(IEnumerable<Invoice> Invoices, int TotalCount)> GetAllAsync(string? customerName, string? status, int page, int pageSize);
        Task<Invoice?> GetByIdAsync(int id);
        Task<Invoice> AddAsync(Invoice invoice);
        Task UpdateAsync(Invoice invoice);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}