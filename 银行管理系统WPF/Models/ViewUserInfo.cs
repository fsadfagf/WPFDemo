using SqlSugar;
using 银行管理系统WPF.Models;

namespace WPF_BankCustomerSystem.Models.DTO
{
    [SugarTable("V_UserInfo")]
    public class ViewUserInfo : UserInfo
    {
        private string createUserName;
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName
        {
            get { return createUserName; }
            set
            {
                if (createUserName != value)
                {
                    createUserName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string lastUpdateUserName;
        /// <summary>
        /// 最后一次修改人
        /// </summary>
        public string LastUpdateUserName
        {
            get { return lastUpdateUserName; }
            set
            {
                if (lastUpdateUserName != value)
                {
                    lastUpdateUserName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string statusName;
        /// <summary>
        /// 状态（从 V_UserInfo 视图取到的中文字段，覆盖基类的计算属性）
        /// </summary>
        public new string StatusName
        {
            get { return statusName; }
            set
            {
                if (statusName != value)
                {
                    statusName = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
