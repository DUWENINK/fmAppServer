using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using FamilyManagerWeb.Models.ViewModels;

namespace FamilyManagerWeb.Controllers
{
    public class LycMVCController : BaseController
    {
        /// <summary>
        /// 调用存储过程，用于查询操作，返回一个datatable集合
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="par">参数集合</param>
        /// <returns></returns>
        protected DataTable QueryStoredProcedure(string procedureName, params SqlParameter[] par)
        {
            DataTable dt = new DataTable();
            SqlConnection conn;
            try
            {
                string connStr = System.Web.Configuration.WebConfigurationManager.AppSettings["SqlCONNECTIONSTRING4"];
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Clear();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "QueryUserFullMoney";
                    foreach (var item in par)
                    {
                        cmd.Parameters.Add(item);
                    }

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "default");
                    dt = ds.Tables["default"];
                    cmd.Dispose();
                }
            }
            catch
            {
                throw new Exception("获取数据时错误！");
            }
            return dt;
        }

        /// <summary>
        /// 同步记账信息，并使用ADO.NET事务
        /// </summary>
        /// <param name="list">要同步的记账信息列表</param>
        /// <returns>返回true执行成功，false执行失败</returns>
        protected bool SyncApplyWithProcAndTransaction(List<Apply_temp_sync_VM> list)
        {
            bool success = true;
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn;
            try
            {
                string connStr = System.Web.Configuration.WebConfigurationManager.AppSettings["SqlCONNECTIONSTRING4"];
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.Transaction = conn.BeginTransaction();//开启事务

                    foreach (var item in list)
                    {
                        //循环调用存储过程
                        cmd.CommandType = CommandType.StoredProcedure;
                        switch (item.flowTypeName)
                        {
                            #region 配置现金记账的存储过程
                            case "现金记账":
                                cmd.CommandText = "proc_AddCashAccouting";

                                //设置参数
                                SqlParameter[] sp1 = new SqlParameter[]
                                {
                                    new SqlParameter{ParameterName = "@applyDate",Value = item.applyDate},
                                    new SqlParameter{ParameterName = "@flowTypeID",Value = item.flowTypeID},
                                    new SqlParameter{ParameterName = "@flowTypeName",Value = item.flowTypeName},
                                    new SqlParameter{ParameterName = "@InOutType",Value = item.inOutType},
                                    new SqlParameter{ParameterName = "@FeeItemID",Value = item.feeItemID},
                                    new SqlParameter{ParameterName = "@FeeItemName",Value = item.feeItemName},
                                    new SqlParameter{ParameterName = "@iMoney",Value = item.imoney},
                                    new SqlParameter{ParameterName = "@UserID",Value = item.userID},
                                    new SqlParameter{ParameterName = "@BJieKuan",Value = "N"},
                                    new SqlParameter{ParameterName = "@BHuanKuan",Value = "N"},
                                    new SqlParameter{ParameterName = "@CAdd",Value = item.cAdd}
                                };

                                //将参数添加到cmd对象中
                                cmd.Parameters.Clear();
                                foreach (var p in sp1)
                                {
                                    cmd.Parameters.Add(p);
                                }
                                break;
                            #endregion

                            #region 配置银行记账的存储过程
                            case "银行记账":
                                cmd.CommandText = "proc_AddBankAccoutingg";

                                //设置参数
                                SqlParameter[] sp2 = new SqlParameter[]
                                {
                                    new SqlParameter{ParameterName = "@applyDate",Value = item.applyDate},
                                    new SqlParameter{ParameterName = "@flowTypeID",Value = item.flowTypeID},
                                    new SqlParameter{ParameterName = "@flowTypeName",Value = item.flowTypeName},
                                    new SqlParameter{ParameterName = "@InOutType",Value = item.inOutType},
                                    new SqlParameter{ParameterName = "@FeeItemID",Value = item.feeItemID},
                                    new SqlParameter{ParameterName = "@FeeItemName",Value = item.feeItemName},
                                    new SqlParameter{ParameterName = "@iMoney",Value = item.imoney},
                                    new SqlParameter{ParameterName = "@UserID",Value = item.userID},
                                    new SqlParameter{ParameterName = "@InUserBankID",Value = item.inUserBankID},
                                    new SqlParameter{ParameterName = "@OutUserBankID",Value = item.outUserBankID},
                                    new SqlParameter{ParameterName = "@BJieKuan",Value = "N"},
                                    new SqlParameter{ParameterName = "@BHuanKuan",Value = "N"},
                                    new SqlParameter{ParameterName = "@CAdd",Value = item.cAdd}
                                };

                                //将参数添加到cmd对象中
                                cmd.Parameters.Clear();
                                foreach (var p in sp2)
                                {
                                    cmd.Parameters.Add(p);
                                }
                                break;
                            #endregion

                            #region 配置内部转账的存储过程
                            case "内部转账":
                                cmd.CommandText = "proc_CashChange";

                                //设置参数
                                SqlParameter[] sp3 = new SqlParameter[]
                                {
                                    new SqlParameter{ParameterName = "@applyDate",Value = item.applyDate},
                                    new SqlParameter{ParameterName = "@flowTypeID",Value = item.flowTypeID},
                                    new SqlParameter{ParameterName = "@flowTypeName",Value = item.flowTypeName},
                                    new SqlParameter{ParameterName = "@InOutType",Value = item.inOutType},
                                    new SqlParameter{ParameterName = "@iMoney",Value = item.imoney},
                                    new SqlParameter{ParameterName = "@UserID",Value = item.userID},
                                    new SqlParameter{ParameterName = "@InUserBankID",Value = item.inUserBankID},
                                    new SqlParameter{ParameterName = "@OutUserBankID",Value = item.outUserBankID},
                                    new SqlParameter{ParameterName = "@CAdd",Value = item.cAdd}
                                };

                                //将参数添加到cmd对象中
                                cmd.Parameters.Clear();
                                foreach (var p in sp3)
                                {
                                    cmd.Parameters.Add(p);
                                }
                                break;
                            #endregion
                            default:
                                cmd.Parameters.Clear();
                                cmd.CommandText = "";
                                break;
                        }
                        if (cmd.CommandText == "")
                        {
                            continue;//如果资金类型不对，跳过这笔记账信息
                        }

                        //执行存储过程
                        cmd.ExecuteNonQuery();
                    }

                    cmd.Transaction.Commit();//提交事务
                    cmd.Dispose();
                }
            }
            catch
            {
                cmd.Transaction.Rollback();
                success = false;
            }

            return success;
        }

        /// <summary>
        /// 执行存储过程（增删改操作）
        /// </summary>
        /// <param name="proc_Name">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>返回true执行成功，返回false执行失败</returns>
        protected bool ExecStoredProcedure(string proc_Name, params SqlParameter[] pars)
        {
            bool success = true;
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn;
            try
            {
                string connStr = System.Web.Configuration.WebConfigurationManager.AppSettings["SqlCONNECTIONSTRING4"];
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    //配置cmd参数
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc_Name;
                    //将参数添加到cmd对象中
                    cmd.Parameters.Clear();
                    foreach (var p in pars)
                    {
                        cmd.Parameters.Add(p);
                    }
                    //执行存储过程
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}
