using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HuffmanDemo
{
    /// <summary>
    /// 一个抽象类，表明一个画布上的控件，封装了一些基本方法
    /// </summary>
    public abstract class DemoComponent : UserControl
    {

        public int RealX
        {
            get { return (int)GetValue(RealXProperty); }
            set { SetValue(RealXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RealX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RealXProperty =
            DependencyProperty.Register("RealX", typeof(int), typeof(DemoComponent), new PropertyMetadata(0));




        public int RealY
        {
            get { return (int)GetValue(RealYProperty); }
            set { SetValue(RealYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RealY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RealYProperty =
            DependencyProperty.Register("RealY", typeof(int), typeof(DemoComponent), new PropertyMetadata(0));



        public int RealHeight
        {
            get { return (int)GetValue(RealHeightProperty); }
            set { SetValue(RealHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RealHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RealHeightProperty =
            DependencyProperty.Register("RealHeight", typeof(int), typeof(DemoComponent), new PropertyMetadata(0));


        public int RealWidth
        {
            get { return (int)GetValue(RealWidthProperty); }
            set { SetValue(RealWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RealWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RealWidthProperty =
            DependencyProperty.Register("RealWidth", typeof(int), typeof(DemoComponent), new PropertyMetadata(0));


        public async Task MoveTo(int x, int y, bool needDelay = true)
        {
            int delay = 1000;

            var animationX = new Int32Animation(x, new Duration(TimeSpan.FromMilliseconds(delay)));
            BeginAnimation(RealXProperty, animationX);


            var animationY = new Int32Animation(y, new Duration(TimeSpan.FromMilliseconds(delay)));
            BeginAnimation(RealYProperty, animationY);

            if (needDelay) await Task.Delay(delay);
        }

        public virtual void Repaint() { }

    }
}