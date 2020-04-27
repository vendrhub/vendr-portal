using System.ComponentModel.DataAnnotations;

namespace Vendr.Portal.Web.Dtos
{
    public class VendrPortalResetPasswordRequestDto
    {
        [Required]
        public string Email { get; set; }
    }
}