using System.ComponentModel.DataAnnotations;

namespace Vendr.Portal.Web.Dtos
{
    public class VendrConfirmRegistrationDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}