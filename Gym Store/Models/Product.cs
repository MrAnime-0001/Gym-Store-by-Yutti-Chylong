using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gym_Store.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; } // Foreign Key

        [ForeignKey("CategoryId")]
        [DisplayName("Product Category")]
        public Category Category { get; set; } // Navigation Property

        [Required]
        [Range(0.01, 10000, ErrorMessage = "Please enter a valid price between 0.01 and 10000")]
        [DisplayName("Price (NZD)")]
        public decimal Price { get; set; }

        [Required]
        public string Quantity { get; set; }

        [DisplayName("Image URL")]
        public string? ImageUrl { get; set; }
    }
}
