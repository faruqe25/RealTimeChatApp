using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRNotificationSystem.Database_Gateway;
using SignalRNotificationSystem.Helper;
using SignalRNotificationSystem.Models;

namespace SignalRNotificationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext context;
        private readonly IHubContext<SignalRServerHub> hub;

        public HomeController(DatabaseContext _context, IHubContext<SignalRServerHub> hub)
        {
            context = _context;
            this.hub = hub;
        }

        public IActionResult Index()
        {
            return View(context.Products.ToList());
        }
        public IActionResult GetProductList() 
        {
            return Json(context.Products.ToList());
        }

        public IActionResult Edit(int id)
        {
          
            return View(context.Products.FirstOrDefault(a => a.ProductId == id));
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            Product p = new Product() {
            ProductId=product.ProductId,
            ProductName=product.ProductName,
            Price=product.Price,
            AvaiableStatus=product.AvaiableStatus
            };
            context.Products.Update(p);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Product p)
        {
            context.Products.Add(p);
            context.SaveChanges();
            hub.Clients.All.SendAsync("refreshNewProduct");
            return Json("Ok"); 
        }
        [HttpGet]
        public IActionResult ProductListCount()
        { 
            return Ok(context.Products.ToList().Count());
        }
        public IActionResult Delete(int id)
        {
            var a=context.Products.FirstOrDefault(a => a.ProductId == id);
            context.Products.Remove(a);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Chat() 
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("JoinPublicChat");
            }
            return View(context.Messages.ToList());
        } 
        public IActionResult ChatWithMe()  
        {
           
            
            return View();
        }
        [HttpPost]
        public  JsonResult Chat(Message cht) 
        {
           cht.UserName = HttpContext.Session.GetString("UserName");
            
            context.Messages.Add(cht);
           int a= context.SaveChanges();
           
            if(a>0)
            {
                return Json("Ok");
            }
            else
            {
                return Json(false);
            }
           
        }
        public JsonResult ValidateUserName(string UserName)
        {
            User a = context.Users.FirstOrDefault(s=>s.UserName== UserName);
            if(a==null)
            {
                return Json(true);
            }
            return Json(false);
        }
        public IActionResult JoinPublicChat()  
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Chat");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user) 
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                HttpContext.Session.SetString("UserName", user.UserName);
                context.Users.Add(user);
                context.SaveChanges();
                
            }

            return RedirectToAction("Chat");
        }
        public JsonResult TotalUser()
        {
            return Json(context.Users.ToList().Count());
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
