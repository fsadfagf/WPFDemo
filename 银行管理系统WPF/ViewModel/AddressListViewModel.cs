using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WPF_BankCustomerSystem.Models;
using 银行管理系统WPF.Repository;
using 银行管理系统WPF.Utilities;
using 银行管理系统WPF.View;

namespace 银行管理系统WPF.ViewModel
{
    /// <summary>
    /// 地址列表页的 ViewModel。结构与 CustomerViewModel 保持一致：
    /// 查询条件 + 分页 + 新增/修改（弹窗）+ 启用/禁用 + 单元格自动保存。
    /// </summary>
    public class AddressListViewModel : ViewModelBase
    {
        private readonly AddressRepository repository;

        public AddressListViewModel(AddressRepository repository)
        {
            this.repository = repository;
            GetAddresses();
        }

        /// <summary>
        /// 表格数据源
        /// </summary>
        private ObservableCollection<AddressInfo> addresses = new ObservableCollection<AddressInfo>();
        public ObservableCollection<AddressInfo> Addresses
        {
            get => addresses;
            set { addresses = value; OnPropertyChanged(); }
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
                GetAddresses();
            });
        }

        // ============ 查询条件 ============

        private string searchProvince;
        /// <summary>
        /// 省份（模糊）
        /// </summary>
        public string SearchProvince
        {
            get => searchProvince;
            set { searchProvince = value; OnPropertyChanged(); }
        }

        private string searchCity;
        /// <summary>
        /// 城市（模糊）
        /// </summary>
        public string SearchCity
        {
            get => searchCity;
            set { searchCity = value; OnPropertyChanged(); }
        }

        private string searchArea;
        /// <summary>
        /// 区域（模糊）
        /// </summary>
        public string SearchArea
        {
            get => searchArea;
            set { searchArea = value; OnPropertyChanged(); }
        }

        private string searchDetail;
        /// <summary>
        /// 详细地址（模糊）
        /// </summary>
        public string SearchDetail
        {
            get => searchDetail;
            set { searchDetail = value; OnPropertyChanged(); }
        }

        private string statusText = "全部";
        public string StatusText
        {
            get => statusText;
            set { statusText = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 查询
        /// </summary>
        public RelayCommand QueryCommand
        {
            get => new RelayCommand((obj) =>
            {
                PageIndex = 1;   // 换条件查询要回到第一页
                GetAddresses();
            });
        }

        /// <summary>
        /// 重置查询条件
        /// </summary>
        public RelayCommand ResetCommand
        {
            get => new RelayCommand((obj) =>
            {
                SearchProvince = null;
                SearchCity = null;
                SearchArea = null;
                SearchDetail = null;
                StatusText = "全部";
                PageIndex = 1;
                GetAddresses();
            });
        }

        /// <summary>
        /// 添加 / 修改：打开 AddandUpdateAddressWindow。参数为 null 表示新增；传 AddressInfo 表示修改。
        /// </summary>
        public RelayCommand AddEditCommand
        {
            get => new RelayCommand((obj) =>
            {
                var window = new AddandUpdateAddressWindow(obj as AddressInfo);
                window.Owner = Application.Current.MainWindow;
                if (window.ShowDialog() == true)
                {
                    GetAddresses();
                }
            });
        }

        /// <summary>
        /// 启用：把被禁用（Status=1）的地址恢复成正常（Status=0）
        /// </summary>
        public RelayCommand EnableCommand
        {
            get => new RelayCommand((obj) =>
            {
                if (!(obj is AddressInfo model)) return;

                if (model.Status == 0)
                {
                    MessageBox.Show("该地址已是启用状态");
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
                        GetAddresses();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("启用失败：" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 单元格改完后自动入库，由页面的 DataGrid.CellEditEnding 调用
        /// </summary>
        public void SaveRow(AddressInfo model)
        {
            if (model == null) return;

            try
            {
                model.LastUpdateTime = DateTime.Now;
                model.LastUpdateUserId = LoginInfo.CurrentUser?.UserId;
                repository.Update(model);
            }
            catch (Exception ex)
            {
                MessageBox.Show("自动保存失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 删除：逻辑删除（Status 置 1）
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get => new RelayCommand((obj) =>
            {
                if (!(obj is AddressInfo model)) return;
                if (model.Status == 1)
                {
                    MessageBox.Show("该地址已是禁用状态");
                    return;
                }

                var result = MessageBox.Show(
                    $"确定删除地址【{model.FullAddress}】吗？",
                    "删除确认",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;

                try
                {
                    repository.Delete(model.AddressId);
                    GetAddresses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败：" + ex.Message);
                }
            });
        }

        private void GetAddresses()
        {
            var exp = Expressionable.Create<AddressInfo>();

            if (!string.IsNullOrWhiteSpace(SearchProvince))
            {
                exp.And(it => it.ProvinceName.Contains(SearchProvince));
            }
            if (!string.IsNullOrWhiteSpace(SearchCity))
            {
                exp.And(it => it.City.Contains(SearchCity));
            }
            if (!string.IsNullOrWhiteSpace(SearchArea))
            {
                exp.And(it => it.Area.Contains(SearchArea));
            }
            if (!string.IsNullOrWhiteSpace(SearchDetail))
            {
                exp.And(it => it.DetailAddress.Contains(SearchDetail));
            }
            if (StatusText == "正常")
            {
                exp.And(it => it.Status == 0);
            }
            else if (StatusText == "禁用")
            {
                exp.And(it => it.Status == 1);
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

                Addresses = new ObservableCollection<AddressInfo>(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询地址失败：" + ex.Message);
            }
        }
    }
}
