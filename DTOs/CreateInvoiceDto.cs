using System.ComponentModel.DataAnnotations;

namespace IInvoiceManagementSystem.DTOs
{
    public class CreateInvoiceDto
    {
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Tax { get; set; }

        [Required]
        [RegularExpression("^(Draft|Sent|Paid)$", ErrorMessage = "Status must be 'Draft', 'Sent', or 'Paid'.")]
        public string Status { get; set; } = "Draft";
    }
}