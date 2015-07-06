using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Xml.Linq;

using Newtonsoft.Json;

using FamilyManagerWeb.Models;
using System.IO;

namespace FamilyManagerWeb.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/MainManage/Home/";
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();
        public ActionResult Index()
        {
            ViewBag.Message = "修改此模板以快速启动你的 ASP.NET MVC 应用程序。";

            return View(viewFolder + "Index.cshtml");
        }

        // 显示首页
        public ActionResult DwzIndex()
        {
            if (Session[SessionList.FamilyManageUser.ToString()] == null)
            {
                return RedirectToAction("GoTo404Page", "CommView");
            }
            SetUserPower();

            return View(viewFolder + "DwzIndex.cshtml");
        }

        [ActionName("toLogin")]
        public ActionResult DwzLogin()
        {
            return View(viewFolder + "DwzLogin.cshtml");
        }

        [HttpPost]
        public string doLogin(User user)
        {
            int userCode = Convert.ToInt32(user.cUserCode);
            var userInfo = db.Users.Where(u => u.cUserCode == userCode && u.cUserPwd == user.cUserPwd).FirstOrDefault();
            if (userInfo == null)
            {
                return "{\"result\":\"f\"}";
            }
            else
            {
                user = db.Users.Where(u => u.cUserCode == user.cUserCode).FirstOrDefault();
                Session[SessionList.FamilyManageUser.ToString()] = user;
                return "{\"result\":\"s\"}";
            }

        }

        [HttpPost]
        public string LogOut()
        {
            try
            {

                Session[SessionList.FamilyManageUser.ToString()] = null;
            }
            catch (Exception)
            {
                return "{\"result\":\"f\"}";
            }
            return "{\"result\":\"s\"}";
        }

        //创建费用项目xml
        public string CreateFeeXml()
        {

            string path = WebComm.FeeItemXmlPath;
            /* 创建xml文件
            XDocument xdoc = new XDocument(
                                            new XDeclaration("1.0", "utf-8", "yes"),
                                            new XElement("FeeItemList")
                                          );
            xdoc.Save(path);
            return "成功";
            */

            /* 通过List集合遍历添加xml文件*/
            XDocument xdoc = XDocument.Load(path);
            //XElement root = xdoc.Element("FeeItemList");
            //root.Add(
            //            WebComm.GetFeeItemList().Select(c =>
            //            new XElement("FeeItem",
            //                            new XAttribute("FeeItemID", c.FeeItemID),
            //                            new XAttribute("FeeItemName", c.FeeItemName),
            //                            new XAttribute("FeeItemClassID", c.FeeItemClassID),
            //                            new XAttribute("IsLast", c.IsLast)
            //                        ))
            //        );
            XElement root = xdoc.Element("FeeItems");
            root.Add(
                        WebComm.GetFeeItemList().Select(c =>
                        new XElement("FeeItem",
                                        new XElement("FeeItemID", c.FeeItemID),
                                        new XElement("FeeItemName", c.FeeItemName),
                                        new XElement("FeeItemClassID", c.FeeItemClassID),
                                        new XElement("IsLast", c.IsLast)
                                    ))
                    );
            xdoc.Save(path);
            return "成功";
        }


        #region 私有方法



        /// <summary>
        /// 设置用户列表权限
        /// </summary>
        private void SetUserPower()
        {
            User loginUser = Session[SessionList.FamilyManageUser.ToString()] as User;

            //用户列表权限
            if (UserPower.adminUserCode.Contains(loginUser.cUserCode))
            {
                ViewBag.menuPower_userList = true;
                ViewBag.menuPower_userAdd = true;
            }
            else
            {
                ViewBag.menuPower_userList = true;
                ViewBag.menuPower_userAdd = false;
            }

            //银行权限
            if (UserPower.adminUserCode.Contains(loginUser.cUserCode))
            {
                ViewBag.menuPower_BankMainList = true;
            }
            else
            {
                ViewBag.menuPower_BankMainList = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion

    }
}
