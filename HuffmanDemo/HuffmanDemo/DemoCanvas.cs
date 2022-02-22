using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace HuffmanDemo
{
    /// <summary>
    /// 一个抽象类，表明了一个画布中的容器
    /// </summary>
    public abstract class DemoCanvas : DemoComponent
    {
        protected Canvas Canvas => this.Content as Canvas;

        public DemoCanvas()
        {
            this.Content = new Canvas();
        }

        public async Task MoveComponent(DemoComponent component, int destX, int destY)
        {
            await component.MoveTo(destX, destY);
        }

        public static double Scale { get; set; } = 1.2;

        public double ToCanvasSize(int realSize)
        {
            return realSize / Scale;

        }

        /// <summary>
        /// 得到画布一个不会影响其他控件的空间
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Point GetSpace(int width,int height)
        {
            throw new NotImplementedException("Not implement!");
        }

        public void SetRed()
        {

            this.Canvas.Background = new SolidColorBrush(Colors.Red);
        }

        public void SetTransparent()
        {
            this.Canvas.Background = new SolidColorBrush(Colors.Transparent);
        }

        public void AddComponent(DemoComponent component)
        {
            Canvas.Children.Add(component);
            RebindComponent(component);
        }

        public void RebindComponent(DemoComponent component)
        {
            var xbinding = new Binding()
            {
                Source = component,
                Path = new System.Windows.PropertyPath("RealX"),
                Mode = BindingMode.OneWay,
                Converter = new ToCanvasSizeConverter(this)
            };
            var ybinding = new Binding()
            {
                Source = component,
                Path = new System.Windows.PropertyPath("RealY"),
                Mode = BindingMode.OneWay,
                Converter = new ToCanvasSizeConverter(this)
            };
            var wbinding = new Binding()
            {
                Source = component,
                Path = new System.Windows.PropertyPath("RealWidth"),
                Mode = BindingMode.OneWay,
                Converter = new ToCanvasSizeConverter(this)
            };
            var hbinding = new Binding()
            {
                Source = component,
                Path = new System.Windows.PropertyPath("RealHeight"),
                Mode = BindingMode.OneWay,
                Converter = new ToCanvasSizeConverter(this)
            };

            component.SetBinding(Canvas.LeftProperty, xbinding);
            component.SetBinding(Canvas.TopProperty, ybinding);
            component.SetBinding(WidthProperty, wbinding);
            component.SetBinding(HeightProperty, hbinding);
        }

        public void RemoveComponent(DemoComponent component)
        {
            Canvas.Children.Remove(component);
            
        }

        public void ClearCanvas()
        {
            this.Canvas.Children.Clear();
        }


        internal class ToCanvasSizeConverter : IValueConverter
        {
            private DemoCanvas Ref;

            public ToCanvasSizeConverter(DemoCanvas ref_)
            {
                Ref = ref_;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return Ref.ToCanvasSize((int)value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return Binding.DoNothing;
            }
        }

        public override void Repaint()
        {
            base.Repaint();
            foreach (var item in Canvas.Children)
            {
                if(item is DemoComponent c)
                {
                    c.Repaint();

                    RebindComponent(c);
                }

            }

        }

    }


}