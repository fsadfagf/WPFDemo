using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 银行管理系统WPF.Utilities
{
    public class SqlSugarHelper
    {
        private static ConnectionConfig connectionConfig = new ConnectionConfig()
        {
            ConnectionString = "server=.,64273;database=BankSystem;uid=sa;pwd=123456",
            DbType = DbType.SqlServer,
            IsAutoCloseConnection = true
        };
        public static SqlSugarScope Db=new SqlSugarScope(connectionConfig);



    }
}
