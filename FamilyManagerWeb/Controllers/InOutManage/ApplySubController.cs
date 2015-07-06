using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;
using BaseFunction;

namespace FamilyManagerWeb.Controllers
{
    public class ApplySubController : Controller
    {
        /// <summary>
        /// 控制器对应的视图路径
        /// </summary>
        private const string viewFolder = "~/Views/InOutManage/ApplySub/";
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        //记账明细列表页
        public ActionResult List(int mainBillCode)
        {
            var apply_sub = db.Apply_Sub.Where(a => a.ApplyMain_BillCode == mainBillCode);
            return View(viewFolder + "List.cshtml", apply_sub.ToList());
        }

        //弹出备注信息
        public ActionResult ToUpdateAdd(int id)
        {
            ViewBag.cadd = db.Apply_Sub.Find(id).CAdd;
            ViewBag.SubID = id;
            return View(viewFolder + "UpdateAdd.cshtml");
        }

        //弹出备注信息
        [HttpPost, ActionName("doUpdateAdd")]
        public string UpdateAdd(Apply_Sub sub)
        {
            try
            {
                Apply_Sub updateSub = db.Apply_Sub.Find(sub.ID);
                updateSub.CAdd = sub.CAdd;
                
                db.SaveChanges();
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "修改成功！", "ApplyInfoListNav", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "修改失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }

        //执行删除操作
        [HttpPost, ActionName("DeleteById")]
        public string DeleteConfirmed(int id)
        {
            try
            {
                string sql = "exec proc_DeleteAccouting " + id.ToString() + " ";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                return WebComm.ReturnAlertMessage(ActionReturnStatus.成功, "删除成功！", "ApplySubListNav", "", CallBackType.none, "");
            }
            catch (Exception ex)
            {
                return WebComm.ReturnAlertMessage(ActionReturnStatus.失败, "删除失败！" + ex.Message, "", "", CallBackType.none, "");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}