using System.ComponentModel.DataAnnotations;

namespace Vendr.Portal.Web.Dtos
{
    public class VendrRegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}