using System.Data.SqlClient;

namespace Stone.DBUtility
{
    /// <summary>
    /// SQL SERVER���ݿ����ݲ���
    /// </summary>
    public class SqlHelper :DataProvider<SqlConnection, SqlCommand, SqlDataAdapter, SqlParameter>
    {
    }
}
