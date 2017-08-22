using MySql.Data.MySqlClient;
using ShiMiao.DBUtility;
using ShiMiao.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiMiao.DAL
{
    public class dl_BillProccess
    {
        public WXBill selectOneBillBySHDDH(WXBill _NewBill)
        {
            WXBill _bill = null;
            try
            {
                StringBuilder strSql = new StringBuilder();
                //"select ID,jysj,gzzhid,shh,zshh,sbh,wxddh,shddh,wbbh,jylx,jyzt,fkyh,hbzl,zje,qyhbje,wxtkdh,shtkdh,tkje,qyhbtkje,tklx,tkzt,spmc,shsjb,sxf,fl from " + SQL_TABLE_NAME + " where shddh = :shddh and wxddh = :wxddh and wxtkdh = :wxtkdh and shtkdh = :shtkdh";
                 strSql.Append(@" select RecordID, TranDate, AppID, MCHID, SubMCHID, DeviceID, TransactionID, OutTradeNo, UserInfo, TranType , TranSta, PayBank , Currency , Cost , EnterpriseLuckyMoney , RefundID , OutRefundNo , RefundMoney , EnterpriseRefundLuckyMoney, RefundType , RefundSta , GoodsName , MerchantData , ExFee , Rates   ");
                strSql.Append("  from TD_Order_WeiXinPay_dz ");
                strSql.Append(" where TransactionID=@TransactionID and OutTradeNo=@OutTradeNo and RefundID=@RefundID and OutRefundNo=@OutRefundNo");
                MySqlParameter[] parameters = {
                                    new MySqlParameter("@TransactionID", _NewBill.TransactionID),
                                   new MySqlParameter("@OutTradeNo", _NewBill.OutTradeNo),
                                   new MySqlParameter("@RefundID", _NewBill.RefundID) ,
                                    new MySqlParameter("@OutRefundNo", _NewBill.OutRefundNo)

                };
                using (DbDataReader dr = MySqlHelperUtil.ExecuteReader(strSql.ToString(), parameters))
                {
                    if (dr.HasRows)
                    {
                        int i = -1;
                        dr.Read();
                        //ID,jysj,gzzhid,shh,zshh,sbh,wxddh,shddh,wbbh,jylx,jyzt,fkyh,hbzl,zje,qyhbje,wxtkdh,shtkdh,tkje,qyhbtkje,tklx,tkzt,spmc,shsjb,sxf,fl
                        _bill = new WXBill();
                        if (!dr.IsDBNull(++i)) _bill.RecordID = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.TranDate = dr.GetDateTime(i);
                        if (!dr.IsDBNull(++i)) _bill.AppID = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.MCHID = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.SubMCHID = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.DeviceID = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.TransactionID = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.OutTradeNo = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.UserInfo = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.TranType = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.TranSta = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.PayBank = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.Currency = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.Cost = dr.GetDecimal(i);
                        if (!dr.IsDBNull(++i)) _bill.EnterpriseLuckyMoney = dr.GetDecimal(i);
                        if (!dr.IsDBNull(++i)) _bill.RefundID = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.OutRefundNo = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.RefundMoney = dr.GetDecimal(i);
                        if (!dr.IsDBNull(++i)) _bill.EnterpriseRefundLuckyMoney = dr.GetDecimal(i);
                        if (!dr.IsDBNull(++i)) _bill.RefundType = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.RefundSta = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.GoodsName = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.MerchantData = dr.GetString(i);
                        if (!dr.IsDBNull(++i)) _bill.ExFee = dr.GetDecimal(i);
                        if (!dr.IsDBNull(++i)) _bill.Rates = dr.GetString(i);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _bill;
        }
        public bool updateBillDZZTByID(string _recordid,int dzzt, string _transid = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TD_Order_WeiXinPay_dz set ");
            strSql.Append(" dzzt = @dzzt  ");
            strSql.Append(" where RecordID=@RecordID ");
            MySqlParameter[] parameters = {
                        new MySqlParameter("@RecordID",_recordid),
                         new MySqlParameter("@dzzt",dzzt)
            };
            if (string.IsNullOrEmpty(_transid))
                return MySqlHelperUtil.ExecuteNonQuery(strSql.ToString(), parameters) == 1;
            else return MySqlHelperUtil.ExecuteNonQuery(_transid, strSql.ToString(), parameters) == 1;
        }
        public bool updateBillByID(string _recordid, WXBill _bill, string _transid = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TD_Order_WeiXinPay_dz set ");
            strSql.Append(" TranDate = @TranDate , ");
            strSql.Append("AppID = @AppID , ");
            strSql.Append(" MCHID = @MCHID , ");
            strSql.Append("SubMCHID= @SubMCHID , ");
            strSql.Append(" DeviceID = @DeviceID , ");
            strSql.Append(" TransactionID = @TransactionID , ");
            strSql.Append(" OutTradeNo = @OutTradeNo , ");
            strSql.Append(" UserInfo = @UserInfo , ");
            strSql.Append(" TranType = @TranType , ");
            strSql.Append(" TranSta = @TranSta , ");
            strSql.Append(" PayBank = @PayBank , ");
            strSql.Append(" Currency = @Currency , ");
            strSql.Append(" Cost = @Cost , ");
            strSql.Append(" EnterpriseLuckyMoney = @EnterpriseLuckyMoney , ");
            strSql.Append(" RefundID = @RefundID , ");
            strSql.Append(" OutRefundNo = @OutRefundNo , ");
            strSql.Append(" RefundMoney = @RefundMoney , ");
            strSql.Append(" EnterpriseRefundLuckyMoney = @EnterpriseRefundLuckyMoney , ");
            strSql.Append(" RefundType = @RefundType , ");
            strSql.Append(" RefundSta = @RefundSta , ");
            strSql.Append(" GoodsName = @GoodsName , ");
            strSql.Append(" MerchantData = @MerchantData , ");
            strSql.Append(" ExFee = @ExFee , ");
            strSql.Append(" Rates = @Rates  ");
            strSql.Append(" where RecordID=@RecordID ");
            MySqlParameter[] parameters = {
                        new MySqlParameter("@RecordID",_recordid)
            };
            if (string.IsNullOrEmpty(_transid))
                return MySqlHelperUtil.ExecuteNonQuery(strSql.ToString(), parameters) == 1;
            else return MySqlHelperUtil.ExecuteNonQuery(_transid, strSql.ToString(), parameters) == 1;
        }
        public bool insertBills(List<WXBill> _BillList)
        {
            bool ret = false;
            string transid = MySqlHelperUtil.BeginTran();
            try
            {

                foreach (WXBill _bill in _BillList)
                {
                    //通过商户订单号查找记录
                    WXBill oldOne = selectOneBillBySHDDH(_bill);
                    if (null != oldOne)
                    {
                        if (!oldOne.Equals(_bill))
                        {//若存在记录且数据不一直 更新
                            ret = updateBillByID(oldOne.RecordID, _bill, transid);
                            if (false == ret)
                            {
                                break;
                            }
                        }
                        else ret = true;
                    }
                    else
                    {//不存在记录 插入
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into TD_Order_WeiXinPay_dz(");
                        strSql.Append("RecordID, TranDate, AppID, MCHID, SubMCHID, DeviceID, TransactionID, OutTradeNo, UserInfo, TranType , TranSta, PayBank , Currency , Cost , EnterpriseLuckyMoney , RefundID , OutRefundNo , RefundMoney , EnterpriseRefundLuckyMoney, RefundType , RefundSta , GoodsName , MerchantData , ExFee , Rates");
                        strSql.Append(") values (");
                        strSql.Append("@RecordID, @TranDate,@AppID, @MCHID, @SubMCHID, @DeviceID, @TransactionID, @OutTradeNo, @UserInfo, @TranType , @TranSta, @PayBank , @Currency , @Cost , @EnterpriseLuckyMoney , @RefundID , @OutRefundNo , @RefundMoney , @EnterpriseRefundLuckyMoney, @RefundType , @RefundSta , @GoodsName , @MerchantData , @ExFee , @Rates");
                        strSql.Append(") ");

                        MySqlParameter[] parameters = {
                            new MySqlParameter("@RecordID", Guid.NewGuid().ToString()),
                            new MySqlParameter("@TranDate", _bill.TranDate),
                            new MySqlParameter("@AppID", _bill.AppID),
                            new MySqlParameter("@MCHID", _bill.MCHID),
                            new MySqlParameter("@SubMCHID", _bill.SubMCHID),
                            new MySqlParameter("@DeviceID", _bill.DeviceID),
                            new MySqlParameter("@TransactionID", _bill.TransactionID),
                            new MySqlParameter("@OutTradeNo", _bill.OutTradeNo),
                            new MySqlParameter("@UserInfo", _bill.UserInfo),
                            new MySqlParameter("@TranType", _bill.TranType),
                            new MySqlParameter("@TranSta", _bill.TranSta),
                            new MySqlParameter("@PayBank", _bill.PayBank),
                            new MySqlParameter("@Currency", _bill.Currency),
                            new MySqlParameter("@Cost", _bill.Cost),
                            new MySqlParameter("@EnterpriseLuckyMoney", _bill.EnterpriseLuckyMoney),
                            new MySqlParameter("@RefundID", _bill.RefundID),
                            new MySqlParameter("@OutRefundNo", _bill.OutRefundNo),
                            new MySqlParameter("@RefundMoney", _bill.RefundMoney),
                            new MySqlParameter("@EnterpriseRefundLuckyMoney", _bill.EnterpriseRefundLuckyMoney),
                            new MySqlParameter("@RefundType", _bill.RefundType),
                            new MySqlParameter("@RefundSta", _bill.RefundSta),
                            new MySqlParameter("@GoodsName", _bill.GoodsName),
                            new MySqlParameter("@MerchantData", _bill.MerchantData),
                            new MySqlParameter("@ExFee", _bill.ExFee),
                            new MySqlParameter("@Rates", _bill.Rates)
                        };
                        if (MySqlHelperUtil.ExecuteNonQuery(transid, strSql.ToString(), parameters) != 1)
                        {
                            MySqlHelperUtil.RollbackTran(transid);
                            ret = false;
                            break;
                        }
                        else
                        {
                            ret = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MySqlHelperUtil.RollbackTran(transid);
                throw ex;
            }
            if (!ret) MySqlHelperUtil.RollbackTran(transid);
            else MySqlHelperUtil.CommitTran(transid);
            return ret;
        }
        public List<WXBill> getNotCheckYet()
        {
            List<WXBill> _billList = new List<WXBill>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select  RecordID, TranDate, AppID, MCHID, SubMCHID, DeviceID, TransactionID, OutTradeNo, UserInfo, TranType , TranSta, PayBank , Currency , Cost , EnterpriseLuckyMoney , RefundID , OutRefundNo , RefundMoney , EnterpriseRefundLuckyMoney, RefundType , RefundSta , GoodsName , MerchantData , ExFee , Rates   ");
            strSql.Append("  from TD_Order_WeiXinPay_dz ");
            strSql.Append(" where dzzt=0 ");
            MySqlParameter[] parameters = { };
            using (DbDataReader dr = MySqlHelperUtil.ExecuteReader(strSql.ToString(), parameters))
            {
                while (dr.HasRows && dr.Read())
                {
                    int i = -1;
                    //ID,jysj,gzzhid,shh,zshh,sbh,wxddh,shddh,wbbh,jylx,jyzt,fkyh,hbzl,zje,qyhbje,wxtkdh,shtkdh,tkje,qyhbtkje,tklx,tkzt,spmc,shsjb,sxf,fl
                    WXBill _bill = new WXBill();
                    if (!dr.IsDBNull(++i)) _bill.RecordID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.TranDate = dr.GetDateTime(i);
                    if (!dr.IsDBNull(++i)) _bill.AppID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.MCHID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.SubMCHID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.DeviceID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.TransactionID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.OutTradeNo = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.UserInfo = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.TranType = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.TranSta = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.PayBank = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.Currency = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.Cost = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.EnterpriseLuckyMoney = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.RefundID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.OutRefundNo = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.RefundMoney = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.EnterpriseRefundLuckyMoney = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.RefundType = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.RefundSta = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.GoodsName = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.MerchantData = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.ExFee = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.Rates = dr.GetString(i);
                    _billList.Add(_bill);
                }
            }
            if (_billList.Count <= 0) return null;
            return _billList;
        }
        public List<WXBill> getOneDay(DateTime time)
        {
            List<WXBill> _billList = new List<WXBill>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select limit 100  RecordID, TranDate, AppID, MCHID, SubMCHID, DeviceID, TransactionID, OutTradeNo, UserInfo, TranType , TranSta, PayBank , Currency , Cost , EnterpriseLuckyMoney , RefundID , OutRefundNo , RefundMoney , EnterpriseRefundLuckyMoney, RefundType , RefundSta , GoodsName , MerchantData , ExFee , Rates   ");
            strSql.Append("  from TD_Order_WeiXinPay_dz ");
            strSql.Append(" where dzzt=0 and trandate>'"+ time.ToString("yyyy-MM-dd") + "' and trandate<'"+ time.AddDays(1).ToString("yyyy-MM-dd")+"'");
            MySqlParameter[] parameters = {        };
            using (DbDataReader dr = MySqlHelperUtil.ExecuteReader(strSql.ToString(), parameters))
            {
                while (dr.HasRows && dr.Read())
                {
                    int i = -1;
                    //ID,jysj,gzzhid,shh,zshh,sbh,wxddh,shddh,wbbh,jylx,jyzt,fkyh,hbzl,zje,qyhbje,wxtkdh,shtkdh,tkje,qyhbtkje,tklx,tkzt,spmc,shsjb,sxf,fl
                    WXBill _bill = new WXBill();
                    if (!dr.IsDBNull(++i)) _bill.RecordID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.TranDate = dr.GetDateTime(i);
                    if (!dr.IsDBNull(++i)) _bill.AppID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.MCHID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.SubMCHID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.DeviceID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.TransactionID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.OutTradeNo = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.UserInfo = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.TranType = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.TranSta = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.PayBank = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.Currency = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.Cost = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.EnterpriseLuckyMoney = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.RefundID = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.OutRefundNo = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.RefundMoney = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.EnterpriseRefundLuckyMoney = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.RefundType = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.RefundSta = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.GoodsName = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.MerchantData = dr.GetString(i);
                    if (!dr.IsDBNull(++i)) _bill.ExFee = dr.GetDecimal(i);
                    if (!dr.IsDBNull(++i)) _bill.Rates = dr.GetString(i);
                    _billList.Add(_bill);
                }
            }
            if (_billList.Count <= 0) return null;
            return _billList;
        }
    }
}
