using System.ComponentModel.DataAnnotations;

namespace NineCafeProductAppV1.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(0.01, 2.5)]
        [DataType(DataType.Text)] // render as <input type="text"/>
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode =true)]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; } = string.Empty;

        // optional image url
        public string? ImageUrl { get; set; }

        // optional image upload
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; }
        [Required]
        public string FoodPandaUrl { get; set; } = "https://www.foodpanda.com.kh/en/chain/cd3lp/nine-cafe";
    }
}
