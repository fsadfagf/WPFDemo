using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using 手写MVVM.Helper;


namespace 手写MVVM.Helper
{
        /// <summary>
        /// 提供一个辅助工具，用于使PasswordBox的Password属性能够进行数据绑定。
        /// </summary>
        public class PasswordBoxHelper
        {
            /// <summary>
            /// 注册附加依赖属性"Password"，类型为string，当其值改变时触发OnPasswordPropertyChanged方法
            /// </summary>
            public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
                "Password",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

            /// <summary>
            /// 注册附加依赖属性"Attach"，表示是否需要启用密码绑定功能，当其值改变时触发Attach方法
            /// </summary>
            public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached(
                "Attach",
                typeof(bool),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(false, Attach));

            /// <summary>
            /// 私有静态依赖属性"IsUpdating"，用于跟踪PasswordBox密码更新状态
            /// </summary>
            private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
                "IsUpdating",
                typeof(bool), typeof(PasswordBoxHelper));

            /// <summary>
            /// 设置附加属性"Attach"的值
            /// </summary>
            public static void SetAttach(DependencyObject dp, bool value)
            {
                dp.SetValue(AttachProperty, value);
            }

            /// <summary>
            /// 取附加属性"Attach"的值
            /// </summary>
            public static bool GetAttach(DependencyObject dp)
            {
                return (bool)dp.GetValue(AttachProperty);
            }

            /// <summary>
            /// 获取附加属性"Password"的值
            /// </summary>
            public static string GetPassword(DependencyObject dp)
            {
                return (string)dp.GetValue(PasswordProperty);
            }

            /// <summary>
            /// 置附加属性"Password"的值
            /// </summary>
            public static void SetPassword(DependencyObject dp, string value)
            {
                dp.SetValue(PasswordProperty, value);
            }

            /// <summary>
            /// 获取附加属性"IsUpdating"的值
            /// </summary>
            private static bool GetIsUpdating(DependencyObject dp)
            {
                return (bool)dp.GetValue(IsUpdatingProperty);
            }

            /// <summary>
            /// 设置附加属性"IsUpdating"的值
            /// </summary>
            private static void SetIsUpdating(DependencyObject dp, bool value)
            {
                dp.SetValue(IsUpdatingProperty, value);
            }

            /// <summary>
            /// 当"Password"属性发生变化时调用此方法，同步PasswordBox的实际密码与绑定值
            /// </summary>
            private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {
                if (sender is PasswordBox passwordBox)
                {
                    passwordBox.PasswordChanged -= PasswordChanged;

                    // 防止在更新过程中引发无限循环
                    if (!(bool)GetIsUpdating(passwordBox))
                    {
                        passwordBox.Password = (string)e.NewValue;
                    }
                    passwordBox.PasswordChanged += PasswordChanged;
                }
            }

            /// <summary>
            /// 当"Attach"属性值变化时（即绑定或解绑事件处理程序）调用此方法
            /// </summary>
            private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {

            PasswordBox passwordBox= sender as PasswordBox;

            if (passwordBox == null)
             return;


                if ((bool)e.OldValue)
                {
                    passwordBox.PasswordChanged -= PasswordChanged;
                }
                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordChanged;
                }
            }

            /// <summary>
            /// 密码更改事件处理程序，当PasswordBox的密码发生更改时，更新绑定到Password属性的值
            /// </summary>
            private static void PasswordChanged(object sender, RoutedEventArgs e)
            {
                if (sender is PasswordBox passwordBox)
                {
                    SetIsUpdating(passwordBox, true);
                    SetPassword(passwordBox, passwordBox.Password);
                    SetIsUpdating(passwordBox, false);
                }
            }
        }

}
//总结工作流程

//    在 XAML 中设置 helper:PasswordBoxHelper.Attach = "True" 和绑定 helper:PasswordBoxHelper.Password = "{Binding ViewModelPassword}"。

//    当 Attach 变为 true 时，注册 PasswordChanged 事件。

//    用户输入 → PasswordChanged → 更新附加属性 Password → 触发绑定 → 更新 ViewModel。

//    ViewModel 改变 → 附加属性 Password 变化 → OnPasswordPropertyChanged → 更新 PasswordBox 实际密码（但不触发事件循环）。