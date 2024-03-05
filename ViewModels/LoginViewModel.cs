using System.ComponentModel.DataAnnotations;

namespace SecureVault.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан E-mail")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [MaxLength(32)]
        public string password { get; set; }
    }
}
