using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IInvoiceManagementSystem.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(30);

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Tax { get; set; }

        // This is calculated - not stored in database
        [NotMapped]
        public decimal TotalAmount => Amount + (Amount * Tax / 100);

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Draft";
    }
}