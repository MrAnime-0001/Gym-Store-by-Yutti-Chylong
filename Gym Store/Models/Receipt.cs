using System;
using System.ComponentModel.DataAnnotations;

namespace Gym_Store.Models
{
    public class Receipt
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Link to Identity User

        [Required]
        public DateTime PurchaseDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string ItemsSummary { get; set; } // Or use a related list of ReceiptItems if needed
    }
}
