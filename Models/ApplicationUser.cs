using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }      
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostCode { get; set; }
        [NotMapped]
        public string Role { get; set; }


    }
}
