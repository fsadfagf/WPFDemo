using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 银行管理系统WPF.Utilities
{
    public class ModelBase:ViewModelBase
    {
        private int createUserId = 0;
        public int CreateUserId 
        {
            get => createUserId;
            set { createUserId = value;OnPropertyChanged(); }
        }

        private DateTime createTime = new DateTime();
        public  DateTime CreateTime
        {
            get => createTime;
            set { createTime = value;OnPropertyChanged() ; }
        }

        /// <summary>
        /// 最后修改时间：数据库里该列允许为 NULL，所以用可空类型。
        /// 若用非空 DateTime，NULL 会变成 0001-01-01，更新时写回 SQL Server 会报 datetime 溢出。
        /// </summary>
        private DateTime? lastUpdateTime;
        public DateTime? LastUpdateTime
        {
            get => lastUpdateTime;
            set { lastUpdateTime = value; OnPropertyChanged(); }
        }

        private int? lastUpdateUserId = 0;
        public int? LastUpdateUserId
        {
            get => lastUpdateUserId;
            set { lastUpdateUserId = value; OnPropertyChanged(); }
        }

        private int status = 0;
        public int Status
        { 
        get=>status; set { status = value;OnPropertyChanged();  }
        
        }

    }
}
