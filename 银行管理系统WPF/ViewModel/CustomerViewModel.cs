using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.Repository;
using 银行管理系统WPF.Utilities;
using 银行管理系统WPF.View;

namespace 银行管理系统WPF.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly CustomerRepository repository;

        public CustomerViewModel(CustomerRepository repository)
        {
            this.repository = repository;
            GetCustomers();
        }

        /// <summary>
        /// 表格数据源
        /// </summary>
        private ObservableCollection<CustomerInfo> customers = new ObservableCollection<CustomerInfo>();
        public ObservableCollection<CustomerInfo> Customers
        {
            get => customers;
            set { customers = value; OnPropertyChanged(); }
        }

        // ============ 分页 ============

        private int pageIndex = 1;
        /// <summary>
        /// 当前页码，从 1 开始（SqlSugar 的 ToPageList 也是 1 起始）
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
        /// 翻页：ucPage 的 PageChanged 事件通过 Interaction 转到这里，参数是新页码
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
                GetCustomers();
            });
        }

        // ============ 查询条件 ============

        private string searchName;
        public string SearchName
        {
            get => searchName;
            set { searchName = value; OnPropertyChanged(); }
        }

        private string sexText = "全部";
        public string SexText
        {
            get => sexText;
            set { sexText = value; OnPropertyChanged(); }
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
        /// 查询：按条件拼表达式后查库
        /// </summary>
        public RelayCommand QueryCommand
        {
            get => new RelayCommand((obj) =>
            {
                PageIndex = 1;   // 换条件查询要回到第一页
                GetCustomers();
            });
        }

        /// <summary>
        /// 添加 / 修改：打开 AddandUpdateWindow。参数为 null 表示新增；传 CustomerInfo 表示修改。
        /// </summary>
        //封装修改和添加的逻辑，并打开对应窗体
        public RelayCommand AddEditCommand
        {
            get => new RelayCommand((obj) =>
            {
                var window = new AddandUpdateWindow(obj as CustomerInfo);
                window.Owner = Application.Current.MainWindow;
                if (window.ShowDialog() == true)
                {
                    GetCustomers();
                }
            });
        }

        /// <summary>
        /// 启用：把被删除（Status=1）的客户恢复成正常（Status=0）
        /// </summary>
        public RelayCommand EnableCommand
        {
            get => new RelayCommand((obj) =>
            {
                if (!(obj is CustomerInfo model)) return;

                if (model.Status == 0)
                {
                    MessageBox.Show("该客户已是启用状态");
                    return;
                }

                try
                {
                    model.Status = 0;
                    model.LastUpdateTime = DateTime.Now;
                    if (repository.Update(model))
                    {
                        MessageBox.Show("已启用");
                        GetCustomers();
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
        public void SaveRow(CustomerInfo model)
        {
            if (model == null) return;

            try
            {
                model.LastUpdateTime = DateTime.Now;
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
                if (!(obj is CustomerInfo model)) return;
                if (model.Status == 1)
                {
                    MessageBox.Show("该客户已是禁用状态");
                    return;
                }


                var result = MessageBox.Show(
                    $"确定删除客户【{model.CustomerName}】吗？",
                    "删除确认",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;

                try
                {
                    repository.Delete(model.CustomerId);
                    GetCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败：" + ex.Message);
                }
            });
        }

        private void GetCustomers()
        {
            var exp = Expressionable.Create<CustomerInfo>();

            if (!string.IsNullOrWhiteSpace(SearchName))
            {
                exp.And(it => it.CustomerName.Contains(SearchName));
            }
            if (SexText == "男")
            {
                exp.And(it => it.Sex == true);
            }
            else if (SexText == "女")
            {
                exp.And(it => it.Sex == false);
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

                Customers = new ObservableCollection<CustomerInfo>(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询客户失败：" + ex.Message);
            }
        }
    }
}
