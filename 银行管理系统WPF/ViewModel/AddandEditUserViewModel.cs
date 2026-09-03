using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.Repository;
using 银行管理系统WPF.Utilities;

namespace 银行管理系统WPF.ViewModel
{
    /// <summary>
    /// 新增 / 修改用户窗口的 ViewModel。
    /// model 传 null 表示新增；传入已有 UserInfo 表示修改（直接在该对象上编辑）。
    /// </summary>
    public class AddandEditUserViewModel : ViewModelBase
    {
        private readonly Window window;

        private readonly UserInfoRepository userRepository = new UserInfoRepository();

        /// <summary>
        /// 是否为修改模式（false = 新增）
        /// </summary>
        private readonly bool isEdit;

        public AddandEditUserViewModel(Window window, UserInfo model)
        {
            this.window = window;
            this.isEdit = model != null;
            this.EditUser = model ?? new UserInfo();
        }

        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title => isEdit ? "修改用户" : "添加用户";

        /// <summary>
        /// 正在编辑的用户对象（新增时是全新对象）
        /// </summary>
        public UserInfo EditUser { get; }

        /// <summary>
        /// 状态文本（ComboBox 用），同步到 EditUser.Status
        /// </summary>
        public string StatusText
        {
            get => EditUser.Status == 0 ? "正常" : "禁用";
            set { EditUser.Status = value == "正常" ? 0 : 1; OnPropertyChanged(); }
        }

        /// <summary>
        /// 保存：校验 -&gt; 新增 Insert / 修改 Update -&gt; 关闭窗口并返回 true
        /// </summary>
        public RelayCommand SaveCommand
        {
            get => new RelayCommand((obj) =>
            {
                if (string.IsNullOrWhiteSpace(EditUser.Account))
                {
                    MessageBox.Show("请输入账号");
                    return;
                }
                if (string.IsNullOrWhiteSpace(EditUser.Password))
                {
                    MessageBox.Show("请输入密码");
                    return;
                }

                try
                {
                    int? userId = LoginInfo.CurrentUser?.UserId;

                    bool success;
                    if (isEdit)
                    {
                        EditUser.LastUpdateTime = DateTime.Now;
                        EditUser.LastUpdateUserId = userId;
                        success = userRepository.Update(EditUser);
                    }
                    else
                    {
                        EditUser.CreateTime = DateTime.Now;
                        EditUser.CreateUserId = userId ?? 0;
                        EditUser.LastUpdateUserId = userId;
                        success = userRepository.Add(EditUser);
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
