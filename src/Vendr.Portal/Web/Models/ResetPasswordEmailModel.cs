using System;

namespace Vendr.Portal.Web.Models
{
    public class ResetPasswordEmailModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string ResetPasswordUrl { get; set; }
    }
}