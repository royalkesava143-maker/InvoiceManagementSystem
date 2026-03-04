using IInvoiceManagementSystem.DTOs;
using IInvoiceManagementSystem.Models;
using IInvoiceManagementSystem.Repositories;

namespace IInvoiceManagementSystem.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly INotificationService _notificationService;

        public InvoiceService(IInvoiceRepository invoiceRepository, INotificationService notificationService)
        {
            _invoiceRepository = invoiceRepository;
            _notificationService = notificationService;
        }

        public async Task<(IEnumerable<InvoiceDto> Invoices, int TotalCount)> GetAllAsync(string? customerName, string? status, int page, int pageSize)
        {
            var (invoices, totalCount) = await _invoiceRepository.GetAllAsync(customerName, status, page, pageSize);

            var invoiceDtos = invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                CustomerName = i.CustomerName,
                CustomerEmail = i.CustomerEmail,
                InvoiceDate = i.InvoiceDate,
                DueDate = i.DueDate,
                Amount = i.Amount,
                Tax = i.Tax,
                TotalAmount = i.Amount + (i.Amount * i.Tax / 100),
                Status = i.Status
            });

            return (invoiceDtos, totalCount);
        }

        public async Task<InvoiceDto?> GetByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null) return null;

            return new InvoiceDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                CustomerName = invoice.CustomerName,
                CustomerEmail = invoice.CustomerEmail,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                Amount = invoice.Amount,
                Tax = invoice.Tax,
                TotalAmount = invoice.Amount + (invoice.Amount * invoice.Tax / 100),
                Status = invoice.Status
            };
        }

        public async Task<InvoiceDto> CreateAsync(CreateInvoiceDto createDto)
        {
            System.Diagnostics.Debug.WriteLine("🔵🔵🔵 CREATE ASYNC STARTED");

            var invoice = new Invoice
            {
                CustomerName = createDto.CustomerName,
                CustomerEmail = createDto.CustomerEmail,
                InvoiceDate = createDto.InvoiceDate,
                DueDate = createDto.DueDate,
                Amount = createDto.Amount,
                Tax = createDto.Tax,
                Status = createDto.Status
            };

            var createdInvoice = await _invoiceRepository.AddAsync(invoice);

            // Create notification for invoice creation
            System.Diagnostics.Debug.WriteLine("🟡🟡🟡 ABOUT TO CREATE NOTIFICATION");
            await _notificationService.CreateNotificationAsync(
                "Invoice Created",
                $"Invoice {createdInvoice.InvoiceNumber} for {createdInvoice.CustomerName} has been created."
            );
            System.Diagnostics.Debug.WriteLine("🟢🟢🟢 NOTIFICATION CREATED");


            return new InvoiceDto
            {
                Id = createdInvoice.Id,
                InvoiceNumber = createdInvoice.InvoiceNumber,
                CustomerName = createdInvoice.CustomerName,
                CustomerEmail = createdInvoice.CustomerEmail,
                InvoiceDate = createdInvoice.InvoiceDate,
                DueDate = createdInvoice.DueDate,
                Amount = createdInvoice.Amount,
                Tax = createdInvoice.Tax,
                TotalAmount = createdInvoice.Amount + (createdInvoice.Amount * createdInvoice.Tax / 100),
                Status = createdInvoice.Status
            };
        }

        public async Task<InvoiceDto?> UpdateAsync(UpdateInvoiceDto updateDto)
        {
            var existingInvoice = await _invoiceRepository.GetByIdAsync(updateDto.Id);
            if (existingInvoice == null) return null;

            var oldStatus = existingInvoice.Status;

            existingInvoice.CustomerName = updateDto.CustomerName;
            existingInvoice.CustomerEmail = updateDto.CustomerEmail;
            existingInvoice.InvoiceDate = updateDto.InvoiceDate;
            existingInvoice.DueDate = updateDto.DueDate;
            existingInvoice.Amount = updateDto.Amount;
            existingInvoice.Tax = updateDto.Tax;
            existingInvoice.Status = updateDto.Status;

            await _invoiceRepository.UpdateAsync(existingInvoice);

            // Create notification if status changed to Sent or Paid
            if (oldStatus != updateDto.Status)
            {
                if (updateDto.Status == "Sent")
                {
                    await _notificationService.CreateNotificationAsync(
                        "Invoice Sent",
                        $"Invoice {existingInvoice.InvoiceNumber} has been sent to {existingInvoice.CustomerName}."
                    );
                }
                else if (updateDto.Status == "Paid")
                {
                    await _notificationService.CreateNotificationAsync(
                        "Invoice Paid",
                        $"Invoice {existingInvoice.InvoiceNumber} from {existingInvoice.CustomerName} has been paid."
                    );
                }
            }

            return new InvoiceDto
            {
                Id = existingInvoice.Id,
                InvoiceNumber = existingInvoice.InvoiceNumber,
                CustomerName = existingInvoice.CustomerName,
                CustomerEmail = existingInvoice.CustomerEmail,
                InvoiceDate = existingInvoice.InvoiceDate,
                DueDate = existingInvoice.DueDate,
                Amount = existingInvoice.Amount,
                Tax = existingInvoice.Tax,
                TotalAmount = existingInvoice.Amount + (existingInvoice.Amount * existingInvoice.Tax / 100),
                Status = existingInvoice.Status
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _invoiceRepository.ExistsAsync(id))
                return false;

            await _invoiceRepository.DeleteAsync(id);
            return true;
        }
    }
}