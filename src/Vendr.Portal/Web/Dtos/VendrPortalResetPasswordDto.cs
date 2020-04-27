using System.ComponentModel.DataAnnotations;

namespace Vendr.Portal.Web.Dtos
{
    public class VendrPortalResetPasswordBaseDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

    }
    public class VendrPortalResetPasswordDto : VendrPortalResetPasswordBaseDto
    {
        [Required]
        public string Password { get; set; }
    }
}