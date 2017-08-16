﻿using ShiMiao.Common;
using ShiMiao.WebCommon;
using Stone.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ShiMiao.Web.Areas.Shop.Controllers
{
    public class Goods2Controller : MiniWebBaseController
    {
        private static readonly BLL.TD_Shop_Order orderBLL = new BLL.TD_Shop_Order();
        private static readonly BLL.TD_Shop_Goods goodsBLL = new BLL.TD_Shop_Goods();

        [ShiMiaoWXValidation]
        public ActionResult Index()
        {
            int orgID = int.Parse(DESEncrypt.Decrypt(ViewBag.EnOrgID));
            StringBuilder where = new StringBuilder();
            where.AppendFormat("orders.OrgID={0} and orders.OrderType={1} and orders.IsPay='1'", orgID, (int)Constants.DonationType.Shop1);
            IList<decimal> counts = orderBLL.GetCount(where.ToString());
            ViewBag.RecordCount = counts[0];
            ViewBag.TotalMoney = counts[1];
            string url = Request.Url.AbsoluteUri;
            WeiXinPort port = new WeiXinPort();
            port.InitSignature(url);
            ThirdSharedInfo share = port.GetShareInfo(url);
            share.Title = string.Format("{0}广种福田", "净名寺");
            share.Link = url;
            share.Image = Request.Url.Scheme + "://" + Request.Url.Authority + "/Content/Org/JMS/logo.jpg";
            share.Content = "欢迎转发分享，功德无量";
            ViewBag.ShareInfo = share;
            var member = MemberData.GetMember();
            if (member != null)
            {
                ViewBag.NickName = member.NickName;
                ViewBag.HeaderImage = member.HeaderImage;
            }

            //大梁A 9918ac35 - a983 - 40c8 - 822a - 2a71d4866c0f
            var dla = goodsBLL.GetModel("9918ac35-a983-40c8-822a-2a71d4866c0f");
            ViewBag.dla = dla.Balance + dla.Frozen;
            ViewBag.dlaCount = dla.Amount - dla.Balance - dla.Frozen;
            //大梁B 3a2a9ebe-9900-4ede-966f-23e343c09445
            var dlb = goodsBLL.GetModel("3a2a9ebe-9900-4ede-966f-23e343c09445");
            ViewBag.dlb = dlb.Balance + dlb.Frozen;
            ViewBag.dlbCount = dlb.Amount - dlb.Balance - dlb.Frozen;
            //副梁A fbc6178f-ba45-4376-b0e1-fe22c36d564b
            var fla = goodsBLL.GetModel("fbc6178f-ba45-4376-b0e1-fe22c36d564b");
            ViewBag.fla = fla.Balance + fla.Frozen;
            ViewBag.flaCount = fla.Amount - fla.Balance - fla.Frozen;
            //副梁B e49e5b82-83ce-417d-8591-aa216e195099
            var flb = goodsBLL.GetModel("e49e5b82-83ce-417d-8591-aa216e195099");
            ViewBag.flb = flb.Balance + flb.Frozen;
            ViewBag.flbCount = flb.Amount - flb.Balance - flb.Frozen;
            //佛像 e540dac1-171f-4b75-a852-612de07a9550
            var fx = goodsBLL.GetModel("e540dac1-171f-4b75-a852-612de07a9550");
            ViewBag.fx = fx.Balance + fx.Frozen;
            ViewBag.fxCount = fx.Amount - fx.Balance - fx.Frozen;

            return View();
        }
        //public JsonResult getBalance(string orgid,string goodsid)
        //{
        //    StringBuilder where = new StringBuilder();
        //    where.Append("statecode=0");
        //    IDictionary<string, object> dict = new Dictionary<string, object>();
        //    string orderField = "OrderIndex";
        //    int recordCount = goodsBLL.GetRecordCount(where.ToString(), dict);
        //}
        [ShiMiaoWXValidation]
        public JsonResult GetList()
        {
            StringBuilder where = new StringBuilder();
            where.Append("statecode=0");
            IDictionary<string, object> dict = new Dictionary<string, object>();
            string orderField = "OrderIndex";
            int recordCount = goodsBLL.GetRecordCount(where.ToString(), dict);
            InitPager(recordCount);
            object list = null;
            if (recordCount > 0)
            {
                int startIndex = PageSize * (PageIndex - 1);
                IList<Model.TD_Shop_Goods> donationList = goodsBLL.GetListByPage(where.ToString(), orderField, dict, startIndex, PageSize);
                list = BuildList(donationList);
            }
            else
            {
                list = new List<Model.TD_Shop_Goods>();
            }
            return GetSucceedListResult(list, recordCount);
        }

        private object BuildList(IList<Model.TD_Shop_Goods> list)
        {
            IList<object> newList = new List<object>();
            foreach (var model in list)
            {
                newList.Add(new
                {
                    GoodsID = model.GoodsID,
                    Price = model.Price,
                    Title = model.Title,
                    Image = model.Image,
                    Amount = model.Amount,
                    Balance = model.Balance,
                    Frozen = model.Frozen,
                    Unit = model.Unit
                });
            }
            return newList;
        }
        [HttpPost]
        [ValidateInput(false)]
        [ShiMiaoWXValidation]
        public JsonResult SaveOrder2(Models.m_Shop_Order model)
        {
            if (model.Amount <= 0) throw new Exception("数量有误");
            int orgID = int.Parse(DESEncrypt.Decrypt(ViewBag.EnOrgID));
            Model.TD_Shop_Goods goods = goodsBLL.GetModel(model.goodsID);
            if (goods.Balance.Value < model.Amount)
            {
                return GetErrorResult("数量不足");
            }
            Model.TD_Shop_Order_Consignee consignee = new Model.TD_Shop_Order_Consignee()
            {
                Address = model.useraddress,
                Phone = model.userphone,
                Name = model.username
            };
            
            var member = MemberData.GetMember();
            Model.TD_Shop_Order order = new Model.TD_Shop_Order();
            order.PayType = Constants.PayType.WeiXin;
            order.MemberID = member.MemberID;
            order.HeaderImage = member.HeaderImage;
            if (!string.IsNullOrEmpty(model.username) && model.username != "输入姓名")
                order.NickName = model.username;
            else order.NickName = member.NickName;
            order.Mobile = model.userphone;
            order.Message = model.message;
            order.OrderType = (int)Constants.DonationType.Shop1;
            order.OrgID = orgID;
            order.OriPrice = goods.Price * model.Amount;
            order.RealPrice = order.OriPrice;
            order.Status = (int)Constants.OrderStatus.WaitPay;

            IList<Model.TD_Shop_OrderGoods> orderGoodsList = new List<Model.TD_Shop_OrderGoods>();
            Model.TD_Shop_OrderGoods orderGoods = new Model.TD_Shop_OrderGoods();
            orderGoods.GoodsID = goods.GoodsID;
            orderGoods.Title = goods.Title;
            orderGoods.Amount = model.Amount;
            orderGoods.OrgID = orgID;
            orderGoods.OriPrice = goods.Price;
            orderGoods.RealPrice = goods.Price;
            orderGoodsList.Add(orderGoods);
            int result = orderBLL.Save(order,consignee, orderGoodsList);
            if (result > 0)
            {
                string url = string.Empty;
                if (order.PayType == Constants.PayType.WeiXin)
                {
                    url = "/WeiXinPay/PayForShop?oid=" + ViewBag.EnOrgID + "&orderid=" + order.OrderID + "&url=" + HttpUtility.UrlEncode("/Shop/Goods2/Index?oid=" + ViewBag.EnOrgID);
                }
                return GetSucceedResult(new
                {
                    url = url
                }, "");
            }
            else
            {
                return GetErrorResult("保存失败，请稍候再试");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        [ShiMiaoWXValidation]
        public JsonResult SaveOrder(string goodsID, int? amount)
        {
            int orgID = int.Parse(DESEncrypt.Decrypt(ViewBag.EnOrgID));
            Model.TD_Shop_Goods goods = goodsBLL.GetModel(goodsID);
            if (goods.Balance.Value < amount.Value)
            {
                return GetErrorResult("数量不足");
            }
            var member = MemberData.GetMember();
            Model.TD_Shop_Order order = new Model.TD_Shop_Order();
            order.PayType = Constants.PayType.WeiXin;
            order.MemberID = member.MemberID;
            order.HeaderImage = member.HeaderImage;
            order.NickName = member.NickName;
            order.OrderType = (int)Constants.DonationType.Shop1;
            order.OrgID = orgID;
            order.OriPrice = goods.Price * (amount ?? 1);
            order.RealPrice = order.OriPrice;
            order.Status = (int)Constants.OrderStatus.WaitPay;

            IList<Model.TD_Shop_OrderGoods> orderGoodsList = new List<Model.TD_Shop_OrderGoods>();
            Model.TD_Shop_OrderGoods orderGoods = new Model.TD_Shop_OrderGoods();
            orderGoods.GoodsID = goods.GoodsID;
            orderGoods.Title = goods.Title;
            orderGoods.Amount = amount;
            orderGoods.OrgID = orgID;
            orderGoods.OriPrice = goods.Price;
            orderGoods.RealPrice = goods.Price;
            orderGoodsList.Add(orderGoods);
            int result = orderBLL.Save(order, orderGoodsList);
            if (result > 0)
            {
                string url = string.Empty;
                if (order.PayType == Constants.PayType.WeiXin)
                {
                    url = "/WeiXinPay/PayForShop?oid=" + ViewBag.EnOrgID + "&orderid=" + order.OrderID + "&url=" + HttpUtility.UrlEncode("/Shop/Goods2/Index?oid=" + ViewBag.EnOrgID);
                }
                return GetSucceedResult(new
                {
                    url = url
                }, "");
            }
            else
            {
                return GetErrorResult("保存失败，请稍候再试");
            }
        }

        [ShiMiaoWXValidation]
        [HttpPost]
        public JsonResult GetOrderList()
        {
            int orgID = int.Parse(DESEncrypt.Decrypt(ViewBag.EnOrgID));
            StringBuilder where = new StringBuilder();
            where.AppendFormat("orders.OrgID={0} and orders.OrderType={1} and orders.IsPay='1'", orgID, (int)Constants.DonationType.Shop1);
            string orderField = "orders.PayTime desc";
            IList<Model.OrderDetail> orderList = orderBLL.GetListByPage(where.ToString(), orderField, 0, 30);
            object list = BuildOrderList(orderList);
            return GetSucceedListResult(list, 30);
        }

        private object BuildOrderList(IList<Model.OrderDetail> list)
        {
            IList<object> newList = new List<object>();
            foreach (var model in list)
            {
                newList.Add(new
                {
                    OrderTime = model.OrderTime.Value.ToString("yyyy-MM-dd"),
                    NickName = DisableName(model.NickName),
                    Title = string.Format("{0}{1}份", model.Title, model.Amount),
                    RealPrice = model.RealPrice
                });
            }
            return newList;
        }

        private string DisableName(string name)
        {
            if (name.Length >= 1)
            {
                return name.Substring(0, 1) + "**";
            }
            return name;
        }
    }
}