﻿using ShopAoThung.Common;
using ShopAoThung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopAoThung.Controllers
{
    public class AuthSiteController : Controller
    {
        // GET: AuthSite
        private BookstoreDbContext db = new BookstoreDbContext();
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(FormCollection fc)
        {
            string Username = fc["uname"];
            string Pass = Mystring.ToMD5(fc["psw"]);

            var user_account = db.Users.Where(m => (m.username == Username) && (m.access == 1));

            if (user_account.Count() == 0)
            {
                Message.set_flash("Tên đăng nhập không tồn tại", "danger");
                return View("login");
            }
            else
            {
                var pass_account = db.Users.Where(m => m.status == 1 && (m.password == Pass) && (m.access == 1));

                if (pass_account.Count() == 0)
                {
                    Message.set_flash("Mật khẩu không đúng", "danger");
                    return View("login");
                }

                else
                {

                    var user = user_account.First();
                    Session.Add(CommonConstants.CUSTOMER_SESSION, user);
                    if (!Response.IsRequestBeingRedirected)
                        Message.set_flash("Đăng nhập thành công", "success");
                    return Redirect("~/");

                }
            }
 
        }

        public void logout()
        {
            Session["id"] = "";
            Session["user"] = "";
            CommonConstants.CUSTOMER_SESSION = "";
            Response.Redirect("~/dang-nhap");
            Message.set_flash("Đăng xuất thành công", "success");
        }
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(Muser muser, FormCollection fc)
        {
            string uname = fc["uname"];
            string fname = fc["fname"];
            string Pass = Mystring.ToMD5(fc["psw"]);
            string Pass2 = Mystring.ToMD5(fc["repsw"]);
            if (Pass2 != Pass)
            {
                ViewBag.error = "Mật khẩu không khớp";
                return View("register");
            }
            string email = fc["email"];
            string address = fc["address"];
            string phone = fc["phone"];
            if (ModelState.IsValid)
            {
                var Luser = db.Users.Where(m => m.status == 1 && m.username == uname && m.access == 1);
                if (Luser.Count() > 0)
                {
                    ViewBag.error = "Tên Đăng Nhập đã tồn tại";
                    return View("register");
                }
                else
                {
                    muser.img = "defalt.png";
                    muser.password = Pass;
                    muser.username = uname;
                    muser.fullname = fname;
                    muser.email = email;
                    muser.address = address;
                    muser.phone = phone;
                    muser.gender = "nam";
                    muser.access = 1;
                    muser.created_at = DateTime.Now;
                    muser.updated_at = DateTime.Now;
                    muser.created_by = 1;
                    muser.updated_by = 1;
                    muser.status = 1;
                    db.Users.Add(muser);
                    db.SaveChanges();
                    Message.set_flash("Đăng ký tài khoản thành công ", "success");
                    return View("register");
                }

            }
            Message.set_flash("Đăng ký tài khoản thất bai", "danger");
            return Redirect("~/");
        }



    }
}