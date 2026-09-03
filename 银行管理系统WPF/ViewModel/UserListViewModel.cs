using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.Repository;
using 银行管理系统WPF.Utilities;
using 银行管理系统WPF.View;

namespace 银行管理系统WPF.ViewModel
{
    /// <summary>
    /// 用户列表页的 ViewModel。
    /// 与 CustomerViewModel / AddressListViewModel 结构一致，但有两处不同：
    /// 1. 密码不在列表里显示，也不支持单元格直接编辑，一律走弹窗（AddandUpdateUserWindow）；
    /// 2. 因此不提供 SaveRow，DataGrid 整表只读。
    /// </summary>
    public class UserListViewModel : ViewModelBase
    {
        private readonly UserInfoRepository repository;

        public UserListViewModel(UserInfoRepository repository)
        {
            this.repository = repository;
            GetUsers();
        }

        /// <summary>
        /// 表格数据源
        /// </summary>
        private ObservableCollection<UserInfo> users = new ObservableCollection<UserInfo>();
        public ObservableCollection<UserInfo> Users
        {
            get => users;
            set { users = value; OnPropertyChanged(); }
        }

        // ============ 分页 ============

        private int pageIndex = 1;
        /// <summary>
        /// 当前页码，从 1 开始
        /// </summary>
        public int PageIndex
        {
            get => pageIndex;
            set { pageIndex = value < 1 ? 1 : value; OnPropertyChanged(); }
        }

        private int pageSize = 10;
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            get => pageSize;
            set { pageSize = value < 1 ? 10 : value; OnPropertyChanged(); }
        }

        private int totalCount;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get => totalCount;
            set { totalCount = value; OnPropertyChanged(); }
        }

        private int totalPage = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get => totalPage;
            set { totalPage = value < 1 ? 1 : value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 翻页：ucPage 的 PageChanged 事件通过 Interaction 转到这里
        /// </summary>
        public RelayCommand PageChangedCommand
        {
            get => new RelayCommand((obj) =>
            {
                // 参数可能是页码本身，也可能是 ucPage 抛出的 RoutedPropertyChangedEventArgs<int>
                if (obj is int page)
                {
                    PageIndex = page;
                }
                else if (obj is RoutedPropertyChangedEventArgs<int> args)
                {
                    PageIndex = args.NewValue;
                }
                GetUsers();
            });
        }

        // ============ 查询条件 ============

        private string searchAccount;
        /// <summary>
        /// 账号（模糊）
        /// </summary>
        public string SearchAccount
        {
            get => searchAccount;
            set { searchAccount = value; OnPropertyChanged(); }
        }

        private string statusText = "全部";
        public string StatusText
        {
            get => statusText;
            set { statusText = value; OnPropertyChanged(); }
        }

        private DateTime? startTime;
        public DateTime? StartTime
        {
            get => startTime;
            set { startTime = value; OnPropertyChanged(); }
        }

        private DateTime? endTime;
        public DateTime? EndTime
        {
            get => endTime;
            set { endTime = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 查询
        /// </summary>
        public RelayCommand QueryCommand
        {
            get => new RelayCommand((obj) =>
            {
                PageIndex = 1;   // 换条件查询要回到第一页
                GetUsers();
            });
        }

        /// <summary>
        /// 重置查询条件
        /// </summary>
        public RelayCommand ResetCommand
        {
            get => new RelayCommand((obj) =>
            {
                SearchAccount = null;
                StatusText = "全部";
                StartTime = null;
                EndTime = null;
                PageIndex = 1;
                GetUsers();
            });
        }

        /// <summary>
        /// 添加 / 修改：打开 AddandUpdateUserWindow。参数为 null 表示新增；传 UserInfo 表示修改。
        /// </summary>
        public RelayCommand AddEditCommand
        {
            get => new RelayCommand((obj) =>
            {
                var window = new AddandUpdateUserWindow(obj as UserInfo);
                window.Owner = Application.Current.MainWindow;
                if (window.ShowDialog() == true)
                {
                    GetUsers();
                }
            });
        }

        /// <summary>
        /// 启用：把被禁用（Status=1）的用户恢复成正常（Status=0）
        /// </summary>
        public RelayCommand EnableCommand
        {
            get => new RelayCommand((obj) =>
            {
                if (!(obj is UserInfo model)) return;

                if (model.Status == 0)
                {
                    MessageBox.Show("该用户已是启用状态");
                    return;
                }

                try
                {
                    model.Status = 0;
                    model.LastUpdateTime = DateTime.Now;
                    model.LastUpdateUserId = LoginInfo.CurrentUser?.UserId;
                    if (repository.Update(model))
                    {
                        MessageBox.Show("已启用");
                        GetUsers();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("启用失败：" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 删除：逻辑删除（Status 置 1）
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get => new RelayCommand((obj) =>
            {
                if (!(obj is UserInfo model)) return;
                if (model.Status == 1)
                {
                    MessageBox.Show("该用户已是禁用状态");
                    return;
                }

                var result = MessageBox.Show(
                    $"确定删除用户【{model.Account}】吗？",
                    "删除确认",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;

                try
                {
                    repository.Delete(model.UserId);
                    GetUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败：" + ex.Message);
                }
            });
        }

        private void GetUsers()
        {
            var exp = Expressionable.Create<UserInfo>();

            if (!string.IsNullOrWhiteSpace(SearchAccount))
            {
                exp.And(it => it.Account.Contains(SearchAccount));
            }
            if (StatusText == "正常")
            {
                exp.And(it => it.Status == 0);
            }
            else if (StatusText == "禁用")
            {
                exp.And(it => it.Status == 1);
            }
            if (StartTime.HasValue)
            {
                exp.And(it => it.CreateTime >= StartTime.Value.Date);
            }
            if (EndTime.HasValue)
            {
                exp.And(it => it.CreateTime <= EndTime.Value.Date.AddDays(1).AddSeconds(-1));
            }

            try
            {
                int total = 0;
                var list = repository.GetListByPage(exp, PageIndex, PageSize, ref total);

                TotalCount = total;
                TotalPage = total == 0 ? 1 : (int)Math.Ceiling(total * 1.0 / PageSize);

                // 删掉末页最后一条后页码可能越界，拉回最后一页重查
                if (PageIndex > TotalPage)
                {
                    PageIndex = TotalPage;
                    int retry = 0;
                    list = repository.GetListByPage(exp, PageIndex, PageSize, ref retry);
                }

                Users = new ObservableCollection<UserInfo>(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询用户失败：" + ex.Message);
            }
        }
    }
}
