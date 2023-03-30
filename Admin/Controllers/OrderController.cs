using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebShopMVC.Data;
using WebShopMVC.Models;

namespace WebShopMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        //Database bağlanmak için
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public OrderDetailsVM OrderVM { get; set; }
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost]
        [Authorize(Roles = Other.Role_Admin)]
        public IActionResult Approved()
        {
            OrderHeader orderHeader = _db.OrderHeaders.FirstOrDefault(i => i.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = Other.Durum_Onaylandi;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = Other.Role_Admin)]
        public IActionResult ShipIt()
        {
            OrderHeader orderHeader = _db.OrderHeaders.FirstOrDefault(i => i.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = Other.Durum_Kargoda;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            OrderVM = new OrderDetailsVM
            {
                OrderHeader = _db.OrderHeaders.FirstOrDefault(i => i.Id == id),
                OrderDetails = _db.OrderDetails.Where(x => x.OrderId == id).Include(x => x.Product)
            };
            return View(OrderVM);
        }

        public IActionResult Index()
        {
            //Giriş yapmış kullanıcıyı getirmek için
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //Bütün siparişlerin listesi
            IEnumerable<OrderHeader> orderHeadersList;
            if(User.IsInRole(Other.Role_Admin)) 
            {
                orderHeadersList = _db.OrderHeaders.ToList();
            }
            else
            {
                orderHeadersList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value).Include(i => i.ApplicationUser);
            }
            return View(orderHeadersList);
        }
        public IActionResult Pending()
        {
            //Giriş yapmış kullanıcıyı getirmek için
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //Bütün siparişlerin listesi
            IEnumerable<OrderHeader> orderHeadersList;
            if (User.IsInRole(Other.Role_Admin))
            {
                orderHeadersList = _db.OrderHeaders.Where(i => i.OrderStatus == Other.Durum_Beklemede);
            }
            else
            {
                orderHeadersList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value && i.OrderStatus==Other.Durum_Beklemede).Include(i => i.ApplicationUser);
            }
            return View(orderHeadersList);
        }
        public IActionResult Completed()
        {
            //Giriş yapmış kullanıcıyı getirmek için
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //Bütün siparişlerin listesi
            IEnumerable<OrderHeader> orderHeadersList;
            if (User.IsInRole(Other.Role_Admin))
            {
                orderHeadersList = _db.OrderHeaders.Where(i => i.OrderStatus == Other.Durum_Onaylandi);
            }
            else
            {
                orderHeadersList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value && i.OrderStatus == Other.Durum_Onaylandi).Include(i => i.ApplicationUser);
            }
            return View(orderHeadersList);
        }
        public IActionResult Shipped()
        {
            //Giriş yapmış kullanıcıyı getirmek için
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //Bütün siparişlerin listesi
            IEnumerable<OrderHeader> orderHeadersList;
            if (User.IsInRole(Other.Role_Admin))
            {
                orderHeadersList = _db.OrderHeaders.Where(i => i.OrderStatus == Other.Durum_Kargoda);
            }
            else
            {
                orderHeadersList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value && i.OrderStatus == Other.Durum_Kargoda).Include(i => i.ApplicationUser);
            }
            return View(orderHeadersList);
        }
    }
}
