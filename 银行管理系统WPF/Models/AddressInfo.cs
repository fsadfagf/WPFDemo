using SqlSugar;

using 银行管理系统WPF.Utilities;

namespace WPF_BankCustomerSystem.Models
{
    /// <summary>
    /// 银行客户地址实体类，需要支持通知
    /// </summary>
    public class AddressInfo : ModelBase
    {
        private int addressId;
        /// <summary>
        /// 地址编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int AddressId
        {
            get { return addressId; }
            set
            {
                if (addressId != value)
                {
                    addressId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string provinceName;
        /// <summary>
        /// 省份
        /// </summary>
        public string ProvinceName
        {
            get { return provinceName; }
            set
            {
                if (provinceName != value)
                {
                    provinceName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string city;
        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string area;
        /// <summary>
        /// 区域
        /// </summary>
        public string Area
        {
            get { return area; }
            set
            {
                if (area != value)
                {
                    area = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 完整地址，仅供界面显示（如下拉框），不参与数据库映射
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string FullAddress => ProvinceName + City + Area + DetailAddress;

        /// <summary>
        /// 状态文本：0=正常，1=禁用，仅供界面显示，不参与数据库映射
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string StatusName => Status == 0 ? "正常" : "禁用";

        private string detailAddress;
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress
        {
            get { return detailAddress; }
            set
            {
                if (detailAddress != value)
                {
                    detailAddress = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
