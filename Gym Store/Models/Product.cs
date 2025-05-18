using System.ComponentModel.DataAnnotations;

namespace Gym_Store.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        [Display(Name = "Stock Level")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock level must be a positive number.")]
        public int Quantity { get; set; }

        [Display(Name = "Serving Size")]
        public string? ServingSize { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
