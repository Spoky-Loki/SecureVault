using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SecureVault.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        [AllowNull]
        public string? Patronymic { get; set; }

        public string Email { get; set; }

        [MaxLength(32)]
        public byte[] Password { get; set; }

        [AllowNull]
        public string? PhoneNumber { get; set; }

        [AllowNull]
        public string? Address { get; set; }

        [AllowNull]
        public string? Country { get; set; }

        [AllowNull]
        public string? Region { get; set; }

        [AllowNull]
        public string? City { get; set; }

        [AllowNull]
        public string? Zip { get; set; }

        [AllowNull]
        public string? EmailConfirmationToken { get; set; } = null;

        public bool EmailConfirmed { get; set; } = false;
    }
}
