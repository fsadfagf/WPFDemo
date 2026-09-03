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
using System.Windows.Shapes;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.ViewModel;

namespace 银行管理系统WPF.View
{
    /// <summary>
    /// AddandUpdateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddandUpdateWindow : Window
    {
        /// <summary>
        /// model 传 null 表示新增客户；传入已有客户表示修改
        /// </summary>
        public AddandUpdateWindow(CustomerInfo model = null)
        {
            InitializeComponent();
            this.DataContext = new AddandEidtViewModel(this, model);
        }
    }
}
