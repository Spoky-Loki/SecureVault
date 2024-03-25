using System.ComponentModel.DataAnnotations;

namespace SecureVault.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        public User user { get; set; }

        public string Name { get; set; }

        public bool CcType { get; set; }

        public string CcName { get; set; }

        [MaxLength(32)]
        public byte[] CcNumber { get; set; }

        public string CcExpiration { get; set; }

        [MaxLength(32)]
        public byte[] CVV { get; set; }
    }
}
