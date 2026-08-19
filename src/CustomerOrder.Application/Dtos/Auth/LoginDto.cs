using System.ComponentModel.DataAnnotations;

namespace CustomerOrder.Application.Dtos.Auth
{
    public class LoginDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 64 characters.")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}
