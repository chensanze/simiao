using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiMiao.Model
{
    public class WXBillAmount
    {//,,,总代金券或立减优惠退款金额,
        public Int64 Amount { get; set; } //总交易单数
        public decimal TotalCost { get; set; }//总交易额
        public decimal TotalRefundMoney { get; set; }//总退款金额
        public decimal TotalRefundLuckyMoney { get; set; }//总企业红包退款金额
        public decimal TotalExFee { get; set; }    //手续费总金额
        public WXBillAmount(string[] header, string[] data)
        {
            int countHead = header.Count();
            int countData = data.Count();
            for (int i = 0; i < countData && i < countHead; ++i)
            {
                string heardStr = header[i];
                string dataStr = data[i];
                switch (heardStr)
                {
                    case "总交易单数":
                        Amount = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToInt64(dataStr);
                        break;
                    case "总交易额":
                        TotalCost = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                    case "总退款金额":
                        TotalRefundMoney = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                    case "总企业红包退款金额":
                        TotalRefundLuckyMoney = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                    case "手续费总金额":
                        TotalExFee = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                }
            }
        }
    }
    public class WXBill
    {
        public string RecordID { get; set; }//表记录ID
        public DateTime TranDate { get; set; } //交易时间
        public string AppID { get; set; } //公众账号ID
        public string MCHID { get; set; } //商户号
        public string SubMCHID { get; set; } //子商户号
        public string DeviceID { get; set; }//设备号
        public string TransactionID { get; set; }//微信订单号
        public string OutTradeNo { get; set; } //商户订单号
        public string UserInfo { get; set; }// 用户标识
        public string TranType { get; set; }//交易类型
        public string TranSta { get; set; }//交易状态
        public string PayBank { get; set; }//付款银行
        public string Currency { get; set; }//货币种类
        public decimal Cost { get; set; }//总金额
        public decimal EnterpriseLuckyMoney { get; set; }//企业红包金额
        public string RefundID { get; set; }//微信退款单号
        public string OutRefundNo { get; set; }//商户退款单号
        public decimal RefundMoney { get; set; }//退款金额
        public decimal EnterpriseRefundLuckyMoney { get; set; }//企业红包退款金额
        public string RefundType { get; set; }//退款类型
        public string RefundSta { get; set; }//退款状态
        public string GoodsName { get; set; }//商品名称
        public string MerchantData { get; set; }//商户数据包
        public decimal ExFee { get; set; }//手续费
        public string Rates { get; set; }//费率
        public WXBill()
        {

        }
        public WXBill(string[] header, string[] data)
        {
            int countHead = header.Count();
            int countData = data.Count();
            for (int i = 0; i < countData && i < countHead; ++i)
            {
                string heardStr = header[i];
                string dataStr = data[i];
                switch (heardStr)
                {
                    case "交易时间":
                        {
                            TranDate = DateTime.ParseExact(dataStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                        }
                        break;
                    case "公众账号ID":
                        AppID = dataStr;
                        break;
                    case "商户号":
                        MCHID = dataStr;
                        break;
                    case "子商户号":
                        SubMCHID = dataStr;
                        break;
                    case "设备号":
                        DeviceID = dataStr;
                        break;
                    case "微信订单号":
                        TransactionID = dataStr;
                        break;
                    case "商户订单号":
                        OutTradeNo = dataStr;
                        break;
                    case "用户标识":
                        UserInfo = dataStr;
                        break;
                    case "交易类型":
                        TranType = dataStr;
                        break;
                    case "交易状态":
                        TranSta = dataStr;
                        break;
                    case "付款银行":
                        PayBank = dataStr;
                        break;
                    case "货币种类":
                        Currency = dataStr;
                        break;
                    case "总金额":
                        Cost = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                    case "企业红包金额":
                        EnterpriseLuckyMoney = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                    case "微信退款单号":
                        RefundID = dataStr;
                        break;
                    case "商户退款单号":
                        OutRefundNo = dataStr;
                        break;
                    case "退款金额":
                        RefundMoney = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                    case "企业红包退款金额":
                        EnterpriseRefundLuckyMoney = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                    case "退款类型":
                        RefundType = dataStr;
                        break;
                    case "退款状态":
                        RefundSta = dataStr;
                        break;
                    case "商品名称":
                        GoodsName = dataStr;
                        break;
                    case "商户数据包":
                        MerchantData = dataStr;
                        break;
                    case "手续费":
                        ExFee = string.IsNullOrEmpty(dataStr) ? 0 : Convert.ToDecimal(dataStr);
                        break;
                    case "费率":
                        Rates = dataStr;
                        break;
                }
            }
        }
        public bool Equals(WXBill _bill)
        {
            if ((_bill.TransactionID == this.TransactionID) && (_bill.TranType == this.TranType) && (_bill.TranSta == this.TranSta) && (_bill.OutTradeNo == this.OutTradeNo) && (_bill.RefundID == this.RefundID) && (_bill.RefundType == this.RefundType) && (_bill.RefundSta == this.RefundSta) && (_bill.OutRefundNo == this.OutRefundNo) && (_bill.Cost == this.Cost) && (_bill.RefundMoney == this.RefundMoney) && (_bill.EnterpriseLuckyMoney == this.EnterpriseLuckyMoney) && (_bill.EnterpriseRefundLuckyMoney == this.EnterpriseRefundLuckyMoney) && (_bill.RefundMoney == this.RefundMoney) && (_bill.GoodsName == this.GoodsName) && (_bill.MerchantData == this.MerchantData))
            {
                return true;
            }
            return false;
        }
        public string toPrintStr()
        {
            return string.Format("RecordID:{0};TranDate:{1};AppID:{2};MCHID:{3};SubMCHID:{4};DeviceID:{5};TransactionID:{6};OutTradeNo:{7};UserInfo:{8};TranType:{9};TranSta:{10};PayBank:{11};Currency:{12};Cost:{13};EnterpriseLuckyMoney:{14};RefundID:{15};OutRefundNo:{16};RefundMoney:{17};EnterpriseRefundLuckyMoney:{18};RefundType:{19};RefundSta:{20};GoodsName:{21};MerchantData:{22};ExFee:{23};Rates:{24};", RecordID, TranDate, AppID, MCHID, SubMCHID, DeviceID, TransactionID, OutTradeNo, UserInfo, TranType, TranSta, PayBank, Currency, Cost, EnterpriseLuckyMoney, RefundID, OutRefundNo, RefundMoney, EnterpriseRefundLuckyMoney, RefundType, RefundSta, GoodsName, MerchantData, ExFee, Rates);
        }
    }
}
