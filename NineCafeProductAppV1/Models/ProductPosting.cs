

using System.ComponentModel.DataAnnotations;

namespace NineCafeProductAppV1.Models
{
    public class ProductPosting 
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }  = string.Empty;
        [Range(0.01, 2.5)]
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } 
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string FoodPandaUrl { get; set; } = "https://www.foodpanda.com.kh/en/restaurant/q0xf/nine-cafe-tuol-sangke";

    }
}
