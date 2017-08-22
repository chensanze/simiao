using MySql.Data.MySqlClient;
using ShiMiao.DBUtility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiMiao.DAL
{
    public class dl_Sell_Goods
    {
        public int GetRecordCount(string strWhere, IDictionary<string, object> dictParams)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from td_sell_goods");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where " + strWhere);
            }
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (dictParams != null)
            {
                foreach (var pair in dictParams)
                {
                    MySqlParameter parameter = new MySqlParameter("@" + pair.Key, pair.Value);
                    parameters.Add(parameter);
                }
            }
            object result = MySqlHelperUtil.ExecuteScalar(strSql.ToString(), parameters.ToArray());
            parameters.Clear();
            return int.Parse(result.ToString());
        }

        public int ClearFrozenGoods(DateTime time, int orderStatus)
        {
            string sql = @"UPDATE td_shop_order as orders
                                    inner join td_shop_ordergoods as ordergoods on orders.OrderID=ordergoods.OrderID
                                    inner join td_sell_goods as goods on ordergoods.GoodsID=goods.goodsid
                                    set orders.Status=@OrderStatus, goods.balance=goods.balance+ordergoods.Amount,goods.frozen=goods.frozen-ordergoods.Amount
                                    where orders.IsPay='0' and orders.OrderTime<@time and goods.frozen>ordergoods.Amount";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Time", time),
                new MySqlParameter("@OrderStatus", orderStatus)
            };
            return MySqlHelperUtil.ExecuteNonQuery(sql, parameters);
        }

        public IList<ShiMiao.Model.m_Sell_Goods> GetListByPage(string where, string orderby, IDictionary<string, object> dictParams, int startIndex, int pageSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT GoodsID, OrgID, Title, Content, Amount, Balance, Price, CreateTime, OrderIndex, StateCode,Frozen,isSell FROM TD_Sell_Goods");
            if (!string.IsNullOrEmpty(where))
            {
                strSql.Append(" WHERE " + where);
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                strSql.AppendFormat(" order by {0}", orderby);
            }
            strSql.AppendFormat(" limit {0}, {1}", startIndex, pageSize);
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (dictParams != null)
            {
                foreach (var pair in dictParams)
                {
                    MySqlParameter parameter = new MySqlParameter("@" + pair.Key, pair.Value);
                    parameters.Add(parameter);
                }
            }
            IList<ShiMiao.Model.m_Sell_Goods> list = new List<ShiMiao.Model.m_Sell_Goods>();
            using (DbDataReader dr = MySqlHelperUtil.ExecuteReader(strSql.ToString(), parameters.ToArray()))
            {
                while (dr.Read())
                {
                    ShiMiao.Model.m_Sell_Goods model = new ShiMiao.Model.m_Sell_Goods();
                    int i = -1;
                    if (dr[++i] != DBNull.Value) model.GoodsID = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.OrgID = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Title = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.Content = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.Amount = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Balance = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Price = dr.GetDecimal(i);
                    if (dr[++i] != DBNull.Value) model.CreateTime = dr.GetDateTime(i);
                    if (dr[++i] != DBNull.Value) model.OrderIndex = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.StateCode = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Frozen = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.isSell = dr.GetInt32(i);

                    list.Add(model);
                }
                parameters.Clear();
                return list;
            }
        }
        public int PayOrderNoFrozen(string goodsID, int amount, string tranID)
        {
            string sql = "UPDATE td_sel_goods SET Amount=Amount-@Amount WHERE GoodsID=@GoodsID";
            MySqlParameter[] parameters = {
                                    new MySqlParameter("@GoodsID", goodsID),
                                    new MySqlParameter("@Amount", amount)
                        };
            return MySqlHelperUtil.ExecuteNonQuery(tranID, sql, parameters);
        }
        public int PayOrder(string goodsID, int amount, string tranID)
        {
            string sql = "UPDATE td_sel_goods SET Frozen=Frozen-@Amount WHERE GoodsID=@GoodsID";
            MySqlParameter[] parameters = {
                                    new MySqlParameter("@GoodsID", goodsID),
                                    new MySqlParameter("@Amount", amount)
                        };
            return MySqlHelperUtil.ExecuteNonQuery(tranID, sql, parameters);
        }

        public int Frozen(string goodsID, int amount, string tranID)
        {
            string sql = "UPDATE TD_Sell_Goods SET Balance=Balance-@Amount,Frozen=Frozen+@Amount WHERE GoodsID=@GoodsID and Balance>@Amount";
            MySqlParameter[] parameters = {
                                    new MySqlParameter("@GoodsID", goodsID),
                                    new MySqlParameter("@Amount", amount)
                        };
            return MySqlHelperUtil.ExecuteNonQuery(tranID, sql, parameters);
        }

        public ShiMiao.Model.m_Sell_Goods GetModel(string goodsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsID, OrgID, Title, Content, Amount, Balance, Price, CreateTime, OrderIndex, StateCode,Frozen,isSell  ");
            strSql.Append("  from TD_Sell_Goods ");
            strSql.Append(" where GoodsID=@GoodsID and isSell=1");
            MySqlParameter[] parameters = {
                                    new MySqlParameter("@GoodsID", goodsID)

                        };

            ShiMiao.Model.m_Sell_Goods model = null;
            using (DbDataReader dr = MySqlHelperUtil.ExecuteReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model = new ShiMiao.Model.m_Sell_Goods();
                    int i = -1;
                    if (dr[++i] != DBNull.Value) model.GoodsID = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.OrgID = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Title = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.Content = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.Amount = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Balance = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Price = dr.GetDecimal(i);
                    if (dr[++i] != DBNull.Value) model.CreateTime = dr.GetDateTime(i);
                    if (dr[++i] != DBNull.Value) model.OrderIndex = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.StateCode = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Frozen = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.isSell = dr.GetInt32(i);
                }
                return model;
            }
        }
        public List<ShiMiao.Model.m_Sell_Goods> getAllModel()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsID, OrgID, Title, Content, Amount, Balance, Price, CreateTime, OrderIndex, StateCode,Frozen,isSell  ");
            strSql.Append("  from TD_Sell_Goods ");
            strSql.Append(" where isSell=@isSell");
            MySqlParameter[] parameters = {
                                    new MySqlParameter("@isSell", 1)

                        };
            List<ShiMiao.Model.m_Sell_Goods> _list = new List<ShiMiao.Model.m_Sell_Goods>();
            
            using (DbDataReader dr = MySqlHelperUtil.ExecuteReader(strSql.ToString(), parameters))
            {
                while (dr.Read())
                {
                    ShiMiao.Model.m_Sell_Goods model = new ShiMiao.Model.m_Sell_Goods();
                    int i = -1;
                    if (dr[++i] != DBNull.Value) model.GoodsID = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.OrgID = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Title = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.Content = dr.GetString(i);
                    if (dr[++i] != DBNull.Value) model.Amount = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Balance = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Price = dr.GetDecimal(i);
                    if (dr[++i] != DBNull.Value) model.CreateTime = dr.GetDateTime(i);
                    if (dr[++i] != DBNull.Value) model.OrderIndex = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.StateCode = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.Frozen = dr.GetInt32(i);
                    if (dr[++i] != DBNull.Value) model.isSell = dr.GetInt32(i);
                    _list.Add(model);
                }
                return _list;
            }
        }
    }
}
