using ShiMiao.Common;
using ShiMiao.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ShiMiao.Web.Areas.Shop.Controllers
{
    [ShiMiaoWXValidation]
    public class YueBingController : MiniWebBaseController
    {
        private static readonly BLL.TD_Shop_Order orderBLL = new BLL.TD_Shop_Order();
        private static readonly BLL.bl_sell_goods sellBLL = new BLL.bl_sell_goods();
        private static readonly BLL.bl_Shop_Order_Consignee orderConsignee = new BLL.bl_Shop_Order_Consignee();
        
        // GET: Shop/YueBing
        public ActionResult Index()
        {
            // int orgID = int.Parse(DESEncrypt.Decrypt(ViewBag.EnOrgID));
            ViewBag.goodsID = BLL.bl_Config.YueBingGoodsID;//指定月饼的货物编号
            //16111c0f-7f4a-11e7-88e6-000c2943fa30
            return View();
        }
        [HttpPost]
        [ShiMiaoWXValidation]
        public JsonResult getOrderList()
        {
            int orgID = int.Parse(DESEncrypt.Decrypt(ViewBag.EnOrgID));
            StringBuilder where = new StringBuilder();
            where.Append("orders.OrgID=" + orgID + " and goods.goodsid='"+ BLL.bl_Config.YueBingGoodsID + "' and IsPay='1'");
         
            string orderField = "PayTime desc";
            var record = orderBLL.GetCount(where.ToString());
            InitPager(record?.Count, 10);
            object list = null;
            if (null != record && record.Count > 0)
            {
                int startIndex = PageSize * PageIndex - 1;
                var orderList = orderBLL.GetListByPage2("orders.OrgID=" + orgID + " and orderGoods.goodsid='" + BLL.bl_Config.YueBingGoodsID + "' and IsPay='1'", orderField, startIndex, PageSize);
                list = BuildOrderList(orderList);
                return GetSucceedListResult(list, record.Count);
            }
            else
            {
                list = new List<Model.TD_Donation_Order>();
                return GetSucceedListResult(list, 0);
            }

        }
        private string DisableName(string name)
        {
            if (name.Length >= 1)
            {
                return name.Substring(0, 1) + "**";
            }
            return name;
        }
        private object BuildOrderList(IList<Model.OrderDetailEx> list)
        {
            IList<object> newList = new List<object>();
            foreach (var model in list)
            {
                newList.Add(new
                {
                    HeaderImage = model.HeaderImage,
                    NickName = DisableName(model.NickName),
                    Amount = model.Amount
                });
            }
            return newList;
        }
        [HttpPost]
        
        public JsonResult SaveOrder(Models.m_YueBing model)
        {
            int orgID = int.Parse(DESEncrypt.Decrypt(ViewBag.EnOrgID));
            var goods = sellBLL.GetModel(model.goodsID);
            if (null == goods) throw new Exception("该商品已达上限");
            //if (goods.Balance.Value < model.Amount)
            //{
            //    return GetErrorResult("数量不足");
            //}
            ShiMiao.Model.TD_Shop_Order_Consignee consignee = new Model.TD_Shop_Order_Consignee();
            consignee.Address = model.useraddress;
            consignee.Name = model.username;
            consignee.Phone = model.userphone;

            var member = MemberData.GetMember();
            Model.TD_Shop_Order order = new Model.TD_Shop_Order();
            order.PayType = Constants.PayType.WeiXin;
            order.MemberID = member.MemberID;
            order.HeaderImage = member.HeaderImage;
            order.NickName = member.NickName;
            order.OrderType = (int)Constants.DonationType.Shop1;
            order.OrgID = orgID;
            //月饼常规价格 + 快递费
            decimal ExtraPrice = 0;
            switch(model.ExtraPrice)
            {
                case 0://自提
                    ExtraPrice = 0;
                    break;
                case 1://省内
                    ExtraPrice = 6;
                    break;
                case 2://省外
                    ExtraPrice = 8;
                    break;
                default://其他
                    ExtraPrice = 8;
                    break;
            }
            order.ExtraPrice = ExtraPrice * model.Amount;
            order.OriPrice = goods.Price * model.Amount +order.ExtraPrice;
            decimal realPrice = goods.Price.Value;
            if (model.Amount>=5)
            {//优惠价格 + 快递费
                order.RealPrice = 88 * model.Amount + ExtraPrice * model.Amount;
                realPrice = 88;
            }
            else
            {
                order.RealPrice = order.OriPrice;
            }
            order.Status = (int)Constants.OrderStatus.WaitPay;

            IList<Model.TD_Shop_OrderGoods> orderGoodsList = new List<Model.TD_Shop_OrderGoods>();
            Model.TD_Shop_OrderGoods orderGoods = new Model.TD_Shop_OrderGoods();
            orderGoods.GoodsID = goods.GoodsID;
            orderGoods.Title = goods.Title;
            orderGoods.Amount = model.Amount;
            orderGoods.OrgID = orgID;
            orderGoods.OriPrice = goods.Price;
            orderGoods.RealPrice = realPrice;//goods.Price;
            orderGoodsList.Add(orderGoods);
            int result = orderBLL.Save(order, consignee, orderGoodsList);
            if (result > 0)
            {
                string url = string.Empty;
                if (order.PayType == Constants.PayType.WeiXin)
                {
                    url = "/WeiXinPay/PayForShop?oid=" + ViewBag.EnOrgID + "&orderid=" + order.OrderID + "&url=" + HttpUtility.UrlEncode("/Shop/YueBing/Index?oid=" + ViewBag.EnOrgID);
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
    }
}