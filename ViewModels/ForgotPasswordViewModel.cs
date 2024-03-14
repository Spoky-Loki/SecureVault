using System.ComponentModel.DataAnnotations;

namespace SecureVault.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
