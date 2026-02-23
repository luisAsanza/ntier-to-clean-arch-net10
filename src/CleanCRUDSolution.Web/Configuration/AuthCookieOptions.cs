using System.ComponentModel.DataAnnotations;

namespace CleanCRUDSolution.Web.Configuration
{
    public class AuthCookieOptions
    {
        [Required]
        public int AbsoluteExpirationHours { get; set; } = 5;
    }
}
