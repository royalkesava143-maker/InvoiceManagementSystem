using IInvoiceManagementSystem.DTOs;

namespace IInvoiceManagementSystem.Services
{
    public interface IInvoiceService
    {
        Task<(IEnumerable<InvoiceDto> Invoices, int TotalCount)> GetAllAsync(string? customerName, string? status, int page, int pageSize);
        Task<InvoiceDto?> GetByIdAsync(int id);
        Task<InvoiceDto> CreateAsync(CreateInvoiceDto createDto);
        Task<InvoiceDto?> UpdateAsync(UpdateInvoiceDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}