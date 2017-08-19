using ShiMiao.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiMiao.BLL
{
    public class bl_sell_goods
    {
        private static readonly DAL.dl_Sell_Goods dal = new DAL.dl_Sell_Goods();
        public int GetRecordCount(string where, IDictionary<string, object> dict)
        {
            return dal.GetRecordCount(where, dict);
        }

        public int ClearFrozenGoods()
        {
            DateTime time = DateTime.Now.AddMinutes(-30);
            int orderStatus = (int)Constants.OrderStatus.Cancel;
            return dal.ClearFrozenGoods(time, orderStatus);
        }

        public IList<ShiMiao.Model.m_Sell_Goods> GetListByPage(string where, string orderby, IDictionary<string, object> dictParams, int startIndex, int pageSize)
        {
            return dal.GetListByPage(where, orderby, dictParams, startIndex, pageSize);
        }

        public Model.m_Sell_Goods GetModel(string goodsID)
        {
            return dal.GetModel(goodsID);
        }

        public int PayOrderNoFrozen(string orderID, IList<Model.TD_Shop_OrderGoods> orderGoodsList, string tranID)
        {
            foreach (Model.TD_Shop_OrderGoods orderGoods in orderGoodsList)
            {
                int result = dal.PayOrderNoFrozen(orderGoods.GoodsID, orderGoods.Amount.Value, tranID);
                if (result == 0)
                {
                    return 0;
                }
            }
            return 1;
        }
    }
}
