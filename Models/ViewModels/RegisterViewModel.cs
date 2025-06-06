using System.ComponentModel.DataAnnotations;

namespace practice1.Models.ViewModels
{
    /// <summary>
    /// View model for the registration form
    /// Contains properties for email, full name, password, and password confirmation
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// User's email address used as the username for authentication
        /// Must be unique in the system
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's full name for display and identification purposes
        /// </summary>
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        [StringLength(200, ErrorMessage = "Full name cannot exceed 200 characters")]
        public string Fullname { get; set; } = string.Empty;

        /// <summary>
        /// User's password for authentication
        /// Must be at least 6 characters long
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Confirmation of the user's password
        /// Must match the Password property exactly
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}