using System;

namespace Vendr.Portal.Web.Models
{
    public class RegistrationEmailModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string ConfirmRegistrationUrl { get; set; }
    }
}