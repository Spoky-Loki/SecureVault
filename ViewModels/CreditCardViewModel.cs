using SecureVault.Models;
using SecureVault.Services;
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
                var ccNumberTemp = Encoding.UTF8.GetString(AesEncryption.decrypt(creditCard.CcNumber, Key.key));

                Id = creditCard.Id;
                Name = creditCard.Name;
                CcExpiration = creditCard.CcExpiration;
                CcName = creditCard.CcName;
                CcNumber = ccNumberTemp.Substring(0, 4) + " " + ccNumberTemp.Substring(4, 4) + " " +
                    ccNumberTemp.Substring(8, 4) + " " + ccNumberTemp.Substring(12, 4);
                CVV = Encoding.UTF8.GetString(AesEncryption.decrypt(creditCard.CVV, Key.key)).Substring(0, 3);
                CardType = creditCard.CcType == true ? CardTypes[0] : CardTypes[1];
            }
            else
            {
                Id = creditCard.Id;
                Name = creditCard.Name;
                CcExpiration = creditCard.CcExpiration;
                CcName = creditCard.CcName;
                CcNumber = "**** **** **** " + Encoding.UTF8.GetString(AesEncryption.decrypt(creditCard.CcNumber, Key.key)).Substring(12);
                CVV = "**" + Encoding.UTF8.GetString(AesEncryption.decrypt(creditCard.CVV, Key.key)).Substring(2).First();
                CardType = creditCard.CcType == true ? CardTypes[0] : CardTypes[1];
            }
        }
    }
}
