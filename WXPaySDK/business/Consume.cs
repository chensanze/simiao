using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WX.Entity.Pay;


namespace WXPaySDK
{
    public class Consume
    {
        public static WxPayData Run(TranRecord _tr, string _ip, string _name, string notify_url, bool isCredit)
        {
            WxPayData data = new WxPayData();
            data.SetValue("body", string.Format("{0}-{1}-充值", _tr.BRBH, _name));
            data.SetValue("attach", _tr.QQLSH);
            data.SetValue("out_trade_no", _tr.QQLSH);
            data.SetValue("total_fee", Convert.ToInt32(_tr.CZJE * 100));//将金额转换为单位分 int
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", _tr.WBBH);
            data.SetValue("spbill_create_ip", _ip);//终端ip	 
            data.SetValue("notify_url", notify_url);
            if(!isCredit) data.SetValue("limit_pay", "no_credit");//不能使用信用卡
            //发起交易 超时时间10s
            WxPayData result = WxPayApi.UnifiedOrder(data,10);
            if (!result.IsSet("appid") )
            {
                //Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error(appid is null)!");
            }
            if (!result.IsSet("prepay_id") )
            {
                //Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error(prepay_id is null)!");
            }
            if ( result.GetValue("prepay_id").ToString() == "")
            {
                //Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error(prepay_id is empty)!");
            }
            if(!result.CheckSign())
            {
                Log.Error("Consume", "CheckSign fail! tranrecord : " + _tr.toPrintStr() + " ip : " + _ip);
                throw new WxPayException("CheckSign fail!");
            }
            return result;
        }
    }
}
