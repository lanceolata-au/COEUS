using carbon.core.dtos.account;

namespace carbon.api.Models.Account
{
    public class ProfileViewModel
    {
       public string UserName { get; set; }
       
       public CoreUserDto CoreUserDto { get; set; }
       
    }
}