using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Gym_Store.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
    }
}
