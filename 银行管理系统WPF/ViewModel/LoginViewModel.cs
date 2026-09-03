using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 银行管理系统WPF;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.Repository;
using 银行管理系统WPF.Utilities;
using 银行管理系统WPF.View;

namespace 银行管理系统WPF.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UserInfoRepository userInfoRepository;
        private readonly Window window;

        public LoginViewModel(UserInfoRepository userInfoRepository, Window window)
        {
            this.userInfoRepository = userInfoRepository;
            this.window = window;
        }

        private string account;
        public string Account
        {
            get => account;
            set { account = value; OnPropertyChanged(); }
        }

        private string password;
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 登录：校验输入 -> 查询账号密码 -> 记录登录用户 -> 打开主窗口并关闭登录窗
        /// </summary>
        public RelayCommand LoginCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (string.IsNullOrWhiteSpace(Account))
                    {
                        MessageBox.Show("账号不能为空");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        MessageBox.Show("密码不能为空");
                        return;
                    }

                    var exp = Expressionable.Create<UserInfo>();
                    exp.And(it => it.Account == Account);
                    exp.And(it => it.Password == Password);

                    try
                    {
                        var user = userInfoRepository.GetList(exp).FirstOrDefault();
                        if (user == null)
                        {
                            MessageBox.Show("账号或密码错误");
                            return;
                        }

                        // 记录当前登录用户，供主窗口状态栏等使用
                        LoginInfo.CurrentUser = user;

                        // 打开主窗口，再关闭登录窗口（必须先 Show 再关，否则程序会直接退出）
                        new MainWindow().Show();
                        window.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("登录失败：" + ex.Message);
                    }
                });
            }
        }

        /// <summary>
        /// 退出：直接结束整个应用程序
        /// </summary>
        public RelayCommand ExitCommand
        {
            get => new RelayCommand((obj) => 
            {
            Application.Current.Shutdown();
            
            });


        }
    }
}
