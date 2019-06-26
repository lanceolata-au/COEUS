using System.ComponentModel.DataAnnotations;

namespace carbon.api.Models.Account
{
    public class PasswordInputModel : ProfileViewModel
    {
        [Required] public string CurrentPassword { get; set; }
        [Required] public string NewPassword { get; set; }
        [Required] public string NewPasswordVerify { get; set; }
    }
}