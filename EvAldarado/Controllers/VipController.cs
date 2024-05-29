using EvAldarado.DAL;
using EvAldarado.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

public class VipController : Controller
{
    private readonly AppDBContext _context;
    private readonly UserManager<Users> _userManager;

    public VipController(AppDBContext context, UserManager<Users> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Checkout(int id)
    {
        var domain = "https://localhost:7294/";
        var options = new SessionCreateOptions
        {
            SuccessUrl = domain + $"Vip/OrderConfirmation?id={id}",
            CancelUrl = domain + "Vip/Login",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 1000, // Example amount in cents (10.00 USD)
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "VIP Advertisement"
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment"
        };

        var service = new SessionService();
        Session session = service.Create(options);
        TempData["Session"] = session.Id;
        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }

    public async Task<IActionResult> OrderConfirmation(int id)
    {
        var service = new SessionService();
        Session session = service.Get(TempData["Session"].ToString());

        if (session.PaymentStatus == "paid")
        {
            var product = await _context.UserProducts.FindAsync(id);
            if (product != null)
            {
                product.IsVip = true;
                _context.UserProducts.Update(product);
                await _context.SaveChangesAsync();
            }

            return View("Success");
        }

        return View("Fail");
    }
}





































//using EvAldarado.DAL;
//using EvAldarado.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Stripe.Checkout;

//namespace EvAldarado.Controllers
//{
//    public class VipController : Controller
//    {
//            private readonly AppDBContext _context;
//            private readonly UserManager<Users> _userManager;
//            public VipController(AppDBContext context, UserManager<Users> userManager)
//            {
//                _context = context;
//                _userManager = userManager;
//            }

//            public IActionResult Checkout(int id)
//            {
//                var domain = "https://localhost:7294/";
//                var options = new SessionCreateOptions()
//                {
//                    SuccessUrl = domain + $"Vip/OrderConfirmation",
//                    CancelUrl = domain + "Vip/Login",
//                    LineItems = new List<SessionLineItemOptions>(),
//                    Mode = "payment"
//                };
//                var service = new SessionService();
//                Session session = service.Create(options);
//                TempData["Session"] = session.Id;
//                Response.Headers.Add("Location", session.Url);
//                return new StatusCodeResult(303);
//            }

//            public async Task<IActionResult> OrderConfirmation()
//            {
//                var service = new SessionService();
//                Session session = service.Get(TempData["Session"].ToString());
//                if (session.PaymentStatus == "paid")
//                {
//                    var user = await _userManager.GetUserAsync(User);
//                    //var random = new Random();
//                    //tempOrder.UserId = user.Id;
//                    //tempOrder.OrderNumber = random.Next(100000, 1000000);
//                    //tempOrder.Date = DateTime.Now;
//                    //tempOrder.Total = total;

//                    //_context.Orders.Add(tempOrder);
//                    //_context.SaveChanges();
//                    //var cartList = HttpContext.Session.GetJson<List<CartItem>>("Cart");
//                    //foreach (var item in cartList)
//                    //{
//                    //    var orderDetail = new OrderItem
//                    //    {
//                    //        OrderId = tempOrder.Id,
//                    //        ProductId = item.ProductId,
//                    //        Quantity = item.Quantity,
//                    //        Subtotal = item.Total,
//                    //        Color = item.Color,
//                    //        Size = item.Size
//                    //    };
//                    //    _context.OrderItems.Add(orderDetail);
//                    //    var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
//                    //    product.StockQuantity -= item.Quantity;
//                    //}
//                    //_context.SaveChanges();
//                    //var _order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == tempOrder.Id);
//                    return View("Success");
//                }
//                return View("Fail");
//            }



//        }
//    }
