using System.ComponentModel.DataAnnotations;

namespace CustomerOrder.Application.Dtos.Customers
{
    public class CreateCustomerDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        [StringLength(256, ErrorMessage = "Email must not exceed 256 characters.")]
        [EmailAddress(ErrorMessage = "Email is not a valid address.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone is required.")]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "Phone must be between 7 and 11 characters.")]
        [RegularExpression(@"^\+?[0-9\s\-()]{7,11}$", ErrorMessage = "Phone may contain digits, spaces, dashes, parentheses and an optional leading +.")]
        public string Phone { get; set; }
    }
}
