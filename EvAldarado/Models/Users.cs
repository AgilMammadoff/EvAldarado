using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvAldarado.Models
{
    public class Users : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [NotMapped]
        public List<UserProducts> UserProducts { get; set; }    
    }
}
