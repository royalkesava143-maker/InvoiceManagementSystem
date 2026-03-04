using Microsoft.AspNetCore.Mvc;
using IInvoiceManagementSystem.Services;
using IInvoiceManagementSystem.DTOs;

namespace IInvoiceManagementSystem.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly INotificationService _notificationService;

        public InvoicesController(IInvoiceService invoiceService, INotificationService notificationService)
        {
            _invoiceService = invoiceService;
            _notificationService = notificationService;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(string searchCustomer, string searchStatus, int page = 1)
        {
            int pageSize = 5; // Number of items per page
            var (invoices, totalCount) = await _invoiceService.GetAllAsync(searchCustomer, searchStatus, page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.SearchCustomer = searchCustomer;
            ViewBag.SearchStatus = searchStatus;
            ViewBag.UnreadCount = await _notificationService.GetUnreadCountAsync();

            return View(invoices);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvoiceDto createDto)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.CreateAsync(createDto);
                return RedirectToAction(nameof(Index));
            }
            return View(createDto);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateInvoiceDto
            {
                Id = invoice.Id,
                CustomerName = invoice.CustomerName,
                CustomerEmail = invoice.CustomerEmail,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                Amount = invoice.Amount,
                Tax = invoice.Tax,
                Status = invoice.Status
            };

            return View(updateDto);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateInvoiceDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _invoiceService.UpdateAsync(updateDto);
                if (result == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updateDto);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Invoices/Notifications
        public async Task<IActionResult> Notifications()
        {
            var notifications = await _notificationService.GetUnreadNotificationsAsync();
            return PartialView("_Notifications", notifications);
        }

        // POST: Invoices/MarkAsRead/5
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Ok();
        }
    }
}