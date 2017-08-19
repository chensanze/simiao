﻿using System;
using System.Collections.Generic;
using System.Web;

namespace WXPaySDK
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public static string APPID { get { return System.Configuration.ConfigurationManager.AppSettings["AppID"]; } }
        public static string MCHID { get; }=System.Configuration.ConfigurationManager.AppSettings["ShopID"];
        public const string DEVICEINFO = "WEB";
        public static string KEY { get; } = System.Configuration.ConfigurationManager.AppSettings["ShopKey"]; //API 密钥 生成网站http://www.sexauth.com/
        public static string APPSECRET { get { return System.Configuration.ConfigurationManager.AppSettings["AppSecret"]; } }

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cert\\apiclient_cert.p12";
        public static string SSLCERT_PASSWORD { get; }=System.Configuration.ConfigurationManager.AppSettings["ShopID"];//证书密码 默认为商户ID 参见http://wenku.baidu.com/link?url=4HFzI_uWeXDLCf59hTVgAW5LU_texVcOGNoQJsW_IRLjIKe4E0gAQkj1W3dVfcTDCho42ZIGKXWvHw3vKAHrO_ZV-pOsat2tGCRRmbibqni



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NOTIFY_URL = "http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "0.0.0.0:0";//"http://10.152.18.220:8080"; csz 不设置

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        //public const int LOG_LEVENL = 3;
        public static int LOG_LEVENL { 
            get { 
                string val = System.Configuration.ConfigurationManager.AppSettings["LOG_LEVENL"];
                if (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val))
                {
                    return 1;
                }
                else return int.Parse(val);
            } 
        }
    }
}