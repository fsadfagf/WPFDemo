using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using 银行管理系统WPF.View;

namespace 银行管理系统WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 登录成功后由 LoginViewModel 自行打开主窗口，这里只负责登录失败/取消时退出程序
            Login login = new Login();
            if (login.ShowDialog() != true)
            {
                Application.Current.Shutdown();
            }

        }

    }
}
