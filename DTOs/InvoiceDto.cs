using System.ComponentModel.DataAnnotations;

namespace IInvoiceManagementSystem.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}