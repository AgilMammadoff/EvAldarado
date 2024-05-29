using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EvAldarado.Models
{
    public class UserProducts
    {
        [Required(ErrorMessage = "Advertisement name is required.")]
        public string AdvertisementName { get; set; }

        [Required(ErrorMessage = "Agent name is required.")]
        public string AgentName { get; set; }

        public string Imagine { get; set; }

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

        [Required(ErrorMessage = "Date advertisement is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Advertisement")]
        public DateTime DateAdvertisement { get; set; }

        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Cost is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Cost cannot be negative.")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "Agent email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string AgentEmail { get; set; }

        [Required(ErrorMessage = "Agent telephone is required.")]
        [Phone(ErrorMessage = "Invalid telephone number.")]
        public string AgentTelephone { get; set; }

        public bool IsVip { get; set; }

        public List<Images> Images { get; set; } = new List<Images>();

        [NotMapped]
        public IFormFile BaseImageFile2 { get; set; }

        [NotMapped]
        public List<IFormFile> imagesFiles { get; set; }

        // Foreign key for the associated user
        public string UserId { get; set; }

        // Navigation property for the associated user
        public Users User { get; set; }
    }
}
