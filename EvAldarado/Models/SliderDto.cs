using System.ComponentModel.DataAnnotations;

namespace EvAldarado.Models
{
    public class SliderDto
    {
        [Required(ErrorMessage = "The name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}