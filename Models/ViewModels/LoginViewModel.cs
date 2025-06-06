using System.ComponentModel.DataAnnotations;

namespace practice1.Models.ViewModels
{
    /// <summary>
    /// View model for the login form
    /// Contains properties for email, password, and remember me option
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// User's email address used as the username for authentication
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's password for authentication
        /// DataType attribute ensures proper handling in the UI
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Option to persist the authentication cookie
        /// When true, the user will remain logged in after closing the browser
        /// </summary>
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}