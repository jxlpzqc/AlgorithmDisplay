using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KnapsackDemo
{
    /// <summary>
    /// 绘图类
    /// </summary>
    class SolutionTreePainter
    {
        NodeDrawingOptions options = new NodeDrawingOptions();
        Canvas panel;

        public string[] names;

        int level;

        public SolutionTreePainter(Canvas panel, int level)
        {
            this.panel = panel;
            this.level = level;
        }

        /// <summary>
        /// 绘制一颗满二叉树
        /// </summary>
        public void Paint()
        {
            // 先清空所有节点，然后进行绘图
            panel.Children.Clear();

            for (int i = 1; i <= level; i++)
            {
                // 该层节点左偏移
                var halfw = options.Gap / 2 * (1 << (level - i));
                var starty = options.HorizentalSpacing * (i - 1);
                for (int j = 0; j < (1 << (i - 1)); j++)
                {
                    var area = this.CreateNodeShape();

                    var x = halfw * (2 * j + 1);
                    area.SetValue(Canvas.LeftProperty, (double)x);
                    area.SetValue(Canvas.TopProperty, (double)starty);

                    // 左线
                    var line1 = new Line
                    {
                        Stroke = options.LineStroke,
                        StrokeThickness = options.LineThickness,
                        X1 = x + options.Diameter / 2,
                        Y1 = starty + options.Diameter / 2,
                        X2 = x - halfw / 2 + options.Diameter / 2,
                        Y2 = starty + options.HorizentalSpacing + options.Diameter / 2
                    };

                    // 右线
                    var line2 = new Line
                    {
                        Stroke = options.LineStroke,
                        StrokeThickness = options.LineThickness,
                        X1 = x + options.Diameter / 2,
                        Y1 = starty + options.Diameter / 2,
                        X2 = x + halfw / 2 + options.Diameter / 2,
                        Y2 = starty + options.HorizentalSpacing + options.Diameter / 2
                    };

                    // 按顺序加入，后期好定位， 两条线+一个节点
                    panel.Children.Add(line1);
                    panel.Children.Add(line2);

                    // 如果是最后一层，隐藏所有线条
                    if (i == level)
                    {
                        line1.Visibility = Visibility.Hidden;
                        line2.Visibility = Visibility.Hidden;
                    }

                    panel.Children.Add(area);
                }

            }

            PaintLineAndText();

            AdjustLayout();
        }

        private void PaintLineAndText()
        {
            var fullw = options.Gap * (1 << (level - 1));
            for (int i = 0; i < level-1; i++)
            {
                var starty = options.HorizentalSpacing * i;

                var line = new Line
                {
                    Stroke = options.SplitLineStroke,
                    StrokeThickness = options.LineThickness,
                    StrokeDashArray = new DoubleCollection { 2, 2 },
                    X1 = 10,
                    Y1 = starty + options.HorizentalSpacing / 2 + options.Diameter /2,
                    X2 = fullw,
                    Y2 = starty + options.HorizentalSpacing / 2 + options.Diameter / 2
                };

                string name = "<NULL>";
                if (names != null && names.Length > i) 
                    name = names[i]; 

                var text = new TextBlock()
                {
                    Text = name
                };
                text.FontSize = 22;
                text.SetValue(Canvas.LeftProperty, 0.0);
                text.SetValue(Canvas.TopProperty, (double)(starty + options.HorizentalSpacing / 2 + 10) );

                panel.Children.Add(line);
                panel.Children.Add(text);
            }
        }

        /// <summary>
        /// 调整布局
        /// </summary>
        private void AdjustLayout()
        {
            panel.UpdateLayout();
        }

        /// <summary>
        /// 高亮某个节点
        /// </summary>
        /// <param name="id"></param>
        public void HighlightNode(int[] id)
        {
            int p = 1;
            for (int i = 0; i < id.Length; i++)
            {
                if (id[i] == 0)
                {
                    p = p * 2;
                }
                else
                {
                    p = p * 2 + 1;
                }

            }

            HighlightNode(p);


        }

        // 正在高亮的编号
        private int highlighting = 0;

        /// <summary>
        ///  高亮节点
        /// </summary>
        private void HighlightNode(int p)
        {
            if (highlighting != 0)
                DeHighlightNode(highlighting);

            var index = (p * 3 - 1);

            highlighting = p;

            var el = (panel.Children[index] as Canvas).Children[0] as Ellipse;
            if (el != null)
                el.Fill = options.HighlightFill;


        }

        /// <summary>
        /// 返高亮节点
        /// </summary>
        /// <param name="h"></param>
        private void DeHighlightNode(int h)
        {
            var index = (h * 3 - 1);
            var el = (panel.Children[index] as Canvas).Children[0] as Ellipse;
            if (el != null)
                el.Fill = options.Fill;
            this.highlighting = 0;
        }


        /// <summary>
        /// 对某个节点进行剪枝，即将其和其子节点全部变成灰色
        /// </summary>
        public void CutBranch(int[] id)
        {
            int p = 1;
            for (int i = 0; i < id.Length; i++)
            {
                if (id[i] == 0)
                {
                    p *= 2;
                }
                else
                {
                    p = p * 2 + 1;
                }
            }
            CutBranch(p);
        }


        private void CutBranch(int p)
        {

            var index = (p * 3 - 1);

            if (highlighting == p) highlighting = 0;

            Ellipse el = (panel.Children[index] as Canvas).Children[0] as Ellipse;
            if (el != null)
                el.Fill = options.DisabledFill;

            if (p * 2 < (1 << level))
            {
                CutBranch(p * 2);
                CutBranch(p * 2 + 1);
            }

        }


        Canvas CreateNodeShape()
        {
            var area = new Canvas();
            var orb = new Ellipse
            {
                Width = options.Diameter,
                Height = options.Diameter,
                Stroke = options.Stroke,
                Fill = options.Fill
            };
            area.Children.Add(orb);

            var tb = new TextBlock
            {
                Text = "",
                FontSize = options.FontSize
            };
            tb.SetValue(Canvas.LeftProperty, 15.0);
            area.Children.Add(tb);

            return area;
        }

    }

    /// <summary>
    /// 节点绘图属性
    /// </summary>
    class NodeDrawingOptions
    {
        /// <summary>
        /// 节点间隔
        /// </summary>
        public int Gap { get; set; }


        /// <summary>
        /// 节点垂直间隔
        /// </summary>
        public int HorizentalSpacing { get; set; }

        /// <summary>
        /// 描边颜色
        /// </summary>
        public Brush Stroke { get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public Brush Fill { get; set; }


        /// <summary>
        /// 填充颜色
        /// </summary>
        public Brush HighlightFill { get; set; }

        /// <summary>
        /// 节点直径
        /// </summary>
        public double Diameter { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; }


        /// <summary>
        /// 前景色
        /// </summary>
        public Brush ForeGround { get; set; }

        /// <summary>
        /// 线条粗细
        /// </summary>
        public int LineThickness { get; set; }


        /// <summary>
        /// 线条颜色
        /// </summary>
        public Brush LineStroke { get; set; }

        /// <summary>
        /// 剪枝后的颜色
        /// </summary>
        public Brush DisabledFill { get; set; }

        /// <summary>
        /// 分隔线条颜色
        /// </summary>
        public Brush SplitLineStroke { get; set; }

        /// <summary>
        /// 默认构造函数，初始化所有参数
        /// </summary>
        public NodeDrawingOptions()
        {
            this.FontSize = 14;
            this.Diameter = 50;
            this.Fill = Brushes.Coral;
            this.Stroke = Brushes.Black;
            this.ForeGround = Brushes.Black;
            this.LineStroke = Brushes.Blue;
            this.HighlightFill = Brushes.GreenYellow;
            this.DisabledFill = Brushes.Gray;
            this.SplitLineStroke = Brushes.Gray;
            this.LineThickness = 3;
            this.Gap = 70;
            this.HorizentalSpacing = 90;
        }
    }



}
