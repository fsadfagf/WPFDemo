using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.Utilities;

namespace 银行管理系统WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private UserInfo currentUser = LoginInfo.CurrentUser;
        public UserInfo CurrentUser
        {
            get => currentUser;
            set { currentUser = value; OnPropertyChanged(); }
        }

        private string currentPage = "View/Pages/UserList.xaml";
        public string CurrentPage
        {
            get => currentPage;
            set { currentPage = value; OnPropertyChanged(); }
        }
        public RelayCommand OpenPageCommand
        {
            get => new RelayCommand((obj) =>
            {
                CurrentPage = obj.ToString();
            });
        }

        public RelayCommand ClosePageCommand
        {
            get => new RelayCommand((obj) =>
            {
                Application.Current.Shutdown();
            });

        }


    }
}
