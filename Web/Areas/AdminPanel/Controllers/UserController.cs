using DataAccess;
using DataAccess.Context;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.AdminPanel.Controllers
{
    public class UserController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        // GET: AdminPanel/User
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<User>().GetAll();
            return View(result);
        }
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<User>().GetById(Id);
            return View(result);
        }
        public ActionResult Edit(int Id)
        {
            var result = unitOfWork.GetRepository<User>().GetById(Id);
            List<UserGroup> groups =  unitOfWork.GetRepository<UserGroup>().GetAll().ToList();          
            SelectList selectLists = new SelectList(groups, "Id", "Title");
            ViewBag.UserGroupId = selectLists;
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            try
            {
                unitOfWork.GetRepository<User>().Update(user);
                unitOfWork.Complate();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                User user = unitOfWork.GetRepository<User>().GetById(id);
                IEnumerable<User> users = new List<User>() {user};
                unitOfWork.GetRepository<User>().DeleteRange(users);
            }
            unitOfWork.Complate();
            return Json("Kayıt başarılı bir şekilde silindi");
        }
    }
}