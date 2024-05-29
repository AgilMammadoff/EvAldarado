using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvAldarado.Models
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Advertisement name is required.")]
        public string AdvertisementName { get; set; }

        [Required(ErrorMessage = "Agent name is required.")]
        public string AgentName { get; set; }

        [Required(ErrorMessage = "Base image is required.")]
        public IFormFile BaseImageFile { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Square root must be a non-negative value.")]
        public double SquareRoot { get; set; }

        public string DocumentType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Floor must be a positive value.")]
        public int Floor { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Room count must be a positive value.")]
        public int Room { get; set; }

        public bool Repaired { get; set; }

        [Required(ErrorMessage = "Date built is required.")]
        [DataType(DataType.Date)]
        public DateTime DateBuilt { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cost is required.")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "Agent email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string AgentEmail { get; set; }

        [Required(ErrorMessage = "Agent telephone is required.")]
        [Phone(ErrorMessage = "Invalid telephone number.")]
        public string AgentTelephone { get; set; }


    

        public List<IFormFile> imagesFiles { get; set; } 




        //public List<IFormFile> AdditionalImages { get; set; } = new List<IFormFile>();
    }
}
