using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace Stone.DBUtility
{
    public class DataProvider<TConnnection, TCommand, TDataAdapter, TParameter>
        where TConnnection : DbConnection, new()
        where TCommand : DbCommand, new()
        where TDataAdapter : DbDataAdapter, new()
        where TParameter : DbParameter, new()
    {
        #region �������ӵĻ��洦��
        /// <summary>
        /// ������󻺴�
        /// </summary>
        static Hashtable tranCache = new Hashtable();
        /// <summary>
        /// ���Ӷ��󻺴�
        /// </summary>
        static Hashtable connCache = new Hashtable();
        /// <summary>
        /// ����һ�����񣬷��ؿ������������ ����ID
        /// </summary>
        /// <returns>����ID</returns>
        public static string BeginTran(string connectionString)
        {
            string tranId = Guid.NewGuid().ToString();
            DbTransaction tran = OpenTransaction(connectionString);
            tranCache.Add(
                tranId,
                tran
                );
            connCache.Add(
                tranId,
                tran.Connection
                );
            return tranId;
        }
        public static string BeginTran()
        {
            return BeginTran(Cfg.Instance.ConnectionString);
        }
        static DbTransaction OpenTransaction(string connectionString)
        {
            TConnnection cn = new TConnnection();
            cn.ConnectionString = connectionString;
            cn.Open();
            return cn.BeginTransaction();
        }
        /// <summary>
        /// �ύָ�� ����ID ������
        /// </summary>
        /// <param name="tranId"></param>
        public static void CommitTran(string tranId)
        {
            DbTransaction tran = tranCache[tranId] as DbTransaction;
            if (tran == null)
                throw new ApplicationException("ָ�����񲻴���");
            tran.Commit();
            TConnnection conn = connCache[tranId] as TConnnection;
            if (conn != null
                && ConnectionState.Open == conn.State)
            {
                conn.Close();
            }
        }
        /// <summary>
        /// �ع�ָ�� ����ID ������
        /// </summary>
        /// <param name="tranId"></param>
        public static void RollbackTran(string tranId)
        {
            DbTransaction tran = tranCache[tranId] as DbTransaction;
            if (tran == null)
                throw new ApplicationException("ָ�����񲻴���");
            tran.Rollback();
            TConnnection conn = connCache[tranId] as TConnnection;
            if (conn != null
                && ConnectionState.Open == conn.State)
            {
                conn.Close();
            }
        }
        /// <summary>
        /// �ر�ָ�� ����ID ���������õ����Ӷ���
        /// </summary>
        /// <param name="tranId"></param>
        public static void CloseConn(string tranId)
        {
            DbTransaction tran = tranCache[tranId] as DbTransaction;
            if (tran == null)
                throw new ApplicationException("ָ�����񲻴���");
            TConnnection conn = connCache[tranId] as TConnnection;
            if (conn == null)
                throw new ApplicationException("ָ�����Ӳ�����");
            conn.Close();

            tranCache.Remove(tranId);
            connCache.Remove(tranId);
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string tranID, string cmdText, params TParameter[] cmdParms)
        {
            return ExecuteNonQuery(null, tranID, cmdText, CommandType.Text, cmdParms);
        }
        public static int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params TParameter[] cmdParms)
        {
            return ExecuteNonQuery(connectionString, null, cmdText, cmdType, cmdParms);
        }
        public static int ExecuteNonQuery(string cmdText, params TParameter[] cmdParms)
        {
            return ExecuteNonQuery(Cfg.Instance.ConnectionString, null, cmdText, CommandType.Text, cmdParms);
        }
        /// <summary>
        /// ִ��һ��sql��������������ݿ���Ӱ��������
        /// </summary>
        /// <param name="tranID">����ID</param>
        /// <param name="cmdText">�����ı�</param>
        /// <param name="cmdType">��������</param>        
        /// <param name="cmdParms">�����б�</param>
        /// <returns></returns>
        private static int ExecuteNonQuery(string connectionString, string tranID, string cmdText, CommandType cmdType, params TParameter[] cmdParms)
        {
            if (tranID == null)
            {
                return DataHelper<TConnnection, TCommand, TDataAdapter, TParameter>.ExecuteNonQuery(
                    connectionString,
                    cmdText,
                    cmdType,
                    cmdParms
                );
            }
            else
            {
                return DataHelper<TConnnection, TCommand, TDataAdapter, TParameter>.ExecuteNonQuery(
                    tranCache[tranID] as DbTransaction,
                    cmdText,
                    cmdType,
                    cmdParms
               );
            }
        }
        #endregion

        #region ExecuteReader
        public static DbDataReader ExecuteReader(string tranID, string cmdText, params TParameter[] cmdParms)
        {
            return ExecuteReader(null, tranID, cmdText, CommandType.Text, cmdParms);
        }
        public static DbDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params TParameter[] cmdParms)
        {
            return ExecuteReader(connectionString, null, cmdText, cmdType, cmdParms);
        }
        public static DbDataReader ExecuteReader(string cmdText, params TParameter[] cmdParms)
        {
            return ExecuteReader(Cfg.Instance.ConnectionString, null, cmdText, CommandType.Text, cmdParms);
        }
        /// <summary>
        /// ִ��һ��sql��ѯ�������DataReader����
        /// </summary>
        /// <param name="tranID">����ID</param>
        /// <param name="cmdText">�����ı�</param>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdParms">�����б�</param>
        /// <returns></returns>
        private static DbDataReader ExecuteReader(string connectionString, string tranID, string cmdText, CommandType cmdType, params TParameter[] cmdParms)
        {
            if (tranID == null)
            {
                return DataHelper<TConnnection, TCommand, TDataAdapter, TParameter>.ExecuteReader(
                    connectionString,
                    cmdText,
                    cmdType,
                    cmdParms
                );
            }
            else
            {
                return DataHelper<TConnnection, TCommand, TDataAdapter, TParameter>.ExecuteReader(
                    tranCache[tranID] as DbTransaction,
                    cmdText,
                    cmdType,
                    cmdParms
               );
            }
        }
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(string tranID, string cmdText, params TParameter[] cmdParms)
        {
            return ExecuteScalar(null, tranID, cmdText, CommandType.Text, cmdParms);
        }
        public static object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params TParameter[] cmdParms)
        {
            return ExecuteScalar(connectionString, null, cmdText, cmdType, cmdParms);
        }
        public static object ExecuteScalar(string cmdText, params TParameter[] cmdParms)
        {
            return ExecuteScalar(Cfg.Instance.ConnectionString, null, cmdText, CommandType.Text, cmdParms);
        }
        /// <summary>
        /// ִ��һ��sql��ѯ������ز�ѯ����ĵ�һ�е�һ��
        /// </summary>
        /// <param name="tranID">����ID</param>
        /// <param name="cmdText">�����ı�</param>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdParms">�����б�</param>
        /// <returns></returns>
        private static object ExecuteScalar(string connectionString, string tranID, string cmdText, CommandType cmdType, params TParameter[] cmdParms)
        {
            if (tranID == null)
            {
                return DataHelper<TConnnection, TCommand, TDataAdapter, TParameter>.ExecuteScalar(
                    connectionString,
                    cmdText,
                    cmdType,
                    cmdParms
                );
            }
            else
            {
                return DataHelper<TConnnection, TCommand, TDataAdapter, TParameter>.ExecuteScalar(
                    tranCache[tranID] as DbTransaction,
                    cmdText,
                    cmdType,
                    cmdParms
               );
            }
        }
        #endregion

        #region FillDataTable
        public static void FillDataTable(string tranID, string cmdText, DataTable dt, params TParameter[] cmdParms)
        {
            FillDataTable(null, tranID, cmdText, dt, CommandType.Text, cmdParms);
        }
        public static void FillDataTable(string connectionString, string cmdText, DataTable dt, CommandType cmdType, params TParameter[] cmdParms)
        {
            FillDataTable(connectionString, null, cmdText, dt, cmdType, cmdParms);
        }
        public static void FillDataTable(string cmdText, DataTable dt, params TParameter[] cmdParms)
        {
            FillDataTable(Cfg.Instance.ConnectionString, null, cmdText, dt, CommandType.Text, cmdParms);
        }
        /// <summary>
        /// ���һ����
        /// </summary>
        /// <param name="tranID"></param>
        /// <param name="cmdText"></param>
        /// <param name="dt"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParms"></param>
        private static void FillDataTable(string connectionString, string tranID, string cmdText, DataTable dt, CommandType cmdType, params TParameter[] cmdParms)
        {
            if (tranID == null)
            {
                DataHelper<TConnnection, TCommand, TDataAdapter, TParameter>.FillDataTable(
                    connectionString,
                    cmdText,
                    cmdType,
                    dt,
                    cmdParms
                );
            }
            else
            {
                DataHelper<TConnnection, TCommand, TDataAdapter, TParameter>.FillDataTable(
                    tranCache[tranID] as DbTransaction,
                    cmdText,
                    cmdType,
                    dt,
                    cmdParms
                );
            }
        }
        #endregion
    }
}
