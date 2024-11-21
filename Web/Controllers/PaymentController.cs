using Buisness.Interface.Abstract.WebAbstract;
using DataModel.ShopCartModels;
using DataModel.UserContactViewModels;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using DataAccess.Context;
using DataAccess;

namespace Web.Controllers
{
    public class PaymentController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        private readonly IUserInfoService<UserInfoListModel> _userInfoService;
        public PaymentController(IUserInfoService<UserInfoListModel> userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [HttpPost]
        public ActionResult Payment(UserInfoListModel model)
        {
            var cart = Session["Cart"] as Cart;
            var user = Session["User"] as User;
            if (model == null)
            {
                return HttpNotFound();
            }
            else
            {
                var userInfo = _userInfoService.UserAdd(model);
                var order = new Order();
                foreach (var item in cart.Items)
                {
                    order.ProductId = item.Id;
                    order.ProductAmount = item.Quantity;
                    order.ProductUnitPrice = item.Price;
                    order.TotalPrice = cart.TotalPrice;
                    order.UserId = userInfo.UserId;
                    order.DiscountRate = 20;
                    order.IsCanceled = false;
                    order.IsCompleated = false;
                }
                unitOfWork.GetRepository<Order>().Add(order);
                unitOfWork.Complate();

                string merchant_id = "484518";
                string merchant_key = "A3JgPhR7c5bCSrUu";
                string merchant_salt = "Lerx88xRFK1X6eaz";
                string emailstr = user.EmailAddress;
                decimal payment_amountstr = Convert.ToDecimal(cart.TotalPrice);
                string merchant_oid = order.Id.ToString();
                string user_namestr = model.FirstName + " " + model.LastName;
                string user_addressstr = model.Distirct + "/ " + model.Address + "/ " + model.City;
                string user_phonestr = model.Phone;
                string merchant_ok_url = "https://localhost:44300/odeme-basarili";
                string merchant_fail_url = "https://localhost:44300/Payment/Failed";
                string user_ip = GetLocalIPAddress();
                var user_basket = new object[cart.Items.Count()][];
                var products = cart.Items.ToList();
                for (int i = 0; i < products.Count; i++)
                {
                    var item = products[i];
                    user_basket[i] = new object[] { item.Name, item.Price, item.Quantity };
                }
                string lang = "tr";
                string card_type = "bonus";
                string debug_on = "1";
                string test_mode = "0";
                string non_3d = "0";
                string non3d_test_failed = "0";
                string no_installment = "1";
                string max_installment = "0";
                string installment_count = "0";
                string payment_type = "card";
                string post_url = "https://www.paytr.com/odeme";
                string currency = "TL";
                JavaScriptSerializer ser = new JavaScriptSerializer();
                string user_basket_json = ser.Serialize(user_basket);
                string Birlestir = string.Concat(merchant_id, user_ip, merchant_oid, emailstr, payment_amountstr.ToString(), payment_type, installment_count, currency, test_mode, non_3d, merchant_salt);
                HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
                byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));

                ViewBag.MerchantId = merchant_id;
                ViewBag.UserIp = user_ip;
                ViewBag.MerchantOid = merchant_oid;
                ViewBag.Email = emailstr;
                ViewBag.PaymentType = payment_type;
                ViewBag.PaymentAmount = payment_amountstr;
                ViewBag.InstallmentCount = installment_count;
                ViewBag.Currency = currency;
                ViewBag.TestMode = test_mode;
                ViewBag.Non3d = non_3d;
                ViewBag.MerchantOkUrl = merchant_ok_url;
                ViewBag.MerchantFailUrl = merchant_fail_url;
                ViewBag.UserName = user_namestr;
                ViewBag.UserAddress = user_addressstr;
                ViewBag.UserPhone = user_phonestr;
                ViewBag.UserBasket = user_basket_json;
                ViewBag.Non3dTestFailed = non3d_test_failed;
                ViewBag.DebugOn = debug_on;
                ViewBag.CardType = card_type;
                ViewBag.PostUrl = post_url;
                ViewBag.PaytrToken = Convert.ToBase64String(b);
                ViewBag.NoInstallment = no_installment;
                ViewBag.MaxInstallment = max_installment;
                ViewBag.Language = lang;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Notification()
        {
            string merchant_key = "A3JgPhR7c5bCSrUu";
            string merchant_salt = "Lerx88xRFK1X6eaz";

            string merchant_oid = Request.Form["merchant_oid"];
            string status = Request.Form["status"];
            string total_amount = Request.Form["total_amount"];
            string hash = Request.Form["hash"];

            string Birlestir = string.Concat(merchant_oid, merchant_salt, status, total_amount);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
            string token = Convert.ToBase64String(b);

            if (hash != token)
            {
                return Content("PAYTR notification failed: bad hash");
            }

            var order = GetOrderByMerchantOid(merchant_oid);

            if (order == null)
            {
                return HttpNotFound("Order not found");
            }

            if (status == "success")
            {
                ConfirmOrder(order);
                Response.Write("OK");
                return RedirectToAction("Success");
            }
            else
            {
                CancelOrder(order);
                Response.Write("OK");
                return RedirectToAction("Failed");
            }
        }

        private Order GetOrderByMerchantOid(string merchant_oid)
        {
            UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
            var order = unitOfWork.GetRepository<Order>().GetById(Convert.ToInt32(merchant_oid));
            if (order != null)
            {
                return order;
            }
            else
            {
                return null;
            }

        }

        private void ConfirmOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                order.IsCompleated = true;
            }
        }

        private void CancelOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                order.IsCanceled = true;
            }
        }
        public static string GetLocalIPAddress()
        {
            string localIP = "?";
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) 
                {
                    localIP = ip.ToString();
                    break;
                }
            }

            return localIP;
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Failed()
        {
            return View();
        }
        

    }
}