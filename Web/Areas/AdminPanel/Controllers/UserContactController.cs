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
    public class UserContactController : Controller
    {
        // GET: AdminPanel/UserContact
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<UserContact>().GetAll();
            return View(result);
        }
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<UserContact>().GetById(Id);
            return View(result);
        }
        //public ActionResult Edit(int Id)
        //{
        //    var result = unitOfWork.GetRepository<UserContact>().GetById(Id);
        //    List<UserGroup> groups = unitOfWork.GetRepository<UserGroup>().GetAll().ToList(); SelectList selectLists = new SelectList(groups, "Id", "Title");
        //    ViewBag.UserGroupId = selectLists;
        //    return View(result);
        //}
        //[HttpPost]
        //public ActionResult Edit(UserContact userContact)
        //{
        //    try
        //    {
        //        unitOfWork.GetRepository<UserContact>().Update(userContact);
        //        unitOfWork.Complate();
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
        //    }
        //    return View(userContact);
        //}
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                UserContact userContact = unitOfWork.GetRepository<UserContact>().GetById(id);
                IEnumerable<UserContact> userContacts = new List<UserContact>() { userContact };
                unitOfWork.GetRepository<UserContact>().DeleteRange(userContacts);
            }
            unitOfWork.Complate();
            return Json("Kayıt başarılı bir şekilde silindi");
        }
    }
}