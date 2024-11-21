using DataAccess;
using DataAccess.Context;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Notification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                Response.Write("PAYTR notification failed: bad hash");
                Response.End();
            }

            var order = GetOrderByMerchantOid(merchant_oid);

            if (order == null)
            {
                Response.Write("Order not found");
                Response.End();
            }

            if (status == "success")
            {
                ConfirmOrder(order);
                Response.Write("OK");
            }
            else
            {
                CancelOrder(order);
                Response.Write("OK");
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
    }
}