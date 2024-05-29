using System.ComponentModel.DataAnnotations;

public class Slider
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Image is required.")]
    public string ImageUrl { get; set; }
}
