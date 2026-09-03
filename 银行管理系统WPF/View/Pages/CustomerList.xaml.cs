using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 银行管理系统WPF.Repository;
using 银行管理系统WPF.ViewModel;

namespace 银行管理系统WPF.View.Pages
{
    /// <summary>
    /// CustomerList.xaml 的交互逻辑
    /// </summary>
    public partial class CustomerList:Page
    {
        public CustomerList()
        {
            InitializeComponent();
            this.DataContext = new CustomerViewModel(new CustomerRepository());
        }

        /// <summary>
        /// 单元格结束编辑（光标离开、回车等）时自动把改动写回数据库
        /// </summary>
        private void CustomerDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // 取消编辑（Esc）时不保存
            if (e.EditAction != DataGridEditAction.Commit) return;

            // 先把编辑框里的值强制刷回绑定的实体，否则拿到的还是旧值
            if (e.EditingElement is TextBox textBox)
            {
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }

            (DataContext as CustomerViewModel)?.SaveRow(e.Row.Item as Models.CustomerInfo);
        }
    }
}
