using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SecureVault.Models;
using SecureVault.Services;
using SecureVault.ViewModels;
using System.Text;
using Key = SecureVault.Services.Key;

namespace SecureVault.Controllers
{
    public class CreditCardController : Controller
    {
        private readonly ApplicationContext _context;

        public CreditCardController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == User.Claims.First().Value.ToString());

                List<CreditCardViewModel> list = new List<CreditCardViewModel>();
                var creditCards = _context.CreditCards.Where(x => x.user == user).ToList();
                foreach (var creditCard in creditCards)
                    list.Add(new CreditCardViewModel(creditCard));

                return View(list);
            }
            else
                return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreditCardViewModel());
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var card = _context.CreditCards.FirstOrDefault(i => i.Id == id);
            if (card != null)
                _context.CreditCards.Remove(card);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == User.Claims.First().Value.ToString());

                if (user != null)
                {
                    var creditCards = _context.CreditCards.Where(x => x.user == user).ToList();
                    CreditCard? card = creditCards.FirstOrDefault(c => c.Id == id);
                    if (card != null)
                    {
                        CreditCardViewModel cardViewModel = new CreditCardViewModel(card, true);

                        return View(cardViewModel);
                    }
                    else
                        return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Login", "Account");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Route("CreditCard/Password/{id}")]
        public ActionResult Password(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Password(PasswordViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == User.Claims.First().Value.ToString());

                if (user != null && AesEncryption.decrypt(user.Password, Key.key).SequenceEqual(AesEncryption.getBytePassword(model.password)))
                    return RedirectToAction("Detail", new { id = id });
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreditCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == User.Claims.First().Value.ToString());
                if (user != null)
                {
                    CreditCard creditCard = new CreditCard
                    {
                        user = user,
                        Name = model.Name,
                        CcExpiration = model.CcExpiration,
                        CcName = model.CcName,
                        CcNumber = AesEncryption.encrypt(Encoding.UTF8.GetBytes(model.CcNumber.Replace(" ", "")), Key.key),
                        CVV = AesEncryption.encrypt(Encoding.UTF8.GetBytes(model.CVV.ToString()), Key.key),
                        CcType = model.CardType == model.CardTypes[0] ? true : false
                    };

                    _context.CreditCards.Add(creditCard);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
