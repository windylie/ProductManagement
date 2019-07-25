using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.InputDto
{
    public class CreateAndEditProductDto
    {
        [Required(ErrorMessage = "Product's description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product's model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Product's brand is required")]
        public string Brand { get; set; }
    }
}
