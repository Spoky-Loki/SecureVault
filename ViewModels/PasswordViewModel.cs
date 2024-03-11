using System.ComponentModel.DataAnnotations;

namespace SecureVault.ViewModels
{
    public class PasswordViewModel
    {
        public int id;

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [MaxLength(32)]
        public string password { get; set; }
    }
}
