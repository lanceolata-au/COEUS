using System.ComponentModel.DataAnnotations;

namespace carbon.api.Models.Account
{
    public class RegisterInputModel
    {
        [Required] public string Password { get; set; }
        
        [Required] public string Email { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}