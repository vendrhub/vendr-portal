using System.ComponentModel.DataAnnotations;

namespace Vendr.Portal.Web.Dtos
{
    public class VendrPortalLoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}