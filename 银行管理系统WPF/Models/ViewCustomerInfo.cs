using SqlSugar;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.Utilities;

namespace WPF_BankCustomerSystem.Models.DTO
{
    [SugarTable("V_CustomerInfo")]
    public class ViewCustomerInfo :CustomerInfo
    {
        private string sexName;
        /// <summary>
        /// 性别（从 V_CustomerInfo 视图取到的中文字段，覆盖基类的计算属性）
        /// </summary>
        public new string SexName
        {
            get { return sexName; }
            set
            {
                if (sexName != value)
                {
                    sexName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int age;
        /// <summary>
        /// 年龄（从 V_CustomerInfo 视图读取，覆盖基类字段）
        /// </summary>
        public new int Age
        {
            get { return age; }
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged();
                }
            }
        }

        private string fullAddress;
        /// <summary>
        /// 完整的客户地址
        /// </summary>
        public string FullAddress
        {
            get { return fullAddress; }
            set
            {
                if (fullAddress != value)
                {
                    fullAddress = value;
                    OnPropertyChanged();
                }
            }
        }

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
        /// 状态（从 V_CustomerInfo 视图取到的中文字段，覆盖基类的计算属性）
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
