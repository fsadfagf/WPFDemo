using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_BankCustomerSystem.Models;
using 银行管理系统WPF.Repository;
using 银行管理系统WPF.Utilities;

namespace 银行管理系统WPF.ViewModel
{
    /// <summary>
    /// 新增 / 修改地址窗口的 ViewModel。
    /// model 传 null 表示新增；传入已有 AddressInfo 表示修改（直接在该对象上编辑）。
    /// </summary>
    public class AddandEditAddressViewModel : ViewModelBase
    {
        private readonly Window window;

        private readonly AddressRepository addressRepository = new AddressRepository();

        /// <summary>
        /// 是否为修改模式（false = 新增）
        /// </summary>
        private readonly bool isEdit;

        public AddandEditAddressViewModel(Window window, AddressInfo model)
        {
            this.window = window;
            this.isEdit = model != null;
            this.EditAddress = model ?? new AddressInfo();
        }

        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title => isEdit ? "修改地址" : "添加地址";

        /// <summary>
        /// 正在编辑的地址对象（新增时是全新对象）
        /// </summary>
        public AddressInfo EditAddress { get; }

        /// <summary>
        /// 状态文本（ComboBox 用），同步到 EditAddress.Status
        /// </summary>
        public string StatusText
        {
            get => EditAddress.Status == 0 ? "正常" : "禁用";
            set { EditAddress.Status = value == "正常" ? 0 : 1; OnPropertyChanged(); }
        }

        /// <summary>
        /// 保存：校验 -&gt; 新增 Insert / 修改 Update -&gt; 关闭窗口并返回 true
        /// </summary>
        public RelayCommand SaveCommand
        {
            get => new RelayCommand((obj) =>
            {
                if (string.IsNullOrWhiteSpace(EditAddress.ProvinceName))
                {
                    MessageBox.Show("请输入省份");
                    return;
                }
                if (string.IsNullOrWhiteSpace(EditAddress.City))
                {
                    MessageBox.Show("请输入城市");
                    return;
                }
                if (string.IsNullOrWhiteSpace(EditAddress.Area))
                {
                    MessageBox.Show("请输入区域");
                    return;
                }
                if (string.IsNullOrWhiteSpace(EditAddress.DetailAddress))
                {
                    MessageBox.Show("请输入详细地址");
                    return;
                }

                try
                {
                    int? userId = LoginInfo.CurrentUser?.UserId;

                    bool success;
                    if (isEdit)
                    {
                        EditAddress.LastUpdateTime = DateTime.Now;
                        EditAddress.LastUpdateUserId = userId;
                        success = addressRepository.Update(EditAddress);
                    }
                    else
                    {
                        EditAddress.CreateTime = DateTime.Now;
                        EditAddress.CreateUserId = userId ?? 0;
                        EditAddress.LastUpdateUserId = userId;
                        success = addressRepository.Add(EditAddress);
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
