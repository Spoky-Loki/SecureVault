using SecureVault.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SecureVault.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [AllowNull]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Не указан E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [AllowNull]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(12)]
        public string? PhoneNumber { get; set; }

        [AllowNull]
        [DataType(DataType.Text)]
        public string? Address { get; set; }

        [AllowNull]
        [DataType(DataType.Text)]
        public string? Country { get; set; }

        [AllowNull]
        [DataType(DataType.Text)]
        public string? Region { get; set; }

        [AllowNull]
        [DataType(DataType.Text)]
        public string? City { get; set; }

        [AllowNull]
        [DataType(DataType.PostalCode)]
        [MaxLength(6)]
        public string? Zip { get; set; }

        public ProfileViewModel()
        {
            Name = "";
            Surname = "";
            Patronymic = "";
            Email = "";
            PhoneNumber = "";
            Address = "";
            Country = "";
            Region = "";
            City = "";
            Zip = "";
        }

        public ProfileViewModel(User user)
        {
            Name = user.Name;
            Surname = user.Surname;
            Patronymic = user.Patronymic;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Address = user.Address;
            Country = user.Country;
            Region = user.Region;
            City = user.City;
            Zip = user.Zip;
        }
    }
}
