using Buisness.SessionAuth;
using DataAccess;
using DataAccess.Context;
using Entity;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Web.Areas.AdminPanel.Controllers
{
    public class AccountsController : Controller
    {
        // GET: AdminPanel/Accounts
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
       
        public ActionResult Login(User user)
        {
            var logUser = unitOfWork.UserRepository.LogUser(user);
            if (logUser != null)
            {
                var userGroup = unitOfWork.GetRepository<UserGroup>().GetById(logUser.UserGroupId);
                //FormsAuthentication.SetAuthCookie(logUser.Name, false);
                SessionManeger.SetSession(logUser.Name, userGroup.Title);
                //if (logUser.UserGroupId == 3)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                //if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                return RedirectToAction("Index", "AdminHome");


            }
            TempData["Message"] = "Kullanıcı bilgileri hatalı tekrar giriş yapınız?";
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string userName, string lastName, string email, string phoneNumber, string password, string repeatPassword)
        {
            ETicaretContext db = new ETicaretContext();
            string message;
            var userGroup = unitOfWork.GetRepository<UserGroup>().GetAll();
            var userGroupId = userGroup.Where(x=> x.Title == "Member").FirstOrDefault();
            var checkUser = unitOfWork.UserRepository.CheckUser(email,phoneNumber);
            if (checkUser == false)
            {
                if (password != repeatPassword)
         
                
                {
                    message = "Girdiğiniz şifreler birbiriyle uyuşmuyor!"; 
                }
                var user = new User
                {
                    Name = userName,
                    Surname = lastName,
                    UserGroupId = userGroupId.Id,
                    EmailAddress = email,
                    PhoneNumber = phoneNumber,
                    Password = password,
                    CreatedDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true,
                };
                
                unitOfWork.GetRepository<User>().Add(user);
                unitOfWork.Complate();
                message = "Kayıt başarılı giriş yapabilrsiniz";
                
            }
            else
            {
                message = "Kullanıcı bilgileri zaten kayıtlı!";
            }
            TempData["Message"] = message;
            return RedirectToAction("Login", "Accounts");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult Logout()
        {
            SessionManeger.ClearSession();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Accounts");
        }
    }
}