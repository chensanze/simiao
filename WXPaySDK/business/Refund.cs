using System;
using System.Collections.Generic;
using System.Web;

namespace WXPaySDK
{
    public class Refund
    {
        /***
        * 申请退款完整业务流程逻辑
        * @param transaction_id 微信充值订单号
        * @param out_refund_no 退款订单号
        * @param total_fee 订单总金额
        * @param refund_fee 退款金额
        * @return 退款结果（xml格式）
        */
        public static WxPayData Run(string transaction_id, string out_refund_no, string total_fee, string refund_fee)
        {
            Log.Info("Refund", "Refund is processing...");

            WxPayData data = new WxPayData();
            if (transaction_id.Contains(WxPayConfig.MCHID))
            {
                data.SetValue("out_trade_no", transaction_id);//充值商户订单号
            }
            else
            {
                data.SetValue("transaction_id", transaction_id);//充值微信订单号
            }
            data.SetValue("total_fee", int.Parse(total_fee));//订单总金额
            data.SetValue("refund_fee", int.Parse(refund_fee));//退款金额
            data.SetValue("out_refund_no", out_refund_no);//退款订单号
            data.SetValue("op_user_id", WxPayConfig.MCHID);//操作员，默认为商户号

            WxPayData result = WxPayApi.Refund(data,10);//提交退款申请给API，接收返回数据
            if(!result.CheckSign())
            {
                throw new WxPayException("Refund response error [CHECKSIGN]");
            }
            Log.Info("Refund", "Refund process complete, result : " + result.ToXml());
            return result;
        }
    }
}