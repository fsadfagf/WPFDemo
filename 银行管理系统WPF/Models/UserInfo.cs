using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using 银行管理系统WPF.Utilities;

namespace 银行管理系统WPF.Models
{
    public class UserInfo : ModelBase
    {
        private int userId;
        /// <summary>
        /// 用户编号（主键 + 自增）
        /// 必须显式标记：SqlSugar 默认只认 Id/ID，不标记会导致 Updateable 找不到主键、InSingle 失效
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int UserId
        {
            get => userId;
            set
            {
                userId = value;
                OnPropertyChanged();
            }
        }
        private string account;
        public string Account
        {
            get => account;
            set { account = value;OnPropertyChanged(); }
        
        }
        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value; OnPropertyChanged();
            }
        }

        /// <summary>
        /// 状态文本：0=正常，1=禁用，仅供界面显示，不参与数据库映射
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string StatusName => Status == 0 ? "正常" : "禁用";
    }
}
