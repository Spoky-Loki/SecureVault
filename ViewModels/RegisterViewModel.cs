using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SecureVault.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [DataType(DataType.Text)]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        [DataType(DataType.Text)]
        public string lastName { get; set; }

        [AllowNull]
        public string? patronymic { get; set; }

        [Required(ErrorMessage = "Не указан E-mail")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(32, ErrorMessage = "Максимальная длинна пароля 32 символа")]
        [MinLength(6, ErrorMessage = "Минимальная длинна пароля 6 символов")]
        [Required(ErrorMessage = "Не указан пароль")]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Не указан пароль")]
        [Compare("password", ErrorMessage = "Пароли не совпадают")]
        public string confirmPassword { get; set; }
    }
}
