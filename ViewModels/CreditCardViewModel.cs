using SecureVault.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecureVault.ViewModels
{
    public class CreditCardViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string CcName { get; set; }

        [DataType(DataType.CreditCard)]
        [MaxLength(19)]
        public string CcNumber { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(5)]
        public string CcExpiration { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3)]
        public string CVV { get; set; }

        public string CardType { get; set; }

        public string[] CardTypes = ["Кредитная", "Дебитовая"];

        public CreditCardViewModel()
        {
            CardType = CardTypes[0];
            CcName = "";
            CcNumber = "";
            CcExpiration = "";
            Name = "";
            CVV = "";
        }

        public CreditCardViewModel(CreditCard creditCard, bool full = false)
        {
            if (full)
            {
                Id = creditCard.Id;
                Name = creditCard.Name;
                CcExpiration = creditCard.CcExpiration;
                CcName = creditCard.CcName;
                CcNumber = Encoding.UTF8.GetString(creditCard.CcNumber).Substring(0, 16);
                CVV = Encoding.UTF8.GetString(creditCard.CVV).Substring(0, 3);
                CardType = creditCard.CcType == true ? CardTypes[0] : CardTypes[1];
            }
            else
            {
                Id = creditCard.Id;
                Name = creditCard.Name;
                CcExpiration = creditCard.CcExpiration;
                CcName = creditCard.CcName;
                CcNumber = "**** **** **** " + Encoding.UTF8.GetString(creditCard.CcNumber).Substring(12);
                CVV = "**" + Encoding.UTF8.GetString(creditCard.CVV).Substring(2).First();
                CardType = creditCard.CcType == true ? CardTypes[0] : CardTypes[1];
            }
        }
    }
}
