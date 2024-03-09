using Microsoft.AspNetCore.Mvc;
using SecureVault.Models;
using SecureVault.ViewModels;
using System.Text;

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
                        CcNumber = Encoding.UTF8.GetBytes(model.CcNumber),
                        CVV = Encoding.UTF8.GetBytes(model.CVV),
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
