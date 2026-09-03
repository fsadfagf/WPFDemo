using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace 银行管理系统WPF.usercontrols
{
    /// <summary>
    /// 分页控件：对外暴露 TotalPage / PageIndex / TotalCount 三个依赖属性，
    /// 翻页时触发 PageChanged 路由事件（冒泡），外部可用 Interaction 转命令，也可直接订阅。
    /// </summary>
    public partial class ucPage : UserControl
    {
        public ucPage()
        {
            InitializeComponent();
            Loaded += (s, e) => UpdateButtons();
        }

        #region 依赖属性

        /// <summary>
        /// 总页数
        /// </summary>
        public static readonly DependencyProperty TotalPageProperty = DependencyProperty.Register(
            "TotalPage", typeof(int), typeof(ucPage),
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPageInfoChanged));

        public int TotalPage
        {
            get => (int)GetValue(TotalPageProperty);
            set => SetValue(TotalPageProperty, value);
        }

        /// <summary>
        /// 当前页码（从 1 开始）
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
            "PageIndex", typeof(int), typeof(ucPage),
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPageInfoChanged));

        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public static readonly DependencyProperty TotalCountProperty = DependencyProperty.Register(
            "TotalCount", typeof(int), typeof(ucPage),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPageInfoChanged));

        public int TotalCount
        {
            get => (int)GetValue(TotalCountProperty);
            set => SetValue(TotalCountProperty, value);
        }

        private static void OnPageInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ucPage)d).UpdateButtons();
        }

        #endregion

        #region PageChanged 路由事件

        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent(
            "PageChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<int>),
            typeof(ucPage));

        public event RoutedPropertyChangedEventHandler<int> PageChanged
        {
            add => AddHandler(PageChangedEvent, value);
            remove => RemoveHandler(PageChangedEvent, value);
        }

        #endregion

        /// <summary>
        /// 翻页：修正边界 -> 写入 PageIndex -> 触发 PageChanged
        /// </summary>
        private void GoTo(int newIndex)
        {
            if (newIndex < 1) newIndex = 1;
            if (TotalPage > 0 && newIndex > TotalPage) newIndex = TotalPage;
            if (newIndex == PageIndex) return;

            int oldIndex = PageIndex;
            PageIndex = newIndex;
            UpdateButtons();

            RaiseEvent(new RoutedPropertyChangedEventArgs<int>(oldIndex, newIndex, PageChangedEvent));
        }

        /// <summary>
        /// 首页/末页时禁用对应按钮
        /// </summary>
        private void UpdateButtons()
        {
            if (BtnFirst == null) return;

            BtnFirst.IsEnabled = PageIndex > 1;
            BtnPrev.IsEnabled = PageIndex > 1;
            BtnNext.IsEnabled = PageIndex < TotalPage;
            BtnLast.IsEnabled = PageIndex < TotalPage;
        }

        private void BtnFirst_Click(object sender, RoutedEventArgs e) => GoTo(1);

        private void BtnPrev_Click(object sender, RoutedEventArgs e) => GoTo(PageIndex - 1);

        private void BtnNext_Click(object sender, RoutedEventArgs e) => GoTo(PageIndex + 1);

        private void BtnLast_Click(object sender, RoutedEventArgs e) => GoTo(TotalPage);

        private void BtnJump_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TxtJump.Text, out int page))
            {
                GoTo(page);
            }
        }
    }
}
