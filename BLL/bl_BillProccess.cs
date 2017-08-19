﻿using ShiMiao.DAL;
using ShiMiao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using WXPaySDK;

namespace BLL
{
   
    public class m_return
    {
        public string DM { get; set; }
        public string Msg { get; set; }
    }
    //业务处理程序
    public class bl_BillProccess
    {
        private static readonly ShiMiao.BLL.TD_Order_WeiXinPay weiXinPayBLL = new ShiMiao.BLL.TD_Order_WeiXinPay();

        public static m_return DownloadBill(DateTime _time)
        {
            /**
            * 下载对账单
            * @param DateTime _time 账单日期
            * @return m_return DM为SUCCESS表示成功 FALL表示失败 Msg为错误信息成功返回OK
            **/
            m_return ret = new m_return() { DM = "FAIL", Msg = "no result" };
            try
            {
                //（格式：20140603，一次只能下载一天的对账单）
                string strTime = _time.ToString("yyyyMMdd");
                //下载所有对账单 时间strTime 类型 ALL
                WxPayData result = WXPaySDK.DownloadBill.Run(strTime, "ALL");
                if (result.IsSet("return_code"))
                {//下载失败
                    ret.DM = result.GetValue("return_code").ToString();
                    if (result.IsSet("return_msg")) ret.Msg = result.GetValue("return_msg").ToString();
                    else ret.Msg = "no return_msg";
                }
                else if (result.IsSet("result"))
                {//下载成功 返回结果
                    List<WXBill> BillList = new List<WXBill>();
                    //合计支付总金额
                    decimal totalPay = 0;
                    //合计退款总金额
                    decimal totalRefund = 0;
                    //获取数据
                    string staticData = result.GetValue("result").ToString();
                    //每一行数据使用"\r\n" 隔开
                    string[] bilList = staticData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    //第一行数据为字段名称 每个字段名以','隔开
                    string[] headList = bilList[0].Split(',');
                    for (int i = 1; i < bilList.Count() - 2; ++i)
                    {
                        string info = bilList[i];
                        string[] tmpStrList = info.Split(',');
                        for (int j = 0; j < tmpStrList.Count(); ++j)
                        {
                            tmpStrList[j] = tmpStrList[j].Replace("`", "");
                        }
                        //账单明细处理 传入字段名称和数据
                        WXBill bill = new WXBill(headList, tmpStrList);
                        //加入到明细列表中
                        BillList.Add(bill);
                        //统计
                        totalPay += bill.Cost;
                        totalRefund += bill.RefundMoney;
                    }
                    //获取账单中合计数据 进行比对
                    string[] totalHead = bilList[bilList.Count() - 2].Split(',');
                    string[] totalInfo = bilList[bilList.Count() - 1].Split(',');
                    for (int j = 0; j < totalInfo.Count(); ++j)
                    {
                        totalInfo[j] = totalInfo[j].Replace("`", "");
                    }
                    WXBillAmount BillAmount = new WXBillAmount(totalHead, totalInfo);
                    //本地比对成功
                    if (totalPay == BillAmount.TotalCost && totalRefund == BillAmount.TotalRefundMoney && BillList.Count() == BillAmount.Amount)
                    {//本地对账成功  插入数据表
                        //using 帐号可使用zfb 也可在配置中获取
                        dl_BillProccess dl_bill = new dl_BillProccess();
                        if (dl_bill.insertBills(BillList))
                        {
                            ret.DM = "SUCCESS";
                            ret.Msg = "OK";
                        }
                        else
                        {
                            ret.DM = "FAIL";
                            ret.Msg = "insert record fail";
                        }

                    }
                }//else DM = "FAIL", Msg = "no result"
            }
            catch (Exception ex)
            {
                ret.DM = "FAIL";
                ret.Msg = "proccess catch a exception";
                Log.Error("bl_billproccess", ex.ToString());
            }

            return ret;
        }
        public static m_return CheckBill(DateTime _time)
        {
            dl_BillProccess dl_bill = new dl_BillProccess();
            var bills = dl_bill.getNotCheckYet();

            foreach (var bill in bills)
            {
                int splitIndex = bill.OutTradeNo.IndexOf("_");
                string orderID = bill.OutTradeNo;
                if (splitIndex > 0)
                {
                    orderID = bill.OutTradeNo.Substring(0, splitIndex);
                }
                string where = string.Format("OrderID='{0}'", orderID);
                IList<ShiMiao.Model.TD_Order_WeiXinPay> payList = weiXinPayBLL.GetList(where, "PayTime desc", null);
                var pay = payList.Where(a => a.OrderID == bill.OutTradeNo && a.Status == 0).ToList<ShiMiao.Model.TD_Order_WeiXinPay>().FirstOrDefault();
                if (null == pay)
                {
                    continue;
                }
                pay.OrderID = orderID;
                pay.WeiXinOrderID = bill.TransactionID;
                //pay.OrderFee = bill..total_fee;
                pay.CashFee = Convert.ToInt32(bill.Cost);
                pay.Status = 1;
                pay.CallBackTime = bill.TranDate;
                weiXinPayBLL.Sync(pay, orderID);

            }
            return new m_return() { DM = "OK" };
        }
    }
}
