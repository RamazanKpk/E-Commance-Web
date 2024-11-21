using Buisness.SessionAuth;
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
    public class UserGroupController : Controller
    {
        // GET: AdminPanel/UserGroup
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<UserGroup>().GetAll();
            return View(result);
        }
        [HttpGet]
        //[Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
                if (user != null)
                {
                    userGroup.CreatedBy = user.Id;
                    userGroup.ModifiedBy = user.Id;
                    userGroup.ModifiedDate = DateTime.Now;
                    userGroup.CreatedDate = DateTime.Now;

                    unitOfWork.GetRepository<UserGroup>().Add(userGroup);
                    unitOfWork.Complate();
                    return RedirectToAction("Index");
                }
            }
            return View(userGroup);
        }
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<UserGroup>().GetById(Id);
            return View(result);
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var result = unitOfWork.GetRepository<UserGroup>().GetById(Id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(UserGroup userGroup)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            var previousRecord = unitOfWork.GetRepository<UserGroup>().GetById(userGroup.Id);
            try
            {
                if (user != null)
                {
                    userGroup.ModifiedBy = user.ModifiedBy;
                    userGroup.ModifiedDate = DateTime.Now;
                    userGroup.CreatedBy = previousRecord.CreatedBy;
                    userGroup.CreatedDate = previousRecord.CreatedDate;

                    unitOfWork.GetRepository<UserGroup>().Update(userGroup);
                    unitOfWork.Complate();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(userGroup);
        }
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                UserGroup user = unitOfWork.GetRepository<UserGroup>().GetById(id);
                IEnumerable<UserGroup> users = new List<UserGroup>() { user };
                unitOfWork.GetRepository<UserGroup>().DeleteRange(users);
            }
            unitOfWork.Complate();
            return Json("Kayıt başarılı bir şekilde silindi");
        }
    }
}