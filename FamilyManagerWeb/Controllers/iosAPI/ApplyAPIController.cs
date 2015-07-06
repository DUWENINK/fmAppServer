using BaseFunction;
using FamilyManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Threading;

namespace FamilyManagerWeb.Controllers
{
    public class ApplyAPIController : Controller
    {
        FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        public ActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" > DoCashAccounting(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string cAdd) <br/>");
            sb.AppendLine(" > DoBankAccounting(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string inUBID, string outUBID, string cAdd) <br/>");
            sb.AppendLine(" > DoZhuanZhang(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string inUBID, string outUBID, string cAdd) <br/>");
            return Content(sb.ToString());
        }

        public string DoCashAccounting(int userID, string ApplyDate, int FlowTypeID, string feeItemID, string feeItemName, string money, string cAdd)
        {
            string result = "{}";
            try
            {
                //获取记账日期
                string applyDate = ApplyDate;
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == FlowTypeID).Single();

                string flowTypeID = ffType.ID.ToString();

                //获取流动资金类型名称
                string flowTypeName = ffType.Name;

                //获取类型
                string InOutType = ffType.InOutType;
                //获取资金
                string iMoney = money;

                string isJieKuan = flowTypeName.Contains("借") == true ? "Y" : "N";

                //获取备注信息               

                string sql = "exec proc_AddCashAccouting '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + feeItemID + ",'" + feeItemName + "'," + iMoney + "," + userID.ToString() + ",'" + isJieKuan + "','N','" + cAdd + "'";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                result = WebComm.ReturnJsonForExterior(true, "现金记账成功！", "{}");
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "现金记账失败！" + ex.Message, "{}");
            }
            return result;
        }

        public string DoBankAccounting(int userID, string ApplyDate, int FlowTypeID,
            string feeItemID, string feeItemName, string money,
            string inUBID, string outUBID, string cAdd)
        {

            string result = "{}";
            try
            {
                //获取记账日期
                string applyDate = ApplyDate;
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == FlowTypeID).Single();

                string flowTypeID = ffType.ID.ToString();

                //获取流动资金类型名称
                string flowTypeName = ffType.Name;

                //获取类型
                string InOutType = ffType.InOutType;
                //获取资金
                string iMoney = money;

                string isJieKuan = flowTypeName.Contains("借") == true ? "Y" : "N";
                //获取入账银行信息
                string inUserBankID = inUBID;
                //获取出账银行信息
                string outUserBankID = outUBID;

                //获取备注信息               

                string sql = "exec proc_AddBankAccouting '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + feeItemID + ",'" + feeItemName + "'," + iMoney + "," + userID.ToString() + "," + inUserBankID + "," + outUserBankID + ",'" + isJieKuan + "','N','" + cAdd + "'";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                result = WebComm.ReturnJsonForExterior(true, "银行记账成功！", "{}");
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "银行记账失败！" + ex.Message, "{}");
            }
            return result;
        }

        public string DoZhuanZhang(int userID, string ApplyDate, int FlowTypeID,
            string feeItemID, string feeItemName, string money,
            string inUBID, string outUBID, string cAdd)
        {

            string result = "{}";
            try
            {
                //获取记账日期
                string applyDate = ApplyDate;
                //获取流动资金类型
                FundFlowType ffType = WebComm.GetFundFlowTypeList().Where(f => f.ID == FlowTypeID).Single();

                string flowTypeID = ffType.ID.ToString();

                //获取流动资金类型名称
                string flowTypeName = ffType.Name;

                //获取类型
                string InOutType = ffType.InOutType;
                //获取资金
                string iMoney = money;

                //获取入账银行信息
                string inUserBankID = inUBID;
                //获取出账银行信息
                string outUserBankID = outUBID;

                //获取备注信息               

                string sql = "exec proc_CashChange '" + applyDate + "'," + flowTypeID + ",'" + flowTypeName + "','" + InOutType + "'," + iMoney + "," + userID.ToString() + "," + inUserBankID + "," + outUserBankID + ",'" + cAdd + "'";
                LycSQLHelper.ExecuteCommand(CommandType.Text, sql);
                result = WebComm.ReturnJsonForExterior(true, "转账记账成功！", "{}");
            }
            catch (Exception ex)
            {
                result = WebComm.ReturnJsonForExterior(false, "转账记账失败！" + ex.Message, "{}");
            }
            return result;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }
    }
}
