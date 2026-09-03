using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 银行管理系统WPF.Utilities;

namespace 银行管理系统WPF.Models
{
    /// <summary>
    /// 客户表（对应数据库 CustomerInfo 表）
    /// </summary>
    [SugarTable("CustomerInfo")]
    public class CustomerInfo : ModelBase
    {
        private int customerId;
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int CustomerId
        {
            get => customerId;
            set { customerId = value; OnPropertyChanged(); }
        }

        private string customerName;
        public string CustomerName
        {
            get => customerName;
            set { customerName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 性别：数据库 bit，true=男，false=女
        /// </summary>
        private bool sex;
        public bool Sex
        {
            get => sex;
            set { sex = value; OnPropertyChanged(); }
        }

        private int age;
        public int Age
        {
            get => age;
            set { age = value; OnPropertyChanged(); }
        }

        private string phone;
        public string Phone
        {
            get => phone;
            set { phone = value; OnPropertyChanged(); }
        }

        private int? addressId;
        public int? AddressId
        {
            get => addressId;
            set { addressId = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 性别文本，仅供界面显示，不参与数据库映射
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string SexName => Sex ? "男" : "女";

        /// <summary>
        /// 状态文本：0=正常，1=禁用
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string StatusName => Status == 0 ? "正常" : "禁用";
    }
}
