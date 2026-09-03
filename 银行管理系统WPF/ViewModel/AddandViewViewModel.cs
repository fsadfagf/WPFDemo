using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_BankCustomerSystem.Models;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.Repository;
using 银行管理系统WPF.Utilities;

namespace 银行管理系统WPF.ViewModel
{
    /// <summary>
    /// 添加 / 修改客户窗口的 ViewModel。
    /// model 传 null 表示新增；传入已有 CustomerInfo 表示修改（直接在该对象上编辑）。
    /// </summary>
    public class AddandEidtViewModel : ViewModelBase
    {
        private readonly Window window;
        
        private readonly CustomerRepository customerRepository = new CustomerRepository();//封装的对数据的操作
        private readonly AddressRepository addressRepository = new AddressRepository();

        /// <summary>
        /// 是否为修改模式（false = 新增）
        /// </summary>
        private readonly bool isEdit;

        public AddandEidtViewModel(Window window, CustomerInfo model)
        {
            this.window = window;
            this.isEdit = model != null;//判断模式
            this.EditCustomer = model ?? new CustomerInfo { Sex = true };

            try
            {
                AddressList = new ObservableCollection<AddressInfo>(addressRepository.GetAll());
                // 修改模式下，让下拉框选中该客户当前的地址
                if (isEdit && EditCustomer.AddressId.HasValue)
                {
                    SelectedAddress = AddressList.FirstOrDefault(a => a.AddressId == EditCustomer.AddressId.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载地址列表失败：" + ex.Message);
                AddressList = new ObservableCollection<AddressInfo>();
            }
        }

        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title => isEdit ? "修改客户" : "添加客户";

        /// <summary>
        /// 正在编辑的客户对象（新增时是全新对象）
        /// </summary>
        public CustomerInfo EditCustomer { get; }

        /// <summary>
        /// 地址下拉数据源
        /// </summary>
        private ObservableCollection<AddressInfo> addressList = new ObservableCollection<AddressInfo>();
        public ObservableCollection<AddressInfo> AddressList
        {
            get => addressList;
            set { addressList = value; OnPropertyChanged(); }
        }

        private AddressInfo selectedAddress;
        /// <summary>
        /// 下拉框选中的地址，保存时写回 EditCustomer.AddressId
        /// </summary>
        public AddressInfo SelectedAddress
        {
            get => selectedAddress;
            set { selectedAddress = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 性别文本（ComboBox 用），同步到 EditCustomer.Sex
        /// </summary>
        public string SexText
        {
            get => EditCustomer.Sex ? "男" : "女";
            set { EditCustomer.Sex = value == "男"; OnPropertyChanged(); }
        }

        /// <summary>
        /// 状态文本（ComboBox 用），同步到 EditCustomer.Status
        /// </summary>
        public string StatusText
        {
            get => EditCustomer.Status == 0 ? "正常" : "禁用";
            set { EditCustomer.Status = value == "正常" ? 0 : 1; OnPropertyChanged(); }
        }

        /// <summary>
        /// 保存：校验 -> 新增 Insert / 修改 Update -> 关闭窗口并返回 true
        /// </summary>
        
        public RelayCommand SaveCommand
        {
            get => new RelayCommand((obj) =>
            {
                if (string.IsNullOrWhiteSpace(EditCustomer.CustomerName))
                {
                    MessageBox.Show("请输入客户姓名");
                    return;
                }
                if (string.IsNullOrWhiteSpace(EditCustomer.Phone))
                {
                    MessageBox.Show("请输入手机号");
                    return;
                }
                // 与数据库 CHECK 约束 CK_CustomerInfo_Phone 保持一致：手机号必须 11 位
                if (EditCustomer.Phone.Trim().Length != 11)
                {
                    MessageBox.Show("手机号必须是 11 位");
                    return;
                }
                if (SelectedAddress == null)
                {
                    MessageBox.Show("请选择地址");
                    return;
                }
                // 与数据库 CHECK 约束 CK_CustomerInfo_Age 保持一致：18~65
                if (EditCustomer.Age < 18 || EditCustomer.Age > 65)
                {
                    MessageBox.Show("年龄必须在 18 到 65 岁之间");
                    return;
                }

                try
                {
                    EditCustomer.AddressId = SelectedAddress.AddressId;
                    int? userId = LoginInfo.CurrentUser?.UserId;

                    bool success;
                    if (isEdit)
                    {
                        EditCustomer.LastUpdateTime = DateTime.Now;
                        EditCustomer.LastUpdateUserId = userId;
                        success = customerRepository.Update(EditCustomer);
                    }
                    else
                    {
                        EditCustomer.CreateTime = DateTime.Now;
                        EditCustomer.CreateUserId = userId ?? 0;
                        EditCustomer.LastUpdateUserId = userId;
                        success = customerRepository.Add(EditCustomer);
                    }

                    if (success)
                    {
                        // DialogResult = true 会关闭窗口，调用方据此刷新列表
                        window.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show(isEdit ? "修改失败" : "添加失败");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存失败：" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 取消：直接关闭窗口，返回 false
        /// </summary>
        public RelayCommand CancelCommand
        {
            get => new RelayCommand((obj) =>
            {
                window.DialogResult = false;
            });
        }
    }
}
