using Microsoft.EntityFrameworkCore;
using IInvoiceManagementSystem.Data;
using IInvoiceManagementSystem.Models;

namespace IInvoiceManagementSystem.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Invoice> Invoices, int TotalCount)> GetAllAsync(string? customerName, string? status, int page, int pageSize)
        {
            var query = _context.Invoices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                query = query.Where(i => i.CustomerName.Contains(customerName));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(i => i.Status == status);
            }

            int totalCount = await query.CountAsync();

            var invoices = await query
                .OrderByDescending(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (invoices, totalCount);
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _context.Invoices.FindAsync(id);
        }

        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            var lastInvoice = await _context.Invoices.OrderByDescending(i => i.Id).FirstOrDefaultAsync();
            int newId = (lastInvoice?.Id ?? 0) + 1;
            invoice.InvoiceNumber = $"INV-{DateTime.Now.Year}-{newId:D4}";

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var invoice = await GetByIdAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Invoices.AnyAsync(i => i.Id == id);
        }
    }
}