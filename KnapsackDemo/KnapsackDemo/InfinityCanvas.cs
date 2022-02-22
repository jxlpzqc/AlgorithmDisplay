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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KnapsackDemo
{
    /// <summary>    
    /// 一个实现无限画布的自定义类   
    /// </summary>
    /// 
    [ContentProperty("Content")]
    [TemplatePart(Name = "PART_Border", Type = typeof(Border))]
    public class InfinityCanvas : Control
    {
        static InfinityCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InfinityCanvas), new FrameworkPropertyMetadata(typeof(InfinityCanvas)));
        }

        private Border border;


        /// <summary>
        /// 画布顶端偏移量
        /// </summary>
        public double TopOffset
        {
            get { return (double)GetValue(TopOffsetProperty); }
            set { SetValue(TopOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopOffsetProperty =
            DependencyProperty.Register("TopOffset", typeof(double), typeof(InfinityCanvas), new PropertyMetadata(0.0));


        /// <summary>
        /// 画布左端偏移量
        /// </summary>
        public double LeftOffset
        {
            get { return (double)GetValue(LeftOffsetProperty); }
            set { SetValue(LeftOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftOffsetProperty =
            DependencyProperty.Register("LeftOffset", typeof(double), typeof(InfinityCanvas), new PropertyMetadata(0.0));


        /// <summary>
        /// 画布缩放
        /// </summary>
        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Scale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(InfinityCanvas), new PropertyMetadata(1.0));


        // 子元素
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // 子元素依赖属性
        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(InfinityCanvas));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // 在应用模板时获取border
            border = GetTemplateChild("PART_Border") as Border;
            if (border != null)
            {
                border.MouseMove += Border_MouseMove;
                border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
                border.MouseLeftButtonUp += Border_MouseLeftButtonUp;
                border.MouseWheel += Border_MouseWheel;
            }

        }

        private bool IsDrag = false;
        private double spanLeft = 0;
        private double spanTop = 0;


        /// <summary>
        /// 鼠标移动的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrag)
            {
                // 如果正在拖动，改变Top和Left
                this.TopOffset = (e.GetPosition(border).Y - spanTop);
                this.LeftOffset = (e.GetPosition(border).X - spanLeft);

            }
        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //释放鼠标捕获
            border.ReleaseMouseCapture();
            IsDrag = false;
            spanLeft = spanTop = 0;
        }

        /// <summary>
        /// 鼠标按下的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(border);
            IsDrag = true;
            spanLeft =   e.GetPosition(border).X - this.LeftOffset;
            spanTop = e.GetPosition(border).Y - this.TopOffset;
        }

        /// <summary>
        /// 滚轮缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mm =  this.Scale + (double)e.Delta/1000;
            this.Scale = Math.Max(0, mm);
        }

    }
}
